using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution
{
	public class InteractablesManager : MonoBehaviour
	{
		private HashSet<IInteractable> interactables = new HashSet<IInteractable>();

		public void Register(IInteractable interactable)
		{
			interactables.Add(interactable);
		}

		public void Unregister(IInteractable interactable)
		{
			interactables.Remove(interactable);
		}
		
		/// <summary>
		/// Returns a HashSet with the IInteractables in the given radius from center
		/// </summary>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public HashSet<IInteractable> GetInteractablesAround(Vector3 position, float radius)
		{
			var squaredRadius = radius * radius;
			var result = new HashSet<IInteractable>();
			foreach (var interactable in interactables.ToList())
			{
				var squaredDistance = (position - interactable.gameObject.transform.position).sqrMagnitude;
				if (squaredDistance <= squaredRadius)
				{
					result.Add(interactable);
				}
			}
			return result;
		}
	}
}
