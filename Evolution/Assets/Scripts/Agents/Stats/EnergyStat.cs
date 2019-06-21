using UnityEngine;

namespace Evolution.Character
{
	public class EnergyStat : BaseStat<float>
	{
		public EnergyStat(Agent owner, float maxValue, float minValue = 0) : base(owner)
		{
			MinValue = minValue;
			MaxValue = maxValue;
			Value = MaxValue;
			percentage = 1;
		}

		private float currentEnergy = 0;
		private float percentage;
		private readonly float regenerationRate = -1 / (Constants.SECONDS_IN_A_DAY * 4);

		public override string Name { get; } = "Energy";
		public override float Value
		{
			get
			{
				return currentEnergy;
			}
			protected set
			{
				if (currentEnergy != value)
				{
					currentEnergy = Mathf.Clamp(value, MinValue, MaxValue);
					percentage = currentEnergy / MaxValue;
				}
			}
		}

		public override void ModifyValue(float amount)
		{
			Value += amount;
		}

		public override float Percentage => percentage;

		public override void UpdateStat(float deltaTime)
		{
			Value += regenerationRate * deltaTime;
		}
	}
}
