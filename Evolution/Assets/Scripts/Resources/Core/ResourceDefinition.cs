using System.Collections.Generic;

namespace Evolution.Resourcess
{
	[System.Serializable]
	public abstract class ResourceDefinition : BaseDefinition
	{
		public List<string> Attributes;
	}
}
