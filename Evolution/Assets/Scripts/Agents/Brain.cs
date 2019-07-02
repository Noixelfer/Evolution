using Evolution.Actions;
using Evolution.Items;
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
		private Needs Needs;

		public Brain(Agent agent)
		{
			knownActions = new List<IAction>();
			actionsToExecute = new Stack<IAction>();
			knownInteractables = new HashSet<string>() { "Agent" };
			this.agent = agent;
			Needs = new Needs(this, agent);
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

			//if (InCriticalCondition())
			//{
			//	PickSurvivingAction();
			//	return;
			//}
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
			var keys = new HashSet<(int, int)>(invalidPoints.Keys);
			foreach (var key in keys)
			{
				invalidPoints[key] -= deltaTime;
				if (invalidPoints[key] <= 0)
					pointsToBeRemoved.Add(key);
			}

			foreach (var key in pointsToBeRemoved)
				invalidPoints.Remove(key);

			pointsToBeRemoved = null;

			foreach (var key in pendingInvalidPoints.Keys)
				invalidPoints.Add(key, pendingInvalidPoints[key]);
			pendingInvalidPoints.Clear();
		}

		private void PickSurvivingAction()
		{

		}

		private float minDistance;
		private void PickAction()
		{
			//Scan for interactables
			var interactables = Game.Instance.InteractablesManager.GetInteractablesAround(agent.Transform.position, 5f);

			//Get the list of possible actions
			HashSet<ActionScore> possbileActions = GetCurrentAvailableActions();
			HashSet<string> interactablesType = new HashSet<string>();

			foreach (var interactable in interactables)
			{
				if (interactable.Equals(agent))
					continue;
				if (!interactablesType.Contains(interactable.ID))
				{
					//interactablesType.Add(interactable.ID);
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

			CalculateScoreForActions(possbileActions);

			//Pick an action from best actions
			var pickedAction = ChooseAction(possbileActions);

			if ((agent.Transform.position - pickedAction.Interactable.gameObject.transform.position).sqrMagnitude >= 2.5f)
			{
				//Get the closest free tile from our current interractable
				minDistance = 100000;

				var posX = (int)pickedAction.Interactable.gameObject.transform.position.x;
				var posY = (int)pickedAction.Interactable.gameObject.transform.position.y;

				Node endNode = null;
				foreach (var offset in Utils.MapConstants.neighboursOffset)
				{
					var node = graph.GetNode(posX + offset.Item1, posY + offset.Item2);
					if (node != null && !invalidPoints.ContainsKey((node.xPosition, node.yPosition)))
					{
						var squaredDistance = (posX - agent.transform.position.x) * (posX - agent.transform.position.x) + (posY - agent.transform.position.y) * (posY - agent.transform.position.y);
						if (squaredDistance < minDistance)
						{
							minDistance = squaredDistance;
							endNode = node;
						}
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

		private void CalculateScoreForActions(HashSet<ActionScore> possibleActions)
		{
			foreach (var action in possibleActions)
			{
				action.Score = action.Action.GetScoreBasedOnTraits();
				action.Score = Mathf.Clamp(action.Score + Random.Range(-0.12f, 0.12f), 0, 1);
				//Add score for basic needs
				if (action.Action.Effects.Contains(ActionEffects.RESTORE_HUNGER))
					action.Score += Needs.GetHungerNeed();
				if (action.Action.Effects.Contains(ActionEffects.OBTAINS_FOOD))
					action.Score += Needs.GetHungerNeed() * 0.6f;
				if (action.Action.Effects.Contains(ActionEffects.RESTORE_ENERGY))
					action.Score += Needs.GetSleepNeed();
			}
		}

		private ActionScore ChooseAction(HashSet<ActionScore> actions)
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

		/// <summary>
		/// Gets the list of available actions for our agent without the interaction with the environment
		/// </summary>
		///
		private HashSet<ActionScore> availableActions = new HashSet<ActionScore>();
		private HashSet<ActionScore> GetCurrentAvailableActions()
		{
			availableActions.Clear();
			//He will always be able to sleep
			var sleepTime = 5 * Constants.HOUR_IN_SECONDS;
			availableActions.Add(new ActionScore(new Sleep(agent, sleepTime), agent));
			foreach (var edibleItemId in ItemsUtils.EDDDIBLE_ITEMS)
			{
				var item = agent.Inventory.GetItem(edibleItemId);
				if (item.Item1 != null)
				{
					var action = new Eat(agent, (BaseEdibleItem)item.Item1, 0.4f * Constants.HOUR_IN_SECONDS);
					availableActions.Add(new ActionScore(action, agent));
					break;
				}
			}
			return availableActions;
		}
	}
}
