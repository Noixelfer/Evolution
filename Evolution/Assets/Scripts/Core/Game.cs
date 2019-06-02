using Evolution.Actions;
using Evolution.Map;
using Evolution.Utils;
using System.Collections;
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
					gameInstance = FindObjectOfType<Game>();
					if (gameInstance == null)
					{
						var container = new GameObject("Game");
						gameInstance = container.AddComponent<Game>();
					}
				}
				return gameInstance;
			}
		}

		public MapManager MapManager;
		public PrefabsManager PrefabsManager;
		public ActionsManager ActionsManager;

		private void Awake()
		{
			PrefabsManager = new PrefabsManager();
			MapManager = Utility.AddComponentIfNotExisting<MapManager>(gameObject);
			ActionsManager = Utility.AddComponentIfNotExisting<ActionsManager>(gameObject);
		}

		public void RunCoroutine(IEnumerator enumerator)
		{
			StartCoroutine(enumerator);
		}
	}
}
