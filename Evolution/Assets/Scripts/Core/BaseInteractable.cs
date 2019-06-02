﻿using Evolution;
using Evolution.Actions;
using Evolution.Character;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
	public abstract string ID { get; }
	private List<IAction> actions = new List<IAction>();
	private bool loadedActionsFromJson = false;

	public virtual void Awake()
	{
		Game.Instance.InteractablesManager.Register(this);
	}

	public virtual void OnDestroy()
	{
		Game.Instance.InteractablesManager.Unregister(this);
	}

	public abstract List<IAction> GetPossibleActions(IAgent agent);
}
