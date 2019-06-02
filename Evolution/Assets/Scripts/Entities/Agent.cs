using System.Collections.Generic;
using Evolution.Actions;
using UnityEngine;

namespace Evolution.Character
{
	public class Agent : MonoBehaviour, IAgent
	{
		public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public int Age { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public List<IAction> KnownActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public Transform Transform => transform;

		private void Start()
		{
			var moveTask = new MoveAction(this, transform.position + new Vector3(3, 1));
			moveTask.Execute();
		}
	}
}
