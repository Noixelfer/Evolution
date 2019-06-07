using UnityEngine;

namespace Evolution.Character
{
	public class HungerStat : BaseStat<float>
	{
		public HungerStat(Agent owner, float maxValue, float minValue = 0) : base(owner)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		private float currentHunger = 0;
		private readonly float regenerationRate = -0.15f;

		public override string Name { get; } = "Hunger";
		public override float Value
		{
			get
			{
				return currentHunger;
			}
			protected set
			{
				if (currentHunger != value)
					currentHunger = value;
			}
		}

		public override void UpdateStat(float deltaTime)
		{
			currentHunger += regenerationRate * deltaTime;
			Mathf.Clamp(currentHunger, MinValue, MaxValue);
		}
	}
}
