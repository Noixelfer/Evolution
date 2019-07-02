using Evolution.Character;

namespace Evolution.Actions
{
	public class FishingAction : BaseAction
	{
		public override string ID => "Fishing";
		private Agent agent;
		private float duration;

		public FishingAction(Agent agent, float duration)
		{
			this.agent = agent;
			this.duration = duration;
		}
	}
}
