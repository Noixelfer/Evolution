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
		private Agent agent;
		private HashSet<string> knownInteractables;
		private MapGraph graph => Game.Instance.MapManager.MapGraph;
		private Dictionary<(int, int), float> invalidPoints = new Dictionary<(int, int), float>();
		private Dictionary<(int, int), float> pendingInvalidPoints = new Dictionary<(int, int), float>();

		public Brain(Agent agent)
		{
			knownActions = new List<IAction>();
			actionsToExecute = new Stack<IAction>();
			knownInteractables = new HashSet<string>() { "Agent" };
			this.agent = agent;
		}

		public void AddKnownInteractable(string interactableID)
		{
			knownInteractables.Add(interactableID);
		}

		public void SetCurretActions(Stack<IAction> actions)
		{
			ClearCurrentActions();
			actionsToExecute = actions;
		}
		public void ClearCurrentActions()
		{
			if (currentAction != null)
			{
				currentAction.SetStatus(ActionStatus.FAILED);
				currentAction = null;
				actionsToExecute.Clear();
			}
		}

		public void StopWaiting(Agent agent)
		{
			if (currentAction != null && currentAction is Wait)
			{
				var waitAgentAction = (Wait)currentAction;
				if (waitAgentAction.Requester != null && waitAgentAction.Requester.Equals(agent))
					waitAgentAction.Resolve();
			}
		}

		public void RequesterArrived(Agent requester, IAction socialAction)
		{
			if (currentAction != null && currentAction is Wait)
			{
				var waitAgentAction = (Wait)currentAction;
				if (waitAgentAction.Requester != null && waitAgentAction.Requester.Equals(agent))
				{
					waitAgentAction.Resolve();
					currentAction = socialAction;
					return;
				}
			}
			socialAction?.SetStatus(ActionStatus.FAILED);
		}

		public void Update(float time)
		{
			UpdateInvalidPoints(time);

			if (InCriticalCondition())
			{
				PickSurvivingAction();
				return;
			}
			if (currentAction == null)
			{
				if (actionsToExecute.Count > 0)
				{
					currentAction = actionsToExecute.Pop();
					currentAction.Initialize();
					agent.UIAgentStatus?.SetStatus(currentAction.Description);
				}
				else
					PickAction();
			}
			else
			{
				if (currentAction.Status == ActionStatus.FAILED)
				{
					currentAction = null;
					actionsToExecute.Clear();
				}
				else if (currentAction.Status == ActionStatus.SUCCESSFULLY_EXECUTED)
					currentAction = null;
			}
		}

		/// <summary>
		/// Clear up states when the agent dies
		/// </summary>
		public void OnDeath()
		{
			currentAction.SetStatus(ActionStatus.FAILED);
			currentAction = null;
		}

		/// <summary>
		/// Marks a position as invalid. Invalid positions will not be selected in the next 5 seconds
		/// </summary>
		/// <param name="posX"></param>
		/// <param name="posY"></param>
		public void MarkInvalidPoint(int posX, int posY)
		{
			pendingInvalidPoints.Add((posX, posY), 5f);
		}

		private void UpdateInvalidPoints(float deltaTime)
		{
			var pointsToBeRemoved = new HashSet<(int, int)>();
			var keys = invalidPoints.Keys;
			foreach (var key in keys)
			{
				invalidPoints[key] -= deltaTime;
				if (invalidPoints[key] <= 0)
					pointsToBeRemoved.Add(key);
			}

			foreach (var key in pointsToBeRemoved)
				invalidPoints.Remove(key);

			foreach (var key in pendingInvalidPoints.Keys)
				invalidPoints.Add(key, pendingInvalidPoints[key]);
			pendingInvalidPoints.Clear();
		}

		private void PickSurvivingAction()
		{
			var sleepAction = new Sleep(agent);

		}

		private void PickAction()
		{
			//Scan for interactables
			var interactables = Game.Instance.InteractablesManager.GetInteractablesAround(agent.Transform.position, 6f);

			//Get the list of possible actions
			List<ActionScore> possbileActions = new List<ActionScore>();
			List<string> interactablesType = new List<string>();

			foreach (var interactable in interactables)
			{
				if (!interactablesType.Contains(interactable.ID))
				{
					interactablesType.Add(interactable.ID);
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
			}

			//TODO : Score the actions
			foreach (var action in possbileActions)
			{
				action.Score = action.Action.GetScoreBasedOnTraits();
			}

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
					if (node != null && !invalidPoints.ContainsKey((node.xPosition, node.yPosition)))
					{
						endNode = node;
						break;
					}
				}

				if (endNode != null)
				{
					var action = pickedAction.Action;
					var moveAction = new MoveAction(agent, new Vector2(endNode.xPosition, endNode.yPosition));
					actionsToExecute.Push(action);
					actionsToExecute.Push(moveAction);

					//If we want to interact with an agent, we have to ask for their permission first
					if (pickedAction.Interactable is Agent)
					{
						var receiverAgent = (Agent)pickedAction.Interactable;
						var askForInteraction = new AskForInteractPermission(agent, (Agent)pickedAction.Interactable);
						askForInteraction.OnEndAction += () => moveAction.OnFailedAction += () => receiverAgent?.StopWaiting(agent);
						actionsToExecute.Push(askForInteraction);
					}
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

			var randomIndice = Random.Range(0, bestActions.Count);
			return bestActions[randomIndice];
		}


		/// <summary>
		/// Returns true if Energy or Hunger is lower than a given treshold, false otherwise
		/// </summary>
		private bool InCriticalCondition()
		{
			var energyPercentage = agent.StatsManager.Energy.Percentage;
			var hungerPercentage = agent.StatsManager.Hunger.Percentage;

			return (energyPercentage <= Constants.ENERGY_CRITICAL_TRESHOLD || hungerPercentage <= Constants.HUNGER_CRITICAL_TRESHOLD);
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
