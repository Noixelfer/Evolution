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
		private float duration;
		public override HashSet<string> Effects { get; set; } = new HashSet<string>()
		{
			ActionEffects.RESTORE_HUNGER
		};

		private float actionDuration;

		public Eat(Agent agent, BaseEdibleItem item, float duration)
		{
			Agent = agent;
			this.item = item;
			this.duration = duration;
		}

		public override void OnStart()
		{
			base.OnStart();
			Description = "Eating " + item?.ItemDefinition.Name;
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (item == null)
				return ActionStatus.FAILED;
			duration -= time;
			if (duration > 0)
				return ActionStatus.IN_PROGRESS;
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}

		public override void OnEnd()
		{
			Agent.Inventory.RemoveItemQuantity(item, 1);
			Agent.StatsManager.Hunger.ModifyValue(item.HungerRestored);
			base.OnEnd();
		}

		public override float GetScoreBasedOnTraits()
		{
			return 0.2f + Agent.CharacterTraits[Traits.GLUTTON_TRAIT].Percentage * 0.3f;
		}
	}
}
