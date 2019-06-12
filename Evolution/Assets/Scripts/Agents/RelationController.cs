using Evolution.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public class RelationController
	{
		private Agent owner;
		private Dictionary<Agent, float> friendshipLevels = new Dictionary<Agent, float>();
		private Brain brain;

		public RelationController(Agent owner, Brain brain)
		{
			this.owner = owner;
			this.brain = brain;
		}

		public void ChangeFriendshipLevel(Agent other, float value)
		{
			if (!friendshipLevels.ContainsKey(other))
				return;
			friendshipLevels[other] = Mathf.Clamp(friendshipLevels[other] + value, -1, 1);
		}

		public bool RespondToSocialRequest(Agent other)
		{
			var response = false;
			if (!friendshipLevels.ContainsKey(other))
			{
				//TODO : calculate the response based on traits
				response = Random.Range(0f, 1f) < 0.5f;
			}
			else
			{
				//Response will be given based of a sigmoid functio
				//f(x) = e^7x / e^7x + 1
				var chance = Random.Range(0f, 1f);
				response = (chance < SigmoidFunction(friendshipLevels[other]));
			}

			if (response.Equals(true))
			{
				//If we accepted the interact request, we need to drop our current actions and add a Wait action
				var waitAgentAction = new Wait(other, 15f);
				var actionsToExecute = new Stack<IAction>();
				actionsToExecute.Push(waitAgentAction);
				brain.SetCurretActions(actionsToExecute);
			}

			return response;
		}

		private float SigmoidFunction(float x)
		{
			return Mathf.Exp(7 * x) / (Mathf.Exp(7 * x) + 1);
		}
	}
}
