using System.Collections.Generic;
using UnityEngine;
namespace Evolution.Map
{
	public class MapManager : MonoBehaviour
	{
		public Map Map;
		public GameObject grass1;
		public GameObject grass2;
		public GameObject grass3;
		public GameObject grass4;
		public GameObject grass5;

		public float scale = 1;
		private float xOffset = 0;
		private float yOffset = 0;

		public void GenerateMap(int width, int height)
		{
			xOffset = Random.Range(0, 1000000);
			yOffset = Random.Range(0, 1000000);
			var noiseMap = GetNoiseMap(width, height);
			GenerateTerrainFromNoise(width, height, noiseMap);
		}

		private float[,] GetNoiseMap(int width, int height)
		{
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

		private void GenerateTerrainFromNoise(int width, int height, float[,] noiseMap)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var val = noiseMap[x, y];
					if (val <= 0.7f)
						Instantiate(Game.Instance.PrefabsManager.GetPrefab("grass"), new Vector3(x, y), Quaternion.identity);
					else
						Instantiate(Game.Instance.PrefabsManager.GetPrefab("tree"), new Vector3(x, y), Quaternion.identity);
				}
			}
		}

		private void AddResources()
		{

		}

		private void Awake()
		{
			GenerateMap(200, 200);
		}
	}
}
