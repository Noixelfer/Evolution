using UnityEngine;

namespace Evolution.Map
{
	public class MapGenerator : MonoBehaviour
	{
		public Map GenerateMap()
		{
			return null;
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
	}
}
