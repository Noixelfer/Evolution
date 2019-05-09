using System.Collections.Generic;

namespace Evolution.Actions
{
	/// <summary>
	/// Base class for all actions.
	/// </summary>
	public abstract class BaseAction : IAction
	{

		public List<string> Categories { get; set; }

		public ActionStatus Status { get; private set; }

		public virtual void Execute(params object[] parameters)
		{
			OnStart();
		}

		public virtual void OnEnd()
		{
			Status = ActionStatus.SUCCESSFULLY_EXECUTED;
		}

		/// <summary>
		/// This function is called right before Execute code
		/// </summary>
		public void OnStart()
		{
			Status = ActionStatus.IN_PROGRESS;
		}

		public ActionStatus OnUpdate()
		{
			return ActionStatus.IN_PROGRESS;
		}

		public void Pause()
		{
			Status = ActionStatus.PAUSED;
		}
	}
}
