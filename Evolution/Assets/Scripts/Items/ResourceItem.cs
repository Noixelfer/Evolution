namespace Evolution.Items
{
	public class ResourceItem : BaseItem
	{
		public ResourceItem(ItemDefinition itemDefinition, string id) : base(itemDefinition)
		{
			ID = id;
		}

		public override string ID { get; protected set; }
	}
}