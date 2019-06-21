using Evolution.Actions;
using Evolution.Character;
using Evolution.Items;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public class TreeResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var apple = new BaseEdibleItem(new ItemDefinition(0.2f, 0.1f), 25, ItemsUtils.APPLE_ID);
			apple.ItemDefinition.Name = "apple";
			var harvestAction = new HarvestNaturalResource(agent, "Collecting apples", apple, 4, Constants.COLLECT_APPLE_TIME);
			harvestAction.Effects = new HashSet<string>() { ActionEffects.OBTAINS_FOOD };
			return new List<IAction>() { harvestAction };
		}
	}
}
