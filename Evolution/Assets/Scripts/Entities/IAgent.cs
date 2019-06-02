using Evolution.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public interface IAgent
	{
		IBrain Brain { get; }
		string Name { get; set; }
		int Age { get; set; }
		List<IAction> KnownActions { get; set; }
		Transform Transform { get; }
	}
}
