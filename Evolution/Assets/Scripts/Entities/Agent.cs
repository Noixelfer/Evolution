using Evolution.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public class Agent : MonoBehaviour, IAgent
	{
		public string Name { get; set; } = "Default";
		public int Age { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public List<IAction> KnownActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public Transform Transform => transform;
		public IBrain Brain => brain;
		public List<Trait> CharacterTraits = new List<Trait>();
		public StatsManager StatsManager;
		private Brain brain;

		private void Start()
		{
			brain = new Brain(this);
			CharacterTraits = Traits.GetRandomTraits();
			StatsManager = new StatsManager(this);
			//moveTask.Execute();
		}

		private void Update()
		{
			brain.Update(Time.deltaTime);
			StatsManager.Update(Time.deltaTime);

			var x = Random.Range(0, 500);
			if (x == 1)
				Debug.Log("The age of a random agent is : " + StatsManager.Age.GetAge().ToString());
		}

		public void Die(string causeOfDeath)
		{
			//TODO : Actual death logic
			Debug.Log("Agent " + Name + " died at the age " + StatsManager.Age.GetAge().ToString() + ". Cause of death: " + causeOfDeath);
			gameObject.SetActive(false);
		}
	}
}
