using Evolution.Character;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution
{
	public class SelectionManager
	{
		public Action<Agent> OnAgentSelected;
		public Agent SelectedAgent { get; protected set; } = null;
		private AgentsManager AgentsManager => Game.Instance.AgentsManager;
		private HashSet<int> visitedAgentsKeys = new HashSet<int>();

		public SelectionManager()
		{
			SelectNextAgent();
		}

		public void SelectNextAgent()
		{
			if (AgentsManager.AllAgents.Count == 0)
				return;
			var remainingKeys = new HashSet<int>();
			foreach (var key in AgentsManager.AllAgents.Keys)
				remainingKeys.Add(key);
			remainingKeys.ExceptWith(visitedAgentsKeys);

			if (remainingKeys.Count == 0)
			{
				visitedAgentsKeys.Clear();
				foreach (var key in AgentsManager.AllAgents.Keys)
					remainingKeys.Add(key);
			}

			var agentID = remainingKeys.ToList()[0];
			visitedAgentsKeys.Add(agentID);
			SelectAgent(AgentsManager.AllAgents[agentID]);
			SelectedAgent = AgentsManager.AllAgents[agentID];

		}

		private void SelectAgent(Agent agent)
		{
			OnAgentSelected?.Invoke(agent);
		}
	}
}
