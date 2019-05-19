using UnityEngine;

namespace Evolution.Map
{
	public class Tile : MonoBehaviour, ITile
	{
		public string id { get;  set; }
		public bool Walkable { get;  set; }
		public bool Occupied { get; set; }
		public Vector2 MapPosition{ get; set; }
		public Vector3 GetRandomPosition()
		{
			var xOffset = Random.Range(-0.35f, 0.35f);
			var yOffset = Random.Range(-0.35f, 0.35f);
			return transform.position + new Vector3(xOffset, yOffset);
		}
	}
}
