using UnityEngine;

namespace Evolution.Character
{
	public class EnergyStat : BaseStat<float>
	{
		public EnergyStat(Agent owner, float maxValue, float minValue = 0) : base(owner)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		private float currentEnergy = 0;
		private readonly float regenerationRate = 0.2f;

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
					currentEnergy = value;
			}
		}

		public override float Percentage => Value / (MaxValue - MinValue);

		public override void UpdateStat(float deltaTime)
		{
			currentEnergy += regenerationRate * deltaTime;
			Mathf.Clamp(currentEnergy, MinValue, MaxValue);
		}
	}
}
