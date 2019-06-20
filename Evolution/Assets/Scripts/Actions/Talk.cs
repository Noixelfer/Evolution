using Evolution.Character;

namespace Evolution.Actions
{
	public class Talk : BaseAction
	{
		public override string ID => "Talk";
		public override string Description
		{
			get
			{
				return description;
			}

			protected set
			{
			}
		}
		private Agent talker;
		private Agent receiver;
		private DialogueAction dialogueAction;
		private string description = "";

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
			description = "Talking with " + receiver.AGENT_ID;
		}

		public override ActionStatus OnUpdate(float time)
		{
			return dialogueAction.OnUpdate(time);
		}

		public override float GetScoreBasedOnTraits()
		{
			var sociableTrait = talker.CharacterTraits[Traits.SOCIALBLE_TRAIT];
			return 0.1f + sociableTrait.Percentage * 0.6f;
		}
	}
}
