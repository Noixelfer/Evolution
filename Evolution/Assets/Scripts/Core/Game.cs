using UnityEngine;

//Singleton
public class Game : MonoBehaviour
{
	private static Game gameInstance;

	public static Game Instance
	{
		get
		{
			if (gameInstance == null)
			{
				var container = new GameObject("Game");
				gameInstance = container.AddComponent<Game>();
			}
			return gameInstance;
		}
	}
}
