using UnityEngine;

namespace Evolution.Character
{
	public class AgeStat : BaseStat<float>
	{
		public override string Name => "Age";
		private float realTimeMultiplier;
		private float currentAgeInSeconds = 0;
		private Age Age;

		public AgeStat(Agent owner, float realTimeMultiplier) : base(owner)
		{
			this.realTimeMultiplier = realTimeMultiplier;
			Age = new Age();
		}


		public override float Value
		{
			get
			{
				return 0;
			}
			protected set
			{
			}
		}

		public override void UpdateStat(float deltaTime)
		{
			currentAgeInSeconds += deltaTime * realTimeMultiplier;
			Age.GetOlder(deltaTime * realTimeMultiplier);
			if (DiedOfOldAge(deltaTime))
				Owner.Die("died of old age!");
		}

		public Age GetAge()
		{
			return Age;
		}

		/// <summary>
		/// The agent has a random chance to die which increases as he gets older
		/// The chance is caped at 95 %
		/// </summary>
		private bool DiedOfOldAge(float deltaTime)
		{
			var x = Mathf.Min((Age.Months + Age.Years * 12) / 960, 1);
			float chanceToDie = Mathf.Min(Mathf.Pow(x, 14), 0.95f) * deltaTime;
			return (Random.Range(0f, 1f) < chanceToDie);
		}
	}

	public class Age
	{
		private readonly ulong SECONDS_IN_A_YEAR = 31556926;
		private readonly ulong SECONDS_IN_A_MONTH = 2629744;
		private readonly ulong SECONDS_IN_A_DAY = 86400;
		private ulong secondsAlive;

		public Age(ulong secondsAlive = 0)
		{
			this.secondsAlive = secondsAlive;
			UpdateAge(secondsAlive);
		}

		public void GetOlder(float seconds)
		{
			secondsAlive += (ulong)seconds;
			UpdateAge(secondsAlive);
		}

		private void UpdateAge(ulong seconds)
		{
			Years = seconds / SECONDS_IN_A_YEAR;
			Months = (seconds - Years * SECONDS_IN_A_YEAR) / SECONDS_IN_A_MONTH;
			Days = (seconds - Years * SECONDS_IN_A_YEAR - Months * SECONDS_IN_A_MONTH) / SECONDS_IN_A_DAY;
		}

		public ulong Years = 0;
		public ulong Months = 0;
		public ulong Days = 0;

		public override string ToString()
		{
			var ageAsString = "";
			if (Years != 0)
				ageAsString += Years.ToString() + " years ";
			if (Months != 0)
				ageAsString += Months.ToString() + " months ";
			if (Days != 0)
			{
				if (ageAsString.Equals(""))
					ageAsString = Days.ToString() + " days";
				else
					ageAsString += "and " + Days.ToString() + " days";
			}
			if (ageAsString == "")
				return "new born";
			return ageAsString;
		}
	}
}
