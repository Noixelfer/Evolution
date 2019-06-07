using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class Talk : BaseAction
	{
		public override string ID => "Talk";
		private Agent talker;
		private Agent receiver;
		private bool canTalkToOther = false;

		public Talk(Agent talker, Agent receiver)
		{
			this.talker = talker;
			this.receiver = receiver;
		}

		public override void OnStart()
		{
			base.OnStart();
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (talker.Brain.CurrentSocialInteraction != null)
			{
				Debug.Log("We did it, booois");
			}
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}
	}
}
