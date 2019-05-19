using UnityEngine;

namespace Evolution.Map
{
	public class MapManager : MonoBehaviour
	{
		public Map Map;
		//Water Ponds
		public float waterPondsNoiseScale = 5;
		[Range(0, 1)]
		public float WaterPondsMinValue = 0.74f;
		//Trees
		public float treesNoiseScale = 18;
		[Range(0, 1)]
		public float treesMinValue = 0.5f;
		//Rocks
		public float rockNoiseScale = 5;
		[Range(0, 1)]
		public float rockMinValue = 0.5f;

		private GameObject mapContainer;
		private ResourceTypeGenerator ResourceTypeGenerator;

		public void GenerateMap(int width, int height)
		{
			if (ResourceTypeGenerator == null)
				ResourceTypeGenerator = new ResourceTypeGenerator();

			Map.Size = new Vector2(width, height);
			GenerateEmptyMap();
			//GenerateWaterPonds
			GenerateResourceFromNoise(waterPondsNoiseScale, WaterPondsMinValue, "water.json");
			GenerateResourceFromNoise(treesNoiseScale, treesMinValue, "trees.json");
			GenerateResourceFromNoise(rockNoiseScale, rockMinValue, "stones.json");
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
					float xCoord = xOffset + (float)x / width * scale;
					float yCoord = yOffset + (float)y / height * scale;
					var value = Mathf.PerlinNoise(xCoord, yCoord);
					result[x, y] = value;
				}
			}
			return result;
		}

		private void GenerateResourceFromNoise(float scale, float minValue, string id, bool decoration = false)
		{
			var noiseMap = GetNoiseMap((int)Map.Size.x, (int)Map.Size.y, scale);
			for (int x = 0; x < Map.Size.x; x++)
			{
				for (int y = 0; y < Map.Size.y; y++)
				{
					var position = new Vector2(x, y);
					if (Map.FreeTile(position) && noiseMap[x, y] >= minValue)
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

		private void Awake()
		{
			mapContainer = GameObject.Find("MapContainer");
			if (mapContainer == null)
				mapContainer = new GameObject("MapContainer");
			Map = new Map();
			GenerateMap(50, 50);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				GenerateMap(50, 50);
		}
	}
}
