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
			percentage = 1;
		}

		private float currentHunger = 0;
		private float percentage;

		public override float Value
		{
			get
			{
				return currentHunger;
			}
			protected set
			{
				if (currentHunger != value)
				{
					currentHunger = Mathf.Clamp(value, MinValue, MaxValue);
					percentage = currentHunger / MaxValue;
				}
			}
		}
		public override void ModifyValue(float amount)
		{
			Value += amount;
		}

		public override void UpdateStat(float deltaTime)
		{
			Value -= Constants.HUNGER_DECREASE_RATE * deltaTime;
			if (Value <= 0)
				Owner.Die("starved");
		}

		public override float Percentage => percentage;
	}
}
