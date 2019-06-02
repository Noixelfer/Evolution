using Evolution.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Map
{
	public class MapGraph
	{
		private Dictionary<(int, int), Node> Nodes = new Dictionary<(int, int), Node>();

		public void AddNode(int x, int y)
		{
			if (Nodes.ContainsKey((x, y)))
				return;
			Nodes.Add((x, y), new Node(x, y));
		}

		public bool RemoveNode(int x, int y)
		{
			return Nodes.Remove((x, y));
		}

		public MapGraph(Map map)
		{
			GenerateGraphFromMap(map);
		}

		private void GenerateGraphFromMap(Map map)
		{
			for (int x = 0; x < map.Size.x; x++)
			{
				for (int y = 0; y < map.Size.y; y++)
				{
					var tile = (map.GetTileValue(new Vector2(x, y)));
					if (!tile.Occupied)
						Nodes.Add((x, y), new Node(x, y));
				}
			}
		}

		public List<Node> GetNodeNeighbours(Node node)
		{
			if (node == null)
			{
				Debug.LogError("Can not get neighbours for null node!!!");
				return null;
			}
			var result = new List<Node>();
			foreach (var offset in MapConstants.neighboursOffset)
			{
				var neighbourNode = GetNode(node.xPosition + offset.Item1, node.yPosition + offset.Item2);
				if (neighbourNode != null)
					result.Add(neighbourNode);
			}
			return result;
		}

		public Node GetNode(int x, int y)
		{
			if (Nodes.ContainsKey((x, y)))
				return Nodes[(x, y)];
			return null;
		}
	}

	public class Node
	{
		public Node(int xPosition, int yPosition)
		{
			this.xPosition = xPosition;
			this.yPosition = yPosition;
		}

		public int xPosition;
		public int yPosition;
	}
}
