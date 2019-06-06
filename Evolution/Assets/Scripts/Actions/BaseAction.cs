using System.Collections.Generic;

namespace Evolution.Actions
{
	/// <summary>
	/// Base class for all actions.
	/// </summary>
	public abstract class BaseAction : IAction
	{
		public abstract string ID { get; }

		public List<string> Categories { get; set; }

		public ActionStatus Status { get; private set; }

		public virtual void Initialize(params object[] parameters)
		{
			OnStart();
			Game.Instance.ActionsManager.Register(this);
		}

		public virtual void OnEnd()
		{
			Status = ActionStatus.SUCCESSFULLY_EXECUTED;
		}

		/// <summary>
		/// This function is called right before Execute code
		/// </summary>
		public virtual void OnStart()
		{
			Status = ActionStatus.IN_PROGRESS;
		}

		public virtual ActionStatus OnUpdate(float time)
		{
			return ActionStatus.IN_PROGRESS;
		}

		/// <summary>
		/// Function to pause the execution of the current action
		/// </summary>
		public virtual void Pause()
		{
			Status = ActionStatus.PAUSED;
		}

		public void SetStatus(ActionStatus status)
		{
			Status = status;
		}
	}
}
