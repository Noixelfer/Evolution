using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class AnalyzeInteractable : BaseAction
	{
		public override string ID => "action.analyzeInteractable";
		private IInteractable interactable;
		private IAgent agent;

		public AnalyzeInteractable(IAgent agent, IInteractable interactable)
		{
			this.agent = agent;
			this.interactable = interactable;
		}

		public override void OnStart()
		{
			base.OnStart();
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (interactable == null || agent == null)
			{
				Debug.Log("Interactable or agent was null in AnalyzeInteractable action");
				return ActionStatus.FAILED;
			}
			agent.Brain.AddKnownInteractable(interactable.ID);
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}
	}
}
