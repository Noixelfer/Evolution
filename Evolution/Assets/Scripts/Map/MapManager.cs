using Evolution.Character;
using Evolution.Resourcess;
using UnityEngine;

namespace Evolution.Map
{
	public class MapManager : MonoBehaviour
	{
		public Map Map;
		public MapGraph MapGraph;
		public float TreesAmount = 0.5f;
		public float RocksAmount = 0.5f;
		public float WaterAmount = 0.5f;
		public int InitialPopulationAmount = 6;

		//Water Ponds
		public float waterPondsNoiseScale = 5;
		[Range(0, 1)]
		public float WaterPondsMinValue = 0.83f;
		//Trees
		public float treesNoiseScale = 18;
		[Range(0, 1)]
		public float treesMinValue = 0.75f;
		//Rocks
		public float rockNoiseScale = 5;
		[Range(0, 1)]
		public float rockMinValue = 0.84f;

		private GameObject mapContainer;
		private GameObject agentsContainer;
		private ResourceTypeGenerator ResourceTypeGenerator;
		private readonly float AMOUNT_IMPORTANCE = 0.2f;

		public void GenerateMap(int width, int height)
		{
			DontDestroyOnLoad(mapContainer);
			if (ResourceTypeGenerator == null)
				ResourceTypeGenerator = new ResourceTypeGenerator();

			Map.Size = new Vector2(width, height);
			GenerateEmptyMap();
			//GenerateWaterPonds
			GenerateTileFromNoise(waterPondsNoiseScale, WaterPondsMinValue, WaterAmount, "water.json");
			PlaceResourceFromNoise<NaturalResource>(treesNoiseScale, treesMinValue, TreesAmount, "trees.json");
			PlaceResourceFromNoise<NaturalResource>(rockNoiseScale, rockMinValue, RocksAmount, "stones.json");

			//Place Agents
			GenerateAgents(InitialPopulationAmount);
			//Create the MapGraph for our current Map
			MapGraph = new MapGraph(Map);
			Game.Instance?.InteractablesManager?.DisableUnreachableIInteractables();
			Game.Instance.SelectionManager.SelectNextAgent();
		}

		private float[,] GetNoiseMap(int width, int height, float scale)
		{
			var xOffset = Random.Range(0, 1000000);
			var yOffset = Random.Range(0, 1000000);
			float[,] result = new float[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					float xCoord = xOffset + (float)x / 50 * scale;
					float yCoord = yOffset + (float)y / 50 * scale;
					var value = Mathf.PerlinNoise(xCoord, yCoord);
					result[x, y] = value;
				}
			}
			return result;
		}

		private void GenerateTileFromNoise(float scale, float minValue, float amount, string id, bool decoration = false)
		{
			var noiseMap = GetNoiseMap((int)Map.Size.x, (int)Map.Size.y, scale);
			for (int x = 0; x < Map.Size.x; x++)
			{
				for (int y = 0; y < Map.Size.y; y++)
				{
					var position = new Vector2(x, y);
					if (Map.FreeTile(position) && noiseMap[x, y] >= minValue - amount * AMOUNT_IMPORTANCE)
					{
						var resourcePrefabName = ResourceTypeGenerator.GetResourcePrefabName(id);
						if (resourcePrefabName != null && resourcePrefabName != "")
						{
							var tile = Game.Instance.PrefabsManager.GetPrefab<Tile>(resourcePrefabName);
							if (tile != null)
							{
								//Map.ClearTile(position);
								var tileClone = Instantiate(tile, position, Quaternion.identity);
								tileClone.MapPosition = position;
								tileClone.transform.SetParent(mapContainer.transform);
								Map.SetTile(tileClone.MapPosition, tileClone);
								if (!decoration)
									tileClone.Occupied = true;
							}
							else
								Debug.LogError("There is no tile with the " + id + " id.");
						}
						else
							Debug.LogError("There is no resource with the " + id + " id.");
					}
				}
			}
		}

		private void PlaceResourceFromNoise<T>(float scale, float minValue, float amount, string id) where T : NaturalResource
		{
			var noiseMap = GetNoiseMap((int)Map.Size.x, (int)Map.Size.y, scale);
			for (int x = 0; x < Map.Size.x; x++)
			{
				for (int y = 0; y < Map.Size.y; y++)
				{
					var position = new Vector2(x, y);
					if (Map.FreeTile(position) && noiseMap[x, y] >= minValue - amount * AMOUNT_IMPORTANCE)
					{
						var resourcePrefabName = ResourceTypeGenerator.GetResourcePrefabName(id);
						if (resourcePrefabName != null && resourcePrefabName != "")
						{
							var resource = Game.Instance.PrefabsManager.GetPrefab<T>(resourcePrefabName);
							if (resource != null)
							{
								//Map.ClearTile(position);
								var resourceClone = Instantiate(resource, position, Quaternion.identity);
								resourceClone.transform.SetParent(mapContainer.transform);
								Map.GetTileValue(new Vector2(x, y)).Occupied = true;
							}
							else
								Debug.LogError("There is no Natural Resource with the " + id + " id.");
						}
						else
							Debug.LogError("There is no resource with the " + id + " id.");
					}
				}
			}
		}

		private void GenerateEmptyMap()
		{
			Map.ClearMap();
			for (int x = 0; x < Map.Size.x; x++)
			{
				for (int y = 0; y < Map.Size.y; y++)
				{
					var grassPrefabName = ResourceTypeGenerator.GetResourcePrefabName("grass.json");
					if (grassPrefabName != null && grassPrefabName != "")
					{
						var grassTile = Instantiate(Game.Instance.PrefabsManager.GetPrefab<Tile>(grassPrefabName), new Vector3(x, y), Quaternion.identity);
						grassTile.MapPosition = new Vector2(x, y);
						grassTile.Occupied = false;
						grassTile.transform.SetParent(mapContainer.transform);
						Map.SetTile(grassTile.MapPosition, grassTile);
					}
					else
						Debug.LogError("There was no resource prefab for grass!");
				}
			}
		}

		private void GenerateAgents(int numberOfAgents)
		{
			//get empty tile
			bool foundTile = false;
			int tileX = 0;
			int tileY = 0;
			for (int i = 0; i < numberOfAgents; i++)
			{
				foundTile = false;
				while (!foundTile)
				{
					tileX = (int)Random.Range(0, Map.Size.x);
					tileY = (int)Random.Range(0, Map.Size.y);

					if (!Map.GetTileValue(new Vector2(tileX, tileY)).Occupied)
						foundTile = true;
				}

				var agent = Instantiate(Game.Instance.PrefabsManager.GetPrefab<Agent>("female_agent"));
				agent.StatsManager.Age.SetAge(Random.Range(14, 25), 0, 0);
				agent.transform.position = new Vector3(tileX, tileY, 0);
				if (agentsContainer == null)
				{
					agentsContainer = new GameObject("Agents Container");
					DontDestroyOnLoad(agentsContainer);
				}
				agent.transform.parent = agentsContainer.transform;
			}
		}

		private void Start()
		{
			mapContainer = GameObject.Find("MapContainer");
			if (mapContainer == null)
				mapContainer = new GameObject("MapContainer");
			Map = new Map();
		}

		private void Update()
		{
			//if (Input.GetKeyDown(KeyCode.Space))
			//	GenerateMap(50, 50);
		}
	}
}
