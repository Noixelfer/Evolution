namespace Evolution.Items
{
	public class BaseEdibleItem : IItem
	{
		public BaseEdibleItem(ItemDefinition itemDefinition, float hungerRestored)
		{
			ItemDefinition = itemDefinition;
			HungerRestored = hungerRestored;
		}
		public ItemDefinition ItemDefinition;
		public float HungerRestored;
		public string Icon => "";
		public float Weight => ItemDefinition.Weight;
	}
}
