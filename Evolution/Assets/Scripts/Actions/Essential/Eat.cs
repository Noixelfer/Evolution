using Evolution.Character;
using Evolution.Items;
using System.Collections.Generic;

namespace Evolution.Actions
{
	public class Eat : BaseAction
	{
		public override string ID => "Eat";
		private Agent Agent;
		private BaseEdibleItem item;
		public override HashSet<string> Effects { get; set; } = new HashSet<string>()
		{
			ActionEffects.RESTORE_HUNGER
		};

		private float actionDuration;

		public Eat(Agent agent, BaseEdibleItem item)
		{
			Agent = agent;
			this.item = item;
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (item == null)
				return ActionStatus.FAILED;

			Agent.Inventory.RemoveItemQuantity(item, 1);
			Agent.StatsManager.Hunger.ModifyValue(item.HungerRestored);
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}

		public override float GetScoreBasedOnTraits()
		{
			return 0.2f + Agent.CharacterTraits[Traits.GLUTTON_TRAIT].Percentage * 0.3f;
		}
	}
}
