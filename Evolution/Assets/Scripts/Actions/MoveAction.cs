using Evolution.Character;
using Evolution.Map;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

		private Agent Agent;
		private Vector2 destination;
		private Stack<(int, int)> path = null;
		private PathStatus pathStatus = PathStatus.Searching;
		private float startTime = 0;
		private Vector3 invalidVector = new Vector3(-999, -999, -999);
		private Vector3 targetPos;
		private bool searchCanceled = false;
		private CancellationToken CancellationToken = new CancellationToken(true);

		public MoveAction(Agent agent, Vector2 destination)
		{
			this.destination = destination;
			Agent = agent;
		}

		public override void OnStart()
		{
			base.OnStart();
			int posX = (int)(Mathf.Round(Agent.Transform.position.x));
			int posY = (int)(Mathf.Round(Agent.Transform.position.y));
			startTime = Time.time;
			targetPos = invalidVector;
			searchCanceled = false;
			SearchForPathAsync(posX, posY);
		}

		private async void SearchForPathAsync(int posX, int posY)
		{
			Task t = Task.Run(async () => SearchForPath(posX, posY));
			await t;
		}

		private void SearchForPath(int posX, int posY)
		{
			Description = "Searching for path...";
			path = AStarSearch(Game.Instance.MapManager.MapGraph, new Vector2(posX, posY), destination);
			if (path != null)
			{
				Description = "Moving to new position";
				pathStatus = PathStatus.Found;
			}
			else
			{
				//Mark the position as invalid
				Agent.Brain.MarkInvalidPoint((int)destination.x, (int)destination.y);
				pathStatus = PathStatus.Failed;
				Debug.LogError("There was no path between the given points!");
			}
		}

		public override ActionStatus OnUpdate(float time)
		{
			//We wasted too much time on searching for a path...move on
			if (pathStatus == PathStatus.Searching && Time.time - startTime > 0.6f)
			{
				Debug.LogError("Too much time to find a path!!!");
				searchCanceled = true;
				//Status = ActionStatus.FAILED;
				return ActionStatus.FAILED;
			}

			if (pathStatus == PathStatus.Failed)
				return ActionStatus.FAILED;
			if (pathStatus == PathStatus.Found)
			{
				if (path.Count == 0)
					return ActionStatus.SUCCESSFULLY_EXECUTED;

				if (targetPos == invalidVector)
				{
					var position = path.Peek();
					targetPos = GetRandomTilePosition(position);
				}

				if (Agent.Transform.position != targetPos)
				{
					Agent.Transform.position = Vector3.MoveTowards(Agent.Transform.position, targetPos, Constants.AGENT_SPEED_HOUR * time);
				}
				else
				{
					var position = path.Pop();
					targetPos = GetRandomTilePosition(position);
				}
			}

			return ActionStatus.IN_PROGRESS;
		}

		private Vector3 GetRandomTilePosition((int, int) position)
		{
			var xOffset = Random.Range(-0.4f, 0.4f);
			var yOffset = Random.Range(-0.4f, 0.4f);

			var newPosition = new Vector3(position.Item1 + xOffset, position.Item2 + yOffset);
			return newPosition;
		}

		private HashSet<AStarNode> nodes = new HashSet<AStarNode>();
		private HashSet<(int, int)> visitedNodes = new HashSet<(int, int)>();
		private HashSet<(int, int)> nodesEntries = new HashSet<(int, int)>();

		private Stack<(int, int)> finalPath = new Stack<(int, int)>();
		private Stack<(int, int)> AStarSearch(MapGraph graph, Vector2 startPosition, Vector2 endPosition)
		{
			var startNode = graph.GetNode((int)startPosition.x, (int)startPosition.y);
			var endNode = graph.GetNode((int)endPosition.x, (int)endPosition.y);

			if (startNode == null || endNode == null)
			{
				Debug.LogError("Invalid start or end node!");
				return null;
			}
			nodes.Clear();
			visitedNodes.Clear();
			nodesEntries.Clear();
			finalPath.Clear();
			var currentNode = new AStarNode(null, startNode.xPosition, startNode.yPosition, 0, GetDistance(startNode.xPosition, startNode.yPosition, endNode.xPosition, endNode.yPosition));
			nodes.Add(currentNode);
			nodesEntries.Add((startNode.xPosition, startNode.yPosition));

			while (nodes.Count > 0)
			{
				if (searchCanceled)
				{
					Debug.LogError("Task successfully stopped with force");
					break;
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

				visitedNodes.Add((bestNode.xPosition, bestNode.yPosition));
				nodes.Remove(bestNode);
				nodesEntries.Remove((bestNode.xPosition, bestNode.yPosition));
				AddNeighbours(ref nodes, visitedNodes, graph, bestNode, endNode);
			}

			Debug.LogError("No path was found!!!");
			return null;
		}

		private void AddNeighbours(ref HashSet<AStarNode> nodes, HashSet<(int, int)> visitedNodes, MapGraph graph, AStarNode aStarNode, Node endNode)
		{
			var node = graph.GetNode(aStarNode.xPosition, aStarNode.yPosition);
			var neighbours = graph.GetNodeNeighbours(node);
			foreach (var neighbour in neighbours)
			{
				if (!visitedNodes.Contains((neighbour.xPosition, neighbour.yPosition)) && !nodesEntries.Contains((neighbour.xPosition, neighbour.yPosition)))
				{
					nodesEntries.Add((neighbour.xPosition, neighbour.yPosition));
					if (neighbour.xPosition == node.xPosition || neighbour.yPosition == node.yPosition)
					{
						nodes.Add(new AStarNode(aStarNode, neighbour.xPosition, neighbour.yPosition, aStarNode.DistanceFromStart + 1, GetDistance(neighbour.xPosition, neighbour.yPosition, endNode.xPosition, endNode.yPosition)));
					}
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

		public override void OnApllicationQuit()
		{
			base.OnApllicationQuit();
			searchCanceled = true;
		}
	}
}
