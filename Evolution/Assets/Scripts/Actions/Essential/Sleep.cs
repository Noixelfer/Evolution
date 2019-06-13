using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class Sleep : BaseAction
	{
		public override string ID => "Sleep";
		private Agent agent;

		public Sleep(Agent agent)
		{
			this.agent = agent;
		}

		/// <summary>
		/// f(x) = [(x^3 + x^(1/3)) + 0.6x]/2
		/// </summary>
		/// <returns></returns>
		public override float GetScoreBasedOnTraits()
		{
			var x = agent.StatsManager.Energy.Percentage;
			return (Mathf.Pow(x, 3) + Mathf.Pow(x, 0.33f) + 0.6f * x) / 2;
		}
	}
}
