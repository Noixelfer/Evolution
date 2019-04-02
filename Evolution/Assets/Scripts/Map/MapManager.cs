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

		public void GenerateMap(int width, int height)
		{
		}

		private void Awake()
		{
			GenerateMap(200, 200);
		}
	}
}
