namespace Evolution.Character
{
	public class StatsManager
	{
		public HungerStat Hunger;
		public StaminaStat Stamina;
		public EnergyStat Energy;
		public AgeStat Age;
		private Agent Owner;

		public StatsManager(Agent owner)
		{
			Owner = owner;
			Hunger = new HungerStat(owner, 200);
			Stamina = new StaminaStat(owner, 140);
			Energy = new EnergyStat(owner, 300);
			Age = new AgeStat(owner);
		}

		public void Update(float deltaTime)
		{
			Hunger.UpdateStat(deltaTime);
			Stamina.UpdateStat(deltaTime);
			Energy.UpdateStat(deltaTime);
			Age.UpdateStat(deltaTime);
		}
	}
}
