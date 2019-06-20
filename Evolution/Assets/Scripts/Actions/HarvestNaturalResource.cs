using Evolution.Character;
using Evolution.Items;

namespace Evolution.Actions
{
	public class HarvestNaturalResource : BaseAction
	{
		public override string ID => "";
		private Agent agent;
		private float harvestTime;
		private IItem resultingItem;
		private int quantity;

		public HarvestNaturalResource(Agent agent, string actionDescription, IItem resultingItem, int quantity, float harvestTime)
		{
			this.agent = agent;
			Description = actionDescription;
			this.harvestTime = harvestTime;
			this.resultingItem = resultingItem;
			this.quantity = quantity;
		}

		public override void OnStart()
		{
			base.OnStart();
		}

		//TODO : make actual collect logic
		public override ActionStatus OnUpdate(float time)
		{
			harvestTime -= time;
			if (harvestTime <= 0)
			{
				agent.Inventory.AddItem(resultingItem, quantity);
				return ActionStatus.SUCCESSFULLY_EXECUTED;
			}
			return ActionStatus.IN_PROGRESS;
		}

		public override float GetScoreBasedOnTraits()
		{
			var activeTrait = agent.CharacterTraits[Traits.ACTIVE_TRAIT];
			return activeTrait.Percentage * 0.5f;
		}
	}
}