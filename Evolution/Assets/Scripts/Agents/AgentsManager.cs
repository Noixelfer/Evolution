﻿using System.Collections.Generic;

namespace Evolution.Character
{
	public class AgentsManager
	{
		private static int currentID = 0;
		private Dictionary<int, Agent> allAgents = new Dictionary<int, Agent>();
		public Dictionary<int, Agent> AllAgents => allAgents;

		public Game Game => Game.Instance;

		public AgentsManager()
		{

		}

		public void Register(Agent agent)
		{
			if (agent == null)
				return;
			if (agent.AGENT_ID != -1 && allAgents.ContainsKey(agent.AGENT_ID))
				return;

			agent.AGENT_ID = currentID;
			allAgents.Add(currentID, agent);
			currentID++;
		}

		public void Unregister(Agent agent)
		{
			if (agent == null)
				return;
			if (allAgents.ContainsKey(agent.AGENT_ID))
				allAgents.Remove(agent.AGENT_ID);
		}
	}
}
