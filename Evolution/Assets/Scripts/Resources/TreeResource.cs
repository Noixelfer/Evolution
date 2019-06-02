using Evolution.Actions;
using Evolution.Character;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public class TreeResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;

		public override List<IAction> GetPossibleActions(IAgent agent)
		{
			var harvestAction = new HarvestNaturalResource(agent, "Collectiong apples", 0.2f);
			return new List<IAction>() { harvestAction };
		}
	}
}
