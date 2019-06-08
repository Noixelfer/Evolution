using System;
using System.Collections.Generic;

namespace Evolution.Actions
{
	public enum ActionStatus
	{
		NOT_STARTED,
		IN_PROGRESS,
		PAUSED,
		FAILED,
		SUCCESSFULLY_EXECUTED
	}

	public interface IAction
	{
		Action OnStartAction { get; set; }
		Action<float> OnUpdateAction { get; set; }
		Action OnEndAction { get; set; }
		Action OnPauseAction { get; set; }
		Action OnFailedAction { get; set; }

		List<string> Categories { get; set; }
		ActionStatus Status { get; }
		void Initialize(params object[] parameters);
		void OnStart();
		ActionStatus OnUpdate(float time);
		void OnEnd();
		void Pause();
		void SetStatus(ActionStatus status);
	}
}