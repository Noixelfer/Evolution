using Evolution.Actions;
using System.Collections.Generic;

namespace Evolution.Character
{
	public interface IEntity
	{
		string Name { get; set; }
		int Age { get; set; }
		List<IAction> KnownActions { get; set; }
	}
}
