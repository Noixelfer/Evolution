using Evolution.Character;

namespace Evolution.Actions
{
	public class DialogueAction : BaseAction
	{
		public override string ID => "Dialogue action";

		private Agent agent1;
		private Agent agent2;
		private Agent talker;

		public DialogueAction(Agent agent1, Agent agent2)
		{
			this.agent1 = agent1;
			this.agent2 = agent2;
		}

		public override ActionStatus OnUpdate(float time)
		{
			//TODO : Actual dialogue implementation
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}
	}
}
