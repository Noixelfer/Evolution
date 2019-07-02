using Evolution.Character;
using Evolution.Items;
using System;

namespace Evolution.Actions
{
	public class HarvestNaturalResource : BaseAction
	{
		public override string ID => "";
		public bool ResourceAvailable = true;
		private Agent agent;
		private float harvestTime;
		private int quantity;
		private Func<IItem> actionResult;

		public HarvestNaturalResource(Agent agent, string actionDescription, int quantity, float harvestTime, Func<IItem> GetActionResultingItem, Func<bool> IsResourceAvailab)
		{
			this.agent = agent;
			Description = actionDescription;
			this.harvestTime = harvestTime;
			this.quantity = quantity;
			actionResult = GetActionResultingItem;
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
				var resultingItem = actionResult();
				if (resultingItem != null)
				{
					agent.Inventory.AddItem(resultingItem.ID, resultingItem, quantity);
					return ActionStatus.SUCCESSFULLY_EXECUTED;
				}

			}
			if (!ResourceAvailable)
				return ActionStatus.FAILED;
			return ActionStatus.IN_PROGRESS;
		}

		public override float GetScoreBasedOnTraits()
		{
			var activeTrait = agent.CharacterTraits[Traits.ACTIVE_TRAIT];
			return activeTrait.Percentage * 0.5f;
		}
	}
}