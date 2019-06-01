using Evolution.Craftable;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public abstract class NaturalResourceDefinition : BaseDefinition
	{
		public List<CollectableResource> BaseResources;
		public int Quantity;
		public BaseTool RequiredTool;
	}

	[System.Serializable]
	public class CollectableResource
	{
		public ResourceDefinition ResourceDefinition;
		public float Amount;
		public float DropChance;
	}
}
