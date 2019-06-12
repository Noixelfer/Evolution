using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class AnalyzeInteractable : BaseAction
	{
		public override string ID => "action.analyzeInteractable";
		private IInteractable interactable;
		private Agent agent;

		public AnalyzeInteractable(Agent agent, IInteractable interactable)
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

		public override float GetScoreBasedOnTraits()
		{
			var curiousTrait = agent.CharacterTraits[Traits.CURIOUS_TRAIT];
			var carefullTrait = agent.CharacterTraits[Traits.CAREFUL_TRAIT];

			//More courious and carefree you are, higher the score will be for analyze action.
			return (curiousTrait.Percentage * 0.7f + (1 - carefullTrait.Percentage) * 0.3f) * 0.5f;
		}
	}
}
