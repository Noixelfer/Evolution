using Evolution.Character;

namespace Evolution.Actions
{
	public class Breed : BaseAction
	{
		public override string ID => "Breed";
		private Agent agent;

		public Breed(Agent agent)
		{
			this.agent = agent;
		}


	}
}
