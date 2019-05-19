using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution.Map
{
	public class Map
	{
		private Vector3 size;
		private Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
		public Vector3 Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		public void ClearMap()
		{
			Tiles.Values.ToList().ForEach(tile => MonoBehaviour.Destroy(tile.gameObject));
			Tiles.Clear();
		}

		public bool FreeTile(Vector2 coordinates)
		{
			if (!Tiles.ContainsKey(coordinates))
				return true;
			return !Tiles[coordinates].Occupied;
		}

		public void SetTile(Vector2 coordinates, Tile tile)
		{
			if (Tiles.ContainsKey(coordinates))
				Tiles[coordinates] = tile;
			else
				Tiles.Add(coordinates, tile);
		}

		public Tile GetTileValue(Vector2 coordinates)
		{
			if (Tiles.ContainsKey(coordinates))
				return Tiles[coordinates];
			return null;
		}

		public void ClearTile(Vector2 coordinates)
		{
			if (!Tiles.ContainsKey(coordinates))
				return;
			MonoBehaviour.Destroy(Tiles[coordinates].gameObject);
		}
	}
}
