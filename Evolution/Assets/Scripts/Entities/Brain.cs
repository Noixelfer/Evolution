using Evolution.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public enum CharacterState
	{
		IDLE = 0,
		IN_ACTION = 1
	}
	public class Brain : IBrain
	{
		private List<IAction> knownActions;
		private Stack<IAction> actionsToExecute;
		private IAction currentAction = null;
		private IAgent Agent;
		private HashSet<string> knownInteractables;

		public Brain(IAgent Agent)
		{
			knownActions = new List<IAction>();
			actionsToExecute = new Stack<IAction>();
			knownInteractables = new HashSet<string>();
			this.Agent = Agent;
		}

		public void AddKnownInteractable(string interactableID)
		{
			knownInteractables.Add(interactableID);
		}

		public void Update(Time time)
		{
			if (currentAction == null)
			{
				if (actionsToExecute.Count > 0)
				{
					currentAction = actionsToExecute.Pop();
					currentAction.Execute();
				}
				else
					PickAction();
			}
			else
			{
				if (currentAction.Status == ActionStatus.FAILED || currentAction.Status == ActionStatus.SUCCESSFULLY_EXECUTED)
					currentAction = null;
			}
		}

		private void PickAction()
		{
			//Scan for interactables
			var interactables = Game.Instance.InteractablesManager.GetInteractablesAround(Agent.Transform.position, 10f);

			//Get the list of possible actions
			foreach (var interactable in interactables)
			{
				if (knownInteractables.Contains(interactable.ID))
				{
					//We already know this type of interactable, we have to add the possible actions for it
				}
				else
				{
					//We don't know this type of interactable so we will add an analyze action for it
				}
			}
		}
	}
}
