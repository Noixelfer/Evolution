using Evolution.Map;
using UnityEngine;

//Singleton
namespace Evolution
{
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

		public MapManager MapManager;
		public PrefabsManager PrefabsManager;
		private void Awake()
		{
			//MapManager = gameObject.AddComponent<MapManager>();
			PrefabsManager = new PrefabsManager();
		}
	}
}
