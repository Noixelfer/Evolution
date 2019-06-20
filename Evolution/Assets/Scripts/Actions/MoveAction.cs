using Evolution.Character;
using Evolution.Map;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

		private const float AGENT_SPEED = 4f;
		private Agent Agent;
		private Vector2 destination;
		private Stack<(int, int)> path = null;
		private PathStatus pathStatus = PathStatus.Searching;
		private float startTime = 0;
		private Vector3 invalidVector = new Vector3(-999, -999, -999);
		private Vector3 targetPos;
		private bool searchCanceled = false;

		public MoveAction(Agent agent, Vector2 destination)
		{
			this.destination = destination;
			Agent = agent;
		}

		public override void OnStart()
		{
			base.OnStart();
			int posX = (int)Agent.Transform.position.x;
			int posY = (int)Agent.Transform.position.y;
			startTime = Time.time;
			targetPos = invalidVector;
			searchCanceled = false;
			ThreadPool.QueueUserWorkItem((_) => SearchForPath(posX, posY));
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
					Agent.Transform.position = Vector3.MoveTowards(Agent.Transform.position, targetPos, AGENT_SPEED * time);
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
			var xOffset = 0;// Random.Range(-0.45f, 0.45f);
			var yOffset = 0;// Random.Range(-0.45f, 0.45f);

			var newPosition = new Vector3((int)position.Item1 + xOffset, (int)position.Item2 + yOffset);
			return newPosition;
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
				if (searchCanceled)
				{
					Debug.LogError("Thread successfully stopped with force");
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
