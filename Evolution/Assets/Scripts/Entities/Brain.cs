using Evolution.Actions;
using Evolution.Map;
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
		private IAgent agent;
		private HashSet<string> knownInteractables;
		private MapGraph graph => Game.Instance.MapManager.MapGraph;

		public Brain(IAgent agent)
		{
			knownActions = new List<IAction>();
			actionsToExecute = new Stack<IAction>();
			knownInteractables = new HashSet<string>();
			this.agent = agent;
		}

		public void AddKnownInteractable(string interactableID)
		{
			knownInteractables.Add(interactableID);
		}

		public void Update(float time)
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
			var interactables = Game.Instance.InteractablesManager.GetInteractablesAround(agent.Transform.position, 10f);

			//Get the list of possible actions
			List<ActionScore> possbileActions = new List<ActionScore>();
			foreach (var interactable in interactables)
			{
				if (knownInteractables.Contains(interactable.ID))
				{
					//We already know this type of interactable, we have to add the possible actions for it
					foreach (var action in interactable.GetPossibleActions(agent))
						possbileActions.Add(new ActionScore(action, interactable));
				}
				else
				{
					//We don't know this type of interactable so we will add an analyze action for it
					var analyzeAction = new AnalyzeInteractable(agent, interactable);
					possbileActions.Add(new ActionScore(analyzeAction, interactable));
				}
			}

			//TODO : Score the actions

			//Pick an action from best actions
			var pickedAction = ChooseAction(possbileActions);

			if ((agent.Transform.position - pickedAction.Interactable.gameObject.transform.position).sqrMagnitude >= 3f)
			{
				//Get a free tile around the given interactable
				var posX = (int)pickedAction.Interactable.gameObject.transform.position.x;
				var posY = (int)pickedAction.Interactable.gameObject.transform.position.y;

				Node endNode = null;
				foreach (var offset in Utils.MapConstants.neighboursOffset)
				{
					var node = graph.GetNode(posX + offset.Item1, posY + offset.Item2);
					if (node != null)
					{
						endNode = node;
						break;
					}
				}

				if (endNode != null)
				{
					actionsToExecute.Push(pickedAction.Action);
					actionsToExecute.Push(new MoveAction(agent, new Vector2(endNode.xPosition, endNode.yPosition)));
				}
			}
			else
			{
				actionsToExecute.Push(pickedAction.Action);
			}

		}

		private ActionScore ChooseAction(List<ActionScore> actions)
		{
			float maxScore = 0;
			List<ActionScore> bestActions = new List<ActionScore>();

			foreach (var action in actions)
			{
				if (action.Score.Equals(maxScore))
					bestActions.Add(action);
				else if (action.Score > maxScore)
				{
					bestActions = new List<ActionScore>() { action };
					maxScore = action.Score;
				}
			}

			if (bestActions.Count == 0)
			{
				Debug.LogError("No action was found!!!");
				return null;
			}

			var randomIndice = Random.Range(0, bestActions.Count - 1);
			return bestActions[randomIndice];
		}

		private class ActionScore
		{
			public IAction Action { get; private set; }
			public IInteractable Interactable { get; private set; }
			public float Score { get; set; } = 0;

			public ActionScore(IAction action, IInteractable interactable)
			{
				Action = action;
				Interactable = interactable;
			}
		}
	}
}
