using Evolution.Craftable;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public abstract class BaseNaturalResource
	{
		public List<CollectableResource> BaseResources;
		public int Quantity;
		public BaseTool RequiredTool;
	}

	[System.Serializable]
	public class CollectableResource
	{
		public BaseResource BaseResource;
		public float Amount;
		public float DropChance;
	}
}
