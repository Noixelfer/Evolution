using UnityEngine;

namespace Evolution.Map
{
	public class Tile : MonoBehaviour, ITile
	{
		public bool Walkable { get; set; }

		public Vector3 GetRandomPosition()
		{
			var xOffset = Random.Range(-0.35f, 0.35f);
			var yOffset = Random.Range(-0.35f, 0.35f);
			return transform.position + new Vector3(xOffset, yOffset);
		}
	}
}
