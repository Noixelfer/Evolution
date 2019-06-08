namespace Evolution.Character
{
	public interface IBrain
	{
		void OnDeath();
		void AddKnownInteractable(string interactableID);
	}
}
