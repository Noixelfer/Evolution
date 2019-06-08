using Evolution.Actions;
using Evolution.Character;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public class RockResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var harvestAction = new HarvestNaturalResource(agent, "Gathering stone", 8f);
			return new List<IAction>() { harvestAction };
		}
	}
}
