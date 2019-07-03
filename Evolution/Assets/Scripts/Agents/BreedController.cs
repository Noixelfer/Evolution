using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution.Character
{
	public class BreedController
	{
		private Agent owner;
		private int LastBreedAgeInYears = 0;
		private int numberOfChilds = 0;

		public BreedController(Agent owner)
		{
			this.owner = owner;
		}

		public void OnBreed()
		{
			numberOfChilds++;
			LastBreedAgeInYears = (int)owner.StatsManager.Age.GetAge().Years;
		}

		public float Compability(Agent other)
		{
			var ownerTraits = owner.CharacterTraits;
			var otherTraits = other.CharacterTraits;

			var absoluteDifference = new List<float>();
			foreach (var traitKey in Traits.AllTraits.Keys)
				absoluteDifference.Add(Mathf.Abs(ownerTraits[traitKey].Percentage - otherTraits[traitKey].Percentage));
			absoluteDifference.Sort();
			absoluteDifference.RemoveRange(4, absoluteDifference.Count - 5);
			var x = absoluteDifference.Sum();
			var result = Mathf.Clamp(1 - (Mathf.Pow(0.25f * x, 0.25f) + 0.25f * x) / 2f, 0, 1);
			return result;
		}

		public bool CanBreed()
		{
			if (numberOfChilds >= Constants.MAXIMUM_NUMBER_OF_CHILDS)
				return false;
			var ageInYears = (int)owner.StatsManager.Age.GetAge().Years;
			if (ageInYears - LastBreedAgeInYears < Constants.PAUSE_YEARS_BETWEEN_BREEDS)
				return false;
			if (ageInYears < Constants.MINIMUM_BREED_AGE || ageInYears > Constants.MAXIMUM_BREED_AGE)
				return false;
			return true;
		}
	}
}
