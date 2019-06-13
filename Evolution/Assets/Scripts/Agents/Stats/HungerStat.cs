using UnityEngine;

namespace Evolution.Character
{
	public class HungerStat : BaseStat<float>
	{
		public override string Name { get; } = "Hunger";

		public HungerStat(Agent owner, float maxValue, float minValue = 0) : base(owner)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		private float currentHunger = 0;
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
			currentHunger -= Constants.HUNGER_DECREASE_RATE * deltaTime;
			Mathf.Clamp(currentHunger, MinValue, MaxValue);
		}

		public override float Percentage => Value / (MaxValue - MinValue);
	}
}
