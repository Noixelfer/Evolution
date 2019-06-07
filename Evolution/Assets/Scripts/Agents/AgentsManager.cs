using System.Collections.Generic;

namespace Evolution.Character
{
	public class AgentsManager
	{
		private static int currentID = 0;
		private Dictionary<int, IAgent> allAgents;
		public Game Game => Game.Instance;

		public AgentsManager()
		{

		}

		public void Register(IAgent agent)
		{
			if (agent == null)
				return;
			if (agent.ID != -1 && allAgents.ContainsKey(agent.ID))
				return;

			agent.ID = currentID;
			allAgents.Add(currentID, agent);
			currentID++;
		}
	}
}
