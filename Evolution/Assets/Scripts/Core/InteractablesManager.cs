using Evolution.Character;
using Evolution.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution
{
	public class InteractablesManager : MonoBehaviour
	{
		private Dictionary<(int, int), IInteractable> interactables = new Dictionary<(int, int), IInteractable>();
		private Dictionary<(int, int), HashSet<(IInteractable, Vector3)>> awkTreeInteractables = new Dictionary<(int, int), HashSet<(IInteractable, Vector3)>>();
		private readonly int BATCH_SIZE = 12;
		private int x;
		private int y;
		private float squaredRadius;
		private float squaredDistance;
		private HashSet<IInteractable> result = new HashSet<IInteractable>();
		private HashSet<(int, int)> awkKeys = new HashSet<(int, int)>();

		private (int, int) point1;
		private (int, int) point2;
		private (int, int) point3;
		private (int, int) point4;

		public void Register(IInteractable interactable)
		{
			var position = interactable.gameObject.transform.position;
			int xIndice = (int)(position.x / BATCH_SIZE);
			int yIndice = (int)(position.y / BATCH_SIZE);
			var key = (xIndice, yIndice);

			if (awkTreeInteractables.ContainsKey(key))
				awkTreeInteractables[key].Add((interactable, position));
			else
			{
				awkTreeInteractables.Add(key, new HashSet<(IInteractable, Vector3)>() { (interactable, position) });
			}
			{
				var agent = interactable as Agent;
				if (agent == null)
					interactables.Add(((int)(position.x), (int)(position.y)), interactable);
			}
		}

		public void Unregister(IInteractable interactable)
		{
			var key = ((int)(interactable.gameObject.transform.position.x), (int)(interactable.gameObject.transform.position.y));
			interactables.Remove(key);
		}

		public void DisableUnreachableIInteractables()
		{
			int totalDisabled = 0;
			var keys = new HashSet<(int, int)>(interactables.Keys);
			foreach (var key in keys)
			{
				foreach (var offset in MapConstants.neighboursOffset)
				{
					var key1 = (key.Item1 + offset.Item1, key.Item2 + offset.Item2);
					if (!interactables.ContainsKey(key1))
					{
						if (Game.Instance.MapManager.Map.FreeTile(new Vector2(key1.Item1, key1.Item2)))
							interactables[key].FreeNeighbourCells.Add(key1);
					}
				}

				if (interactables[key].FreeNeighbourCells.Count == 0)
				{
					interactables[key].Reachable = false;
					totalDisabled++;
				}
			}
			Debug.LogWarning("Total disabled interactables : " + totalDisabled + "from  " + interactables.Keys.Count);
		}

		/// <summary>
		/// Returns a HashSet with the IInteractables in the given radius from center
		/// Batch size must be bigger than radius!!!
		/// </summary>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public HashSet<IInteractable> GetInteractablesAround(Vector3 position, float radius)
		{
			squaredRadius = radius * radius;
			result.Clear();
			awkKeys.Clear();

			point1 = ((int)((position.x - radius) / BATCH_SIZE), (int)((position.y - radius) / BATCH_SIZE));
			point2 = ((int)((position.x - radius) / BATCH_SIZE), (int)((position.y + radius) / BATCH_SIZE));
			point3 = ((int)((position.x + radius) / BATCH_SIZE), (int)((position.y - radius) / BATCH_SIZE));
			point4 = ((int)((position.x + radius) / BATCH_SIZE), (int)((position.y + radius) / BATCH_SIZE));

			awkKeys.Add(point1);
			awkKeys.Add(point2);
			awkKeys.Add(point3);
			awkKeys.Add(point4);

			foreach (var key in awkKeys)
			{
				if (awkTreeInteractables.ContainsKey(key))
				{
					foreach (var value in awkTreeInteractables[key])
					{
						if (!value.Item1.Reachable)
							continue;
						squaredDistance = (position - value.Item2).sqrMagnitude;
						if (squaredDistance <= squaredRadius)
						{
							result.Add(value.Item1);
						}
					}
				}
			}
			return result;
		}
	}
}
