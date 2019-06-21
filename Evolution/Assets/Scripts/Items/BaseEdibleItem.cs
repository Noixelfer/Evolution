namespace Evolution.Items
{
	public class BaseEdibleItem : BaseItem
	{
		public BaseEdibleItem(ItemDefinition itemDefinition, float hungerRestored, string id) : base(itemDefinition)
		{
			HungerRestored = hungerRestored;
			ID = id;
		}

		public float HungerRestored;

		public override string ID { get; protected set; }
	}
}
