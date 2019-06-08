using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Actions
{
	public class ActionsManager : MonoBehaviour
	{
		private HashSet<IAction> actions = new HashSet<IAction>();
		private HashSet<IAction> actionsToBeAdded = new HashSet<IAction>();
		private HashSet<IAction> actionsToBeRemoved = new HashSet<IAction>();

		public void Register(IAction action)
		{
			actionsToBeAdded.Add(action);
		}

		public void Unregister(IAction action)
		{
			actionsToBeRemoved.Add(action);
		}

		private void Update()
		{
			foreach (var action in actions)
			{
				if (action.Status == ActionStatus.IN_PROGRESS)
				{
					var resultStatus = action.OnUpdate(Time.deltaTime);
					if (resultStatus == ActionStatus.SUCCESSFULLY_EXECUTED)
					{
						action.OnEnd();
						Unregister(action);
					}
					if (resultStatus == ActionStatus.FAILED)
					{
						action.OnFailedAction?.Invoke();
						action.SetStatus(ActionStatus.FAILED);
						Unregister(action);
					}
				}
				else if (action.Status == ActionStatus.FAILED)
				{
					action.OnFailedAction?.Invoke();
					Unregister(action);
				}
				else if (action.Status == ActionStatus.SUCCESSFULLY_EXECUTED)
				{
					Unregister(action);
				}
			}
		}

		private void LateUpdate()
		{
			//Add and remove registered/unregistered actions here so that it won't change the Set during iteration
			foreach (var action in actionsToBeAdded)
				actions.Add(action);
			foreach (var action in actionsToBeRemoved)
				actions.Remove(action);
			actionsToBeAdded.Clear();
			actionsToBeRemoved.Clear();
		}
	}
}
