using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class HarvestNaturalResource : BaseAction
	{
		public override string ID => "";
		private IAgent agent;
		private string description = "";
		private float harvestTime;

		public HarvestNaturalResource(IAgent agent, string actionDescription, float harvestTime)
		{
			this.agent = agent;
			description = actionDescription;
			this.harvestTime = harvestTime;
		}

		public override void OnStart()
		{
			base.OnStart();
			//Debug.Log(description);
		}

		//TODO : make actual collect logic
		public override ActionStatus OnUpdate(float time)
		{
			harvestTime -= time;
			if (harvestTime <= 0)
				return ActionStatus.SUCCESSFULLY_EXECUTED;
			return ActionStatus.IN_PROGRESS;
		}
	}
}