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
		public IBrain Brain => brain;
		private Brain brain;

		private void Start()
		{
			brain = new Brain(this);
			//moveTask.Execute();
		}

		private void Update()
		{
			brain.Update(Time.deltaTime);
		}
	}
}
