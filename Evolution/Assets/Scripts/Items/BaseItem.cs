namespace Evolution.Items
{
	public abstract class BaseItem : IItem
	{
		public BaseItem(ItemDefinition itemDefinition)
		{
			ItemDefinition = itemDefinition;
		}
		public ItemDefinition ItemDefinition;
		public string Icon => "";
		public float Weight => ItemDefinition.Weight;

		public abstract string ID { get; protected set; }
	}
}
