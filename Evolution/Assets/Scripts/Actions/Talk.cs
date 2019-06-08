using Evolution.Character;

namespace Evolution.Actions
{
	public class Talk : BaseAction
	{
		public override string ID => "Talk";
		private Agent talker;
		private Agent receiver;
		private bool canTalkToOther = false;
		private DialogueAction dialogueAction;

		public Talk(Agent talker, Agent receiver)
		{
			this.talker = talker;
			this.receiver = receiver;
			dialogueAction = new DialogueAction(talker, receiver);
		}

		public override void OnStart()
		{
			base.OnStart();
			receiver.RequesterArrived(talker, dialogueAction);
			dialogueAction?.Initialize();
		}

		public override ActionStatus OnUpdate(float time)
		{
			return dialogueAction.OnUpdate(time);
		}
	}
}
