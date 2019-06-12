using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class Breed : BaseAction
	{
		public override string ID => "Breed";
		private Agent agent;

		public Breed(Agent agent)
		{
			this.agent = agent;
		}

		public override float GetScoreBasedOnTraits()
		{
			var breedScore = GetBreedScore();
			var lustScore = Mathf.Pow(agent.CharacterTraits[Traits.LUST_TRAIT].Percentage, 2.25f);
			if (breedScore != 0)
			{
				breedScore = Mathf.Clamp(breedScore * 0.7f + 0.3f * lustScore, 0, 1);
			}
			return breedScore;
		}

		private float GetBreedScore()
		{
			var currentAgeInYears = agent.StatsManager.Age.GetAge().Years;
			if (currentAgeInYears <= Constants.MINIMUM_BREED_AGE || currentAgeInYears >= Constants.MAXIMUM_BREED_AGE)
				return 0;
			var x = Mathf.Clamp(currentAgeInYears / (Constants.MAXIMUM_BREED_AGE - Constants.MINIMUM_BREED_AGE), 0, 1);
			if (x <= 0.1f)
				return Mathf.Clamp(0.5f + Mathf.Pow(x, 0.3f), 0, 1);
			else
				return Mathf.Clamp(-Mathf.Pow(x, 2) + 1.1f, 0, 1);
		}
	}
}
