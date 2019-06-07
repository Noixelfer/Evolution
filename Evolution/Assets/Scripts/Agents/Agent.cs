using Evolution.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public class Agent : BaseInteractable
	{
		public int AGENT_ID = -1;
		public override string ID => "Agent";
		public string Name { get; set; } = "Default";
		public int Age { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public List<IAction> KnownActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public Transform Transform => transform;
		public IBrain Brain => brain;


		public List<Trait> CharacterTraits = new List<Trait>();
		public StatsManager StatsManager;
		public RelationController RelationController;

		private Brain brain;

		private void Start()
		{
			Game.Instance.AgentsManager?.Register(this);
			brain = new Brain(this);
			CharacterTraits = Traits.GetRandomTraits();
			StatsManager = new StatsManager(this);
			RelationController = new RelationController(this);
			//moveTask.Execute();
		}

		private void Update()
		{
			brain.Update(Time.deltaTime);
			StatsManager.Update(Time.deltaTime);
		}

		public void Die(string causeOfDeath)
		{
			//TODO : Actual death logic
			Debug.Log("Agent " + Name + " died at the age " + StatsManager.Age.GetAge().ToString() + ". Cause of death: " + causeOfDeath);
			gameObject.SetActive(false);
		}

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var possibleActions = new List<IAction>();
			possibleActions.Add(new Talk(agent, this));
			return possibleActions;
		}
	}
}
