using Evolution.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Items
{
	public class Inventory
	{
		private Dictionary<string, (IItem, int)> items = new Dictionary<string, (IItem, int)>();
		private Agent owner;
		public Inventory(Agent owner)
		{
			this.owner = owner;
		}

		public bool HasItem(string id)
		{
			return items.ContainsKey(id);
		}

		/// <summary>
		/// Function to add a given amount of an item in inventory
		/// </summary>
		/// <param name="item"></param>
		/// <param name="quantity"></param>
		public void AddItem(string id, IItem item, int quantity)
		{
			var type = item.GetType();
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
			if (HasItem(id))
				items[id] = (items[id].Item1, items[id].Item2 + quantity);
			else
				items.Add(id, (item, quantity));
		}

		/// <summary>
		/// Returns all the items of type T from inventory and
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Dictionary<T, int> GetItemsOfType<T>() where T : IItem
		{
			var result = new Dictionary<T, int>();
			foreach (var value in items.Values)
				if (value is T)
					result.Add((T)value.Item1, value.Item2);
			return result;
		}

		public (IItem, int) GetItem(string id)
		{
			if (items.ContainsKey(id))
				return items[id];
			return (null, -1);
		}

		public void RemoveItem(IItem item)
		{
		}

		public void RemoveItemQuantity(IItem item, int quantity)
		{
			if (!items.ContainsKey(item.ID))
			{
				Debug.LogError("Tried to remove null item!");
				return;
			}

			if (items[item.ID].Item2 < quantity)
			{
				Debug.LogError("Tried to remove more items than it has!");
			}

			if (items[item.ID].Item2 == quantity)
				items.Remove(item.ID);
			else
				items[item.ID] = (items[item.ID].Item1, items[item.ID].Item2 - quantity);
		}
	}
}
