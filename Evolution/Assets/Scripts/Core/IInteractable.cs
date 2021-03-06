﻿using Evolution.Actions;
using Evolution.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution
{
	/// <summary>
	/// Interface implemented by all the object which can interract with our Agent
	/// </summary>
	public interface IInteractable
	{
		string ID { get; }
		GameObject gameObject { get; }
		List<IAction> GetPossibleActions(Agent agent);
		HashSet<(int, int)> FreeNeighbourCells { get; }
		bool Reachable { get; set; }
	}
}
