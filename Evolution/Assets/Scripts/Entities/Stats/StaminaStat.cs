using UnityEngine;

namespace Evolution.Character
{
	public class StaminaStat : BaseStat<float>
	{
		public StaminaStat(Agent owner, float maxValue, float minValue = 0) : base(owner)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		private float currentStamina = 0;
		private readonly float regenerationRate = 0.2f;

		public override string Name { get; } = "Stamina";
		public override float Value
		{
			get
			{
				return currentStamina;
			}
			protected set
			{
				if (currentStamina != value)
					currentStamina = value;
			}
		}

		public override void UpdateStat(float deltaTime)
		{
			currentStamina += regenerationRate * deltaTime;
			Mathf.Clamp(currentStamina, MinValue, MaxValue);
		}
	}
}
