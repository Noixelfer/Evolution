using Evolution.Character;
using Evolution.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution.Actions
{
	public class MoveAction : BaseAction
	{
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

		public MoveAction(Agent agent, Vector2 destination)
		{
			Game.Instance.ActionsManager.Register(this);
			this.destination = destination;
			Agent = agent;
		}

		public override void OnStart()
		{
			base.OnStart();
			int posX = (int)Agent.Transform.position.x;
			int posY = (int)Agent.Transform.position.y;

			path = AStarSearch(Game.Instance.MapManager.MapGraph, new Vector2(posX, posY), destination);
			if (path != null)
				pathStatus = PathStatus.Found;
			else
				pathStatus = PathStatus.Failed;
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (pathStatus == PathStatus.Failed)
				return ActionStatus.FAILED;
			if (pathStatus == PathStatus.Found)
			{
				if (path.Count == 0)
					return ActionStatus.SUCCESSFULLY_EXECUTED;

				var targetPos = path.Peek();
				if (Agent.Transform.position != new Vector3(targetPos.Item1, targetPos.Item2))
				{
					Agent.Transform.position = Vector3.MoveTowards(Agent.Transform.position, new Vector3(targetPos.Item1, targetPos.Item2), 1f * time);
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
				var minDistance = nodes.Min(n => n.TotalDistance);
				var bestNode = nodes.First(n => n.TotalDistance == minDistance);

				if (bestNode.DistanceToFinish == 0)
				{
					Debug.Log("Path completed");
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
