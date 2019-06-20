﻿using Evolution.Character;
using TMPro;
using UnityEngine;

namespace Evolution.UI
{
	public class UIAgentViewer : MonoBehaviour
	{
		public TextMeshProUGUI Name;
		public TextMeshProUGUI Age;
		public GameObject Traits;

		public void ShowAgent(Agent agent)
		{
			if (agent == null)
			{
				Debug.LogError("The agent was null!");
				return;
			}

			Name.text = agent.Name;
			Age.text = agent.StatsManager.Age.GetAge().Years.ToString() + " years old";

			DisplayTraits(agent);
		}

		private void DisplayTraits(Agent agent)
		{
			foreach (Transform go in Traits.transform)
			{
				if (go.GetComponent<UITrait>() != null)
					Destroy(go.gameObject);
			}

			foreach (var trait in agent.CharacterTraits)
			{
				var uITrait = Instantiate(Game.Instance.PrefabsManager.GetPrefab<UITrait>("UITrait"), Traits.transform);
				uITrait.SetName(trait.Value.Name);
				uITrait.SetPercentage(trait.Value.Percentage);
			}
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
	}
}