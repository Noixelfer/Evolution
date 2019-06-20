using System.Collections.Generic;
using UnityEngine;

namespace Evolution
{
	public class InteractablesManager : MonoBehaviour
	{
		private HashSet<IInteractable> interactables = new HashSet<IInteractable>();
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
			interactables.Add(interactable);
		}

		public void Unregister(IInteractable interactable)
		{
			interactables.Remove(interactable);
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
