using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Character
{
	public class RelationController
	{
		private Agent owner;
		private Dictionary<Agent, float> friendshipLevels = new Dictionary<Agent, float>();

		public RelationController(Agent owner)
		{
			this.owner = owner;
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
				//TODO : calculate the response based
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
			}

			return response;
		}

		private float SigmoidFunction(float x)
		{
			return Mathf.Exp(7 * x) / (Mathf.Exp(7 * x) + 1);
		}
	}
}
