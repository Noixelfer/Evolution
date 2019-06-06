namespace Evolution.Character
{
	[System.Serializable]
	public class Trait
	{
		public Trait(string name, float percentage, string oppositeTrait)
		{
			Name = name;
			OppositeTrait = oppositeTrait;
			Percentage = percentage;
		}

		public Trait(string name, string oppositeTrait)
		{
			Name = name;
			OppositeTrait = oppositeTrait;
		}

		public string Name;
		public float Percentage = 1;
		public string OppositeTrait;
	}
}
