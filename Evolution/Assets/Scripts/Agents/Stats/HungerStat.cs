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
			Value = maxValue;
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
					currentHunger = Mathf.Clamp(value, MinValue, MaxValue);
			}
		}
		public override void ModifyValue(float amount)
		{
			Value += amount;
		}

		public override void UpdateStat(float deltaTime)
		{
			Value -= Constants.HUNGER_DECREASE_RATE * deltaTime;
		}

		public override float Percentage => Value / (MaxValue - MinValue);
	}
}
