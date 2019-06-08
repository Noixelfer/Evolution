using Evolution.Actions;

namespace Evolution.Character
{
	public interface ISocialInteraction
	{
		void StopWaiting(Agent agent);
		void RequesterArrived(Agent requester, IAction socialAction);
	}
}