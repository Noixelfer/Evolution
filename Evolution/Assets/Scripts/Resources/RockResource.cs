using Evolution.Actions;
using Evolution.Character;
using Evolution.Items;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public class RockResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var stone = new ResourceItem(new ItemDefinition(0.7f, 0.5f), ItemsUtils.STONE_ID);
			stone.ItemDefinition.Name = "stone";
			var harvestAction = new HarvestNaturalResource(agent, "Mining stone", stone, 2, Constants.MINE_TIME);
			return new List<IAction>() { harvestAction };
		}
	}
}
