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
		public static readonly string GLUTTON_TRAIT = "Glutton";

		/// <summary>
		/// List with all available traits
		/// </summary>
		public static readonly Dictionary<string, Trait> AllTraits = new Dictionary<string, Trait>()
		{
			{SOCIALBLE_TRAIT, new Trait(SOCIALBLE_TRAIT, "Atisocial") },
			{ACTIVE_TRAIT, new Trait(ACTIVE_TRAIT, "Slauch") },
			{AGRESSIVE_TRAIT, new Trait(AGRESSIVE_TRAIT, "Passive")},
			{CAREFUL_TRAIT, new Trait(CAREFUL_TRAIT, "Gambler")},
			{LUST_TRAIT, new Trait(LUST_TRAIT, "Chastity")},
			{GREEDY_TRADE, new Trait(GREEDY_TRADE, "Generous")},
			{FAST_LEARNER_TRAIT, new Trait(FAST_LEARNER_TRAIT, "Slow Learner")},
			{CURIOUS_TRAIT, new Trait(CURIOUS_TRAIT, "Incurious")},
			{GLUTTON_TRAIT, new Trait(GLUTTON_TRAIT, "Nibbler")}
		};

		public static Dictionary<string, Trait> GetRandomTraits()
		{
			var traits = new Dictionary<string, Trait>();
			foreach (var trait in AllTraits)
			{
				var newTrait = new Trait(trait.Value.Name, trait.Value.OppositeTrait);
				newTrait.Percentage = Random.Range(0f, 1f);
				traits.Add(newTrait.Name, newTrait);
			}
			return traits;
		}

		public static Dictionary<string, Trait> GetTraitsFromParents(Agent agent1, Agent agent2, float mutationChace = 0.1f)
		{
			var traits = new Dictionary<string, Trait>();
			foreach (var trait in AllTraits)
			{
				var newTrait = new Trait(trait.Value.Name, trait.Value.OppositeTrait);
				if (Random.Range(0f, 1f) > mutationChace)
					newTrait.Percentage = (agent1.CharacterTraits[trait.Key].Percentage + agent2.CharacterTraits[trait.Key].Percentage) / 2;
				else
					newTrait.Percentage = Random.Range(0f, 1f);
				traits.Add(newTrait.Name, newTrait);
			}
			return traits;
		}
	}
}
