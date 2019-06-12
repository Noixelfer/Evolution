namespace Evolution.Character
{
	public interface IBrain
	{
		void OnDeath();
		void AddKnownInteractable(string interactableID);
		void MarkInvalidPoint(int posX, int posY);
	}
}
