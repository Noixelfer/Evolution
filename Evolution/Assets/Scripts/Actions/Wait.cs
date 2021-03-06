﻿using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	/// <summary>
	/// Action to wait a certain amount of time or until Resolve method is called.
	/// if the constructor is empty, the action will return IN_PROGRESS untill resolved is called
	/// </summary>
	public class Wait : BaseAction
	{
		public override string ID => "Wait";
		private float waitTime;
		private bool resolved = false;
		public Agent Requester { get; private set; }

		public Wait(float waitTime = -1)
		{
			this.waitTime = waitTime;
		}

		public Wait(Agent requester, float waitTime = -1)
		{
			Requester = requester;
			this.waitTime = waitTime;
			Description = "Waiting for " + requester.AGENT_ID;
		}

		public void Resolve()
		{
			resolved = true;
		}

		public override ActionStatus OnUpdate(float delatTime)
		{
			if (waitTime != -1)
			{
				waitTime = Mathf.Clamp(waitTime - delatTime, 0, 1);
				if (waitTime == 0)
					return ActionStatus.SUCCESSFULLY_EXECUTED;
			}

			if (resolved)
				return ActionStatus.SUCCESSFULLY_EXECUTED;

			return ActionStatus.IN_PROGRESS;
		}
	}
}
