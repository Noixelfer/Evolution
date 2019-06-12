using Evolution.Actions;
using Evolution.Character;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public class TreeResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var harvestAction = new HarvestNaturalResource(agent, "Collecting apples", Constants.COLLECT_APPLE_TIME);
			return new List<IAction>() { harvestAction };
		}
	}
}
