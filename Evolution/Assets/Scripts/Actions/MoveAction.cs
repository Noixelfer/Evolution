using Evolution.Character;
using Evolution.Map;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Evolution.Actions
{
	public class MoveAction : BaseAction
	{
		public override string ID => "action.move";
		public enum PathStatus
		{
			Searching = 0,
			Found = 1,
			Failed = 2
		}


		private IAgent Agent;
		private Vector2 destination;
		private Stack<(int, int)> path = null;
		private PathStatus pathStatus = PathStatus.Searching;
		private BackgroundWorker pathFindWorker;
		private float startTime = 0;

		public MoveAction(IAgent agent, Vector2 destination)
		{
			this.destination = destination;
			Agent = agent;
			pathFindWorker = new BackgroundWorker();
		}

		public override void OnStart()
		{
			base.OnStart();
			int posX = (int)Agent.Transform.position.x;
			int posY = (int)Agent.Transform.position.y;

			//Init background worker
			pathFindWorker.DoWork += findPathWorker_DoWork;
			pathFindWorker.RunWorkerCompleted += findPathWorker_RunWorkerCompleted;
			pathFindWorker.WorkerReportsProgress = true;
			pathFindWorker.WorkerSupportsCancellation = true;
			pathFindWorker.RunWorkerAsync(argument: new int[] { posX, posY });
			startTime = Time.time;
		}

		private void findPathWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			int[] positions = new int[2];
			bool badArguments = false;
			if (e.Argument == null)
			{
				badArguments = true;
			}
			else
			{
				positions = (int[])e.Argument;
				if (positions == null || positions.Length != 2)
				{
					badArguments = true;
				}
			}
			if (badArguments)
			{
				pathStatus = PathStatus.Failed;
				e.Cancel = true;
				pathFindWorker.ReportProgress(100);
				return;
			}

			int posX = positions[0];
			int posY = positions[1];

			path = AStarSearch(Game.Instance.MapManager.MapGraph, new Vector2(posX, posY), destination);
			pathFindWorker.ReportProgress(100);
		}

		private void findPathWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			if (pathFindWorker.CancellationPending)
			{
				Debug.LogError("Path finding worker was canceled!!!");
				pathStatus = PathStatus.Failed;
			}
			else if (e.Error != null)
			{
				Debug.LogError("There was an error in finding path background work!!!   " + e.Error);
				pathStatus = PathStatus.Failed;
			}
			else
			{
				if (path != null)
				{
					pathStatus = PathStatus.Found;
				}
				else
				{
					pathStatus = PathStatus.Failed;
					Debug.LogError("There was no path between the given points!");
				}
			}
		}

		public override ActionStatus OnUpdate(float time)
		{
			//We wasted too much time on searching for a path...move on
			if (pathStatus == PathStatus.Searching && Time.time - startTime > 0.6f)
			{
				Debug.LogError("Too much time to find a path!!!");
				pathFindWorker.CancelAsync();
				//Status = ActionStatus.FAILED;
				return ActionStatus.FAILED;
			}

			if (pathStatus == PathStatus.Failed)
				return ActionStatus.FAILED;
			if (pathStatus == PathStatus.Found)
			{
				if (path.Count == 0)
					return ActionStatus.SUCCESSFULLY_EXECUTED;

				var targetPos = path.Peek();
				if (Agent.Transform.position != new Vector3(targetPos.Item1, targetPos.Item2))
				{
					Agent.Transform.position = Vector3.MoveTowards(Agent.Transform.position, new Vector3(targetPos.Item1, targetPos.Item2), 100f * time);
				}
				else
					path.Pop();
			}

			return ActionStatus.IN_PROGRESS;
		}

		private Stack<(int, int)> AStarSearch(MapGraph graph, Vector2 startPosition, Vector2 endPosition)
		{
			var startNode = graph.GetNode((int)startPosition.x, (int)startPosition.y);
			var endNode = graph.GetNode((int)endPosition.x, (int)endPosition.y);

			if (startNode == null || endNode == null)
			{
				Debug.LogError("Invalid start or end node!");
				return null;
			}

			List<AStarNode> nodes = new List<AStarNode>();
			List<AStarNode> visitedNodes = new List<AStarNode>();
			var finalPath = new Stack<(int, int)>();
			var currentNode = new AStarNode(null, startNode.xPosition, startNode.yPosition, 0, GetDistance(startNode.xPosition, startNode.yPosition, endNode.xPosition, endNode.yPosition));
			nodes.Add(currentNode);

			while (nodes.Count > 0)
			{
				if (pathFindWorker.CancellationPending)
				{
					return null;
				}
				var minDistance = nodes.Min(n => n.TotalDistance);
				var bestNode = nodes.First(n => n.TotalDistance == minDistance);

				if (bestNode.DistanceToFinish == 0)
				{
					var cNode = bestNode;
					while (cNode.FromNode != null)
					{
						finalPath.Push((cNode.xPosition, cNode.yPosition));
						cNode = cNode.FromNode;
					}
					return finalPath;
					//TODO : receive path
				}

				visitedNodes.Add(bestNode);
				nodes.Remove(bestNode);
				AddNeighbours(ref nodes, visitedNodes, graph, bestNode, endNode);
			}

			Debug.LogError("No path was found!!!");
			return null;
		}

		private void AddNeighbours(ref List<AStarNode> nodes, List<AStarNode> visitedNodes, MapGraph graph, AStarNode aStarNode, Node endNode)
		{
			var node = graph.GetNode(aStarNode.xPosition, aStarNode.yPosition);
			var neighbours = graph.GetNodeNeighbours(node);
			foreach (var neighbour in neighbours)
			{
				if (visitedNodes.Find(n => n.xPosition == neighbour.xPosition && n.yPosition == neighbour.yPosition) == null)
				{
					if (neighbour.xPosition == node.xPosition || neighbour.yPosition == node.yPosition)
						nodes.Add(new AStarNode(aStarNode, neighbour.xPosition, neighbour.yPosition, aStarNode.DistanceFromStart + 1, GetDistance(neighbour.xPosition, neighbour.yPosition, endNode.xPosition, endNode.yPosition)));
					else
						nodes.Add(new AStarNode(aStarNode, neighbour.xPosition, neighbour.yPosition, aStarNode.DistanceFromStart + 1.41f, GetDistance(neighbour.xPosition, neighbour.yPosition, endNode.xPosition, endNode.yPosition)));
				}
			}
		}

		private float GetDistance(int x, int y, int x1, int y1)
		{
			return Mathf.Sqrt(Mathf.Pow(x - x1, 2) + Mathf.Pow(y - y1, 2));
		}

		private class AStarNode
		{
			public AStarNode(AStarNode fromNode, int xPosition, int yPosition, float distanceFromStart, float distanceTofinish)
			{
				FromNode = fromNode;
				this.xPosition = xPosition;
				this.yPosition = yPosition;
				DistanceFromStart = distanceFromStart;
				DistanceToFinish = distanceTofinish;
			}

			public AStarNode FromNode;
			public int xPosition;
			public int yPosition;
			public float DistanceFromStart;
			public float DistanceToFinish;
			public float TotalDistance => DistanceFromStart + DistanceToFinish;
		}
	}
}
