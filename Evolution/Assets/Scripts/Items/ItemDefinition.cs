namespace Evolution.Items
{
	[System.Serializable]
	public class ItemDefinition : BaseDefinition
	{
		public ItemDefinition(float weight, float size)
		{
			Weight = weight;
			Size = size;
		}

		public float Weight;
		public float Size;
	}
}
