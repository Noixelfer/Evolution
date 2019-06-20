using Evolution.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Items
{
	public class Inventory
	{
		private Dictionary<IItem, int> items = new Dictionary<IItem, int>();
		private Agent owner;
		private Dictionary<BaseEdibleItem, int> edibleItems = new Dictionary<BaseEdibleItem, int>();
		public Inventory(Agent owner)
		{
			this.owner = owner;
		}

		public bool HasItem(IItem item)
		{
			return items.ContainsKey(item);
		}

		/// <summary>
		/// Function to add a given amount of an item in inventory
		/// </summary>
		/// <param name="item"></param>
		/// <param name="quantity"></param>
		public void AddItem(IItem item, int quantity)
		{
			if (item == null)
			{
				Debug.LogError("Tried to add null item in inventory!");
				return;
			}

			if (quantity <= 0)
			{
				Debug.LogError("Tried to add 0 or negative amount of items of type " + item + " in inventory!!!");
				return;
			}

			if (item is BaseEdibleItem)
			{
				var edibletem = (BaseEdibleItem)item;
				if (edibleItems.ContainsKey(edibletem))
					edibleItems[edibletem] += quantity;
				else
					edibleItems.Add(edibletem, quantity);
			}
			else
			{
				if (HasItem(item))
					items[item] += quantity;
				else
					items.Add(item, quantity);
			}
		}

		/// <summary>
		/// Returns all the items of type T from inventory and
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Dictionary<T, int> GetItemsOfType<T>() where T : IItem
		{
			var result = new Dictionary<T, int>();
			foreach (var key in items.Keys)
				if (key is T)
					result.Add((T)key, items[key]);
			return result;
		}

		public Dictionary<BaseEdibleItem, int> GetEdibleItems()
		{
			return edibleItems;
		}

		public void RemoveItem(IItem item)
		{
		}

		public void RemoveItemQuantity(IItem item, int quantity)
		{
			if (!items.ContainsKey(item))
			{
				Debug.LogError("Tried to remove null item!");
				return;
			}

			if (items[item] < quantity)
			{
				Debug.LogError("Tried to remove more items than it has!");
			}

			if (items[item] == quantity)
				items.Remove(item);
			else
				items[item] -= quantity;
		}
	}
}
