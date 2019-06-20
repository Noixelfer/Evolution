namespace Evolution.Items
{
	public class BaseItem : IItem
	{
		public BaseItem(ItemDefinition itemDefinition)
		{
			ItemDefinition = itemDefinition;
		}
		public ItemDefinition ItemDefinition;
		public string Icon => "";
		public float Weight => ItemDefinition.Weight;
	}
}
