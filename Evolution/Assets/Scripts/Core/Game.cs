using Evolution.Actions;
using Evolution.Character;
using Evolution.Map;
using Evolution.UI;
using Evolution.Utils;
using System.Collections;
using System.Threading;
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

		public PrefabsManager PrefabsManager;
		public AgentsManager AgentsManager;
		public MapManager MapManager;
		public ActionsManager ActionsManager;
		public SelectionManager SelectionManager;
		public InteractablesManager InteractablesManager;
		public InputManager InputManager;
		public UIManager UIManager;
		public AudioSource GameSound;
		public int MAP_SIZE = 100;

		private void Awake()
		{
			if (Instance != null && Instance != this)
				Destroy(this.gameObject);

			PrefabsManager = new PrefabsManager();
			AgentsManager = new AgentsManager();
			SelectionManager = new SelectionManager();
			InputManager = new InputManager();
			UIManager = new UIManager();

			MapManager = Utility.AddComponentIfNotExisting<MapManager>(gameObject);
			ActionsManager = Utility.AddComponentIfNotExisting<ActionsManager>(gameObject);
			InteractablesManager = Utility.AddComponentIfNotExisting<InteractablesManager>(gameObject);
			ThreadPool.SetMaxThreads(20, 20);
			DontDestroyOnLoad(this.gameObject);
		}

		private void Update()
		{
			InputManager.Update();
		}

		public void RunCoroutine(IEnumerator enumerator)
		{
			StartCoroutine(enumerator);
		}
	}
}
