using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evolution.Actions
{
	public class ActionsManager : MonoBehaviour
	{
		private HashSet<IAction> actions = new HashSet<IAction>();

		public void Register(IAction action)
		{
			if (action != null)
				actions.Add(action);
		}

		public void Unregister(IAction action)
		{
			actions.Remove(action);
		}

		private void Update()
		{
			foreach (var action in actions.ToList())
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
						Unregister(action);
					}
				}
			}
		}
	}
}
