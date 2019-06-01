using System.Collections.Generic;

namespace Evolution.Resourcess
{
	[System.Serializable]
	public abstract class BaseResource
	{
		public string Name;
		public List<string> Attributes;
	}
}
