using System.Collections.Generic;
using Evolution.Actions;

namespace Evolution.Character
{
	public class Entity : IEntity
	{
		public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public int Age { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
		public List<IAction> KnownActions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	}
}
