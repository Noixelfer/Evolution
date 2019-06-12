using System;
using System.Collections.Generic;

namespace Evolution.Actions
{
	/// <summary>
	/// Base class for all actions.
	/// </summary>
	public abstract class BaseAction : IAction
	{
		public Action OnStartAction { get; set; }
		public Action<float> OnUpdateAction { get; set; }
		public Action OnEndAction { get; set; }
		public Action OnPauseAction { get; set; }
		public Action OnFailedAction { get; set; }

		public virtual string Description { get; protected set; } = "";
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
			OnEndAction?.Invoke();
		}

		/// <summary>
		/// This function is called right before Execute code
		/// </summary>
		public virtual void OnStart()
		{
			Status = ActionStatus.IN_PROGRESS;
			OnStartAction?.Invoke();
		}

		public virtual ActionStatus OnUpdate(float time)
		{
			OnUpdateAction?.Invoke(time);
			return ActionStatus.IN_PROGRESS;
		}

		/// <summary>
		/// Function to pause the execution of the current action
		/// </summary>
		public virtual void Pause()
		{
			OnPauseAction?.Invoke();
			Status = ActionStatus.PAUSED;
		}

		public void SetStatus(ActionStatus status)
		{
			Status = status;
		}

		/// <summary>
		/// Action to calculate the Score using weights for traits
		/// </summary>
		/// <returns></returns>
		public virtual float GetScoreBasedOnTraits()
		{
			return 0;
		}
	}
}
