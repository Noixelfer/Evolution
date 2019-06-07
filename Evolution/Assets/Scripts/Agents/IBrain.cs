using Evolution.Actions;

namespace Evolution.Character
{
	public interface IBrain
	{
		SocialInteraction CurrentSocialInteraction { get; set; }
		void OnDeath();
		void AddKnownInteractable(string interactableID);
	}
}
