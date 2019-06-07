using Evolution.Character;

namespace Evolution.Actions
{
	public class SocialInteraction
	{
		private Agent requester;
		private Agent receiver;

		public SocialInteraction(Agent requester, Agent receiver)
		{
			this.requester = requester;
			this.receiver = receiver;
		}

		public void Resolve()
		{

		}

		public void Reject()
		{

		}
	}
}
