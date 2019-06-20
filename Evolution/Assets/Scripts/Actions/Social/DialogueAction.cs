using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class DialogueAction : BaseAction
	{
		public override string ID => "Dialogue action";

		private Agent agent1;
		private Agent agent2;
		private float duration;

		public DialogueAction(Agent agent1, Agent agent2)
		{
			this.agent1 = agent1;
			this.agent2 = agent2;
			duration = Random.Range(0.2f, 2f) * Constants.HOUR_IN_SECONDS;
		}
		public override void OnStart()
		{
			base.OnStart();
			Description = "Talking with " + agent1.AGENT_ID;
		}

		public override ActionStatus OnUpdate(float time)
		{
			duration -= time;
			if (duration > 0)
				return ActionStatus.IN_PROGRESS;
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}
	}
}
