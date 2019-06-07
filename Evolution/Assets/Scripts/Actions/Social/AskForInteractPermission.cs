using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class AskForInteractPermission : BaseAction
	{
		public override string ID => "Interact Permission";
		private bool canInterract = false;
		private readonly Agent requester;
		private readonly Agent receiver;

		public AskForInteractPermission(Agent requester, Agent receiver)
		{
			this.requester = requester;
			this.receiver = receiver;
		}

		public override ActionStatus OnUpdate(float time)
		{
			canInterract = receiver.RelationController.RespondToSocialRequest(requester);
			if (canInterract)
			{
				Debug.Log("talk permission was accpted");
				requester.Brain.CurrentSocialInteraction = new SocialInteraction(requester, receiver);
				return ActionStatus.SUCCESSFULLY_EXECUTED;
			}
			else
			{
				Debug.Log("talk permission was denied");
				return ActionStatus.FAILED;
			}
		}
	}
}
