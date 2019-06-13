﻿using Evolution.Actions;
using Evolution.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution.Character
{
	[System.Serializable]
	public class EditorFTW
	{
		public string value1;
		public float value2;
	}

	public class Agent : BaseInteractable, ISocialInteraction
	{
		public int AGENT_ID = -1;
		public override string ID => "Agent";
		public string Name
		{
			get
			{
				return name;
			}
		}
		public Transform Transform => transform;
		public IBrain Brain => brain;
		public Inventory Inventory;
		public List<EditorFTW> traitsAsList = new List<EditorFTW>();
		public Dictionary<string, Trait> CharacterTraits { get; protected set; } = new Dictionary<string, Trait>();
		public StatsManager StatsManager;
		public RelationController RelationController;
		public UIAgentStatus UIAgentStatus;

		private Brain brain;
		private string name => "Agent " + AGENT_ID;

		public override void Awake()
		{
			base.Awake();
			Game.Instance.AgentsManager?.Register(this);
			brain = new Brain(this);
			CharacterTraits = Traits.GetRandomTraits();
			StatsManager = new StatsManager(this);
			RelationController = new RelationController(this, brain);
			UIAgentStatus = GetComponentInChildren<UIAgentStatus>();

			if (Game.Instance.SelectionManager.SelectedAgent == null)
				Game.Instance.SelectionManager.SelectNextAgent();
			//moveTask.Execute();
		}

		private void Update()
		{
			brain.Update(Time.deltaTime * Constants.REAL_TIME_MULTIPLIER);
			StatsManager.Update(Time.deltaTime * Constants.REAL_TIME_MULTIPLIER);
			traitsAsList = CharacterTraits.Select(keyPairValue => new EditorFTW { value1 = keyPairValue.Key, value2 = keyPairValue.Value.Percentage}).ToList<EditorFTW>();
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

		public void StopWaiting(Agent agent)
		{
			brain.StopWaiting(agent);
		}

		public void RequesterArrived(Agent requester, IAction socialAction)
		{
			brain.RequesterArrived(requester, socialAction);
		}
	}
}
