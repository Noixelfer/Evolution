namespace Evolution.Character
{
	public class Needs
	{
		private Brain Brain;
		private Agent Agent;

		public Needs(Brain brain, Agent agent)
		{
			Brain = brain;
			Agent = agent;
		}
		public float GetSleepNeed()
		{
			var x = 1 - Agent.StatsManager.Energy.Percentage;
			return x * x * x * x * x;
		}

		public float GetHungerNeed()
		{
			var x = 1 - Agent.StatsManager.Hunger.Percentage;
			return x * x * x;
		}
	}
}
