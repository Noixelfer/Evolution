using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public static class Traits
	{
		public static readonly string SOCIALBLE_TRAIT = "Sociable";
		public static readonly string ACTIVE_TRAIT = "Active";
		public static readonly string AGRESSIVE_TRAIT = "Agressive";
		public static readonly string CAREFUL_TRAIT = "Careful";
		public static readonly string LUST_TRAIT = "Lust";
		public static readonly string GREEDY_TRADE = "Greedy";
		public static readonly string FAST_LEARNER_TRAIT = "Fast Learner";
		public static readonly string CURIOUS_TRAIT = "Curious";

		/// <summary>
		/// List with all available traits
		/// </summary>
		public static readonly List<Trait> AllTraits = new List<Trait>()
		{
			new Trait(SOCIALBLE_TRAIT, "Atisocial"),
			new Trait(ACTIVE_TRAIT, "Slauch"),
			new Trait(AGRESSIVE_TRAIT, "Passive"),
			new Trait(CAREFUL_TRAIT, "Gambler"),
			new Trait(LUST_TRAIT, "Chastity"),
			new Trait(GREEDY_TRADE, "Generous"),
			new Trait(FAST_LEARNER_TRAIT, "Slow Learner"),
			new Trait(CURIOUS_TRAIT, "Incurious")
		};

		public static List<Trait> GetRandomTraits()
		{
			var traits = new List<Trait>();
			foreach (var trait in AllTraits)
			{
				trait.Percentage = Random.Range(0f, 1f);
				traits.Add(trait);
			}

			return traits;
		}
	}
}
