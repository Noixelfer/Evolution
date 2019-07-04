using Evolution.Actions;
using Evolution.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public class Agent : BaseInteractable, ISocialInteraction
	{
		public SpriteRenderer Face;
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
		public Dictionary<string, Trait> CharacterTraits { get; set; } = new Dictionary<string, Trait>();
		public StatsManager StatsManager;
		public RelationController RelationController;
		public UIAgentStatus UIAgentStatus;
		public BreedController BreedController;

		private Brain brain;
		private string name => "Agent " + AGENT_ID;
		private (int, int) lastInteractableKey;

		public override void Awake()
		{
			//base.Awake();
			Game.Instance.AgentsManager?.Register(this);
			brain = new Brain(this);
			this.CharacterTraits = Traits.GetRandomTraits();
			StatsManager = new StatsManager(this);
			RelationController = new RelationController(this, brain);
			BreedController = new BreedController(this);
			UIAgentStatus = GetComponentInChildren<UIAgentStatus>();
			Inventory = new Inventory(this);
			if (Game.Instance.SelectionManager.SelectedAgent == null)
				Game.Instance.SelectionManager.SelectNextAgent();
			//moveTask.Execute();
		}

		private void Start()
		{
			lastInteractableKey = GetInteractableKey();
			Game.Instance.InteractablesManager.Register(this);
		}

		private int xIndice;
		private int yIndice;
		private (int, int) GetInteractableKey()
		{
			xIndice = (int)(transform.position.x / 12);
			yIndice = (int)(transform.position.y / 12);
			return (xIndice, yIndice);
		}

		private void Update()
		{
			var newKey = GetInteractableKey();
			if (newKey != lastInteractableKey)
			{
				Game.Instance.InteractablesManager?.UpdateKey(this, lastInteractableKey, newKey);
				lastInteractableKey = newKey;
			}
			brain.Update(Time.deltaTime * Constants.REAL_TIME_MULTIPLIER);
			StatsManager.Update(Time.deltaTime * Constants.REAL_TIME_MULTIPLIER);
		}

		public void Die(string causeOfDeath)
		{
			//TODO : Actual death logic
			var deathText = Name + " died at the age " + StatsManager.Age.GetAge().ToString() + ". Cause of death: " + causeOfDeath;
			Game.Instance.UIManager?.UILog?.AddLog(deathText, UI.Event.DIED);
			Debug.Log(deathText);
			Game.Instance.AgentsManager.Unregister(this);
			gameObject.SetActive(false);
		}

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			var possibleActions = new List<IAction>();
			possibleActions.Add(new Talk(agent, this));
			if (BreedController.CanBreed() && agent.BreedController.CanBreed())
				possibleActions.Add(new Breed(agent, this));
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
