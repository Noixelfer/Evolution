using UnityEngine;

namespace Evolution.Character
{
	public class AgeStat : BaseStat<float>
	{
		public override string Name => "Age";
		private float currentAgeInSeconds = 0;
		private Age Age;

		public AgeStat(Agent owner) : base(owner)
		{
			Age = new Age();
		}

		public void SetAge(int years, int months, int days)
		{
			Age.Years = years;
			Age.Months = months;
			Age.Days = days;
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
			currentAgeInSeconds += deltaTime;
			Age.GetOlder(deltaTime);
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
			float chanceToDie = Mathf.Min(Mathf.Pow(x, 12), 0.75f) * (deltaTime / Constants.REAL_TIME_MULTIPLIER);
			return (Random.Range(0f, 1f) < chanceToDie);
		}
	}

	public class Age
	{
		private int secondsAlive;

		public Age(int secondsAlive = 0)
		{
			this.secondsAlive = secondsAlive;
			UpdateAge((int)secondsAlive);
		}

		public void GetOlder(float seconds)
		{
			UpdateAge((int)seconds);
		}

		private void UpdateAge(int passedSeconds)
		{
			if (passedSeconds == 0)
				return;
			Seconds += passedSeconds;
			if (Seconds >= Constants.SECONDS_IN_A_DAY)
			{
				Days += Seconds / Constants.SECONDS_IN_A_DAY;
				Seconds = Seconds % Constants.SECONDS_IN_A_DAY;
			}

			if (Days * Constants.SECONDS_IN_A_DAY >= Constants.SECONDS_IN_A_MONTH)
			{
				Months += (Days * Constants.SECONDS_IN_A_DAY) / Constants.SECONDS_IN_A_MONTH;
				Days = ((Days * Constants.SECONDS_IN_A_DAY) % Constants.SECONDS_IN_A_MONTH) / Constants.SECONDS_IN_A_DAY;
			}

			if (Months * Constants.SECONDS_IN_A_MONTH + Days * Constants.SECONDS_IN_A_DAY >= Years * Constants.SECONDS_IN_A_YEAR)
			{
				Years += (Months * Constants.SECONDS_IN_A_MONTH + Days * Constants.SECONDS_IN_A_DAY) / Constants.SECONDS_IN_A_YEAR;
				var remainder = (Months * Constants.SECONDS_IN_A_MONTH + Days * Constants.SECONDS_IN_A_DAY) % Constants.SECONDS_IN_A_YEAR;
				Months = remainder / Constants.SECONDS_IN_A_MONTH;
				if (Months != 0)
					Days = (remainder % Months * Constants.SECONDS_IN_A_MONTH) / Constants.SECONDS_IN_A_DAY;
				else
					Days = (remainder % Constants.SECONDS_IN_A_MONTH) / Constants.SECONDS_IN_A_DAY;
			}
		}

		private int Seconds = 0;
		public int Years = 0;
		public int Months = 0;
		public int Days = 0;

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
