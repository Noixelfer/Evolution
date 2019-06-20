using Evolution.Character;
using System.Collections.Generic;

namespace Evolution.Actions
{
	public class Sleep : BaseAction
	{
		public override string ID => "Sleep";
		public override string Description { get; protected set; } = "Sleeping";
		public override HashSet<string> Effects { get; set; } = new HashSet<string>()
		{
			ActionEffects.RESTORE_ENERGY
		};

		private Agent agent;
		private float time;
		public Sleep(Agent agent, float time)
		{
			this.agent = agent;
			this.time = time;
		}

		public override ActionStatus OnUpdate(float deltaTime)
		{
			time -= deltaTime;
			agent.StatsManager.Energy.ModifyValue(agent.StatsManager.Energy.MaxValue / (7 * Constants.HOUR_IN_SECONDS) * deltaTime);
			if (time <= 0 || agent.StatsManager.Energy.Percentage >= 0.95f)
				return ActionStatus.SUCCESSFULLY_EXECUTED;
			return base.OnUpdate(time);
		}

		//Returns a base score for sleep
		public override float GetScoreBasedOnTraits()
		{
			return 0.05f;
		}
	}
}
