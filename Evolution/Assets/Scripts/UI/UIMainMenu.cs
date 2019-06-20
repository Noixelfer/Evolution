using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Evolution.UI
{
	public class UIMainMenu : MonoBehaviour
	{
		public Slider TreesSlider;
		public Slider RocksSlider;
		public Slider WaterSlider;
		public TMP_Dropdown MapSize;

		public InputField InitialPopulation;
		private bool started = false;
		private Game Game => Game.Instance;

		private void Awake()
		{
			TreesSlider?.onValueChanged.AddListener(OnTreesValueChanged);
			RocksSlider?.onValueChanged.AddListener(OnRocksValueChanged);
			WaterSlider?.onValueChanged.AddListener(OnWaterValueChanged);
			MapSize?.onValueChanged.AddListener(OnMapSizeChanged);
			if (Game != null && Game.MapManager != null)
			{
				TreesSlider.value = Game.MapManager.TreesAmount;
				RocksSlider.value = Game.MapManager.RocksAmount;
				WaterSlider.value = Game.MapManager.WaterAmount;
				if (InitialPopulation != null)
					InitialPopulation.text = Game.MapManager.InitialPopulationAmount.ToString();
			}
		}

		private void OnDestroy()
		{
			TreesSlider?.onValueChanged.RemoveListener(OnTreesValueChanged);
			RocksSlider?.onValueChanged.RemoveListener(OnRocksValueChanged);
			WaterSlider?.onValueChanged.RemoveListener(OnWaterValueChanged);
			MapSize?.onValueChanged.RemoveListener(OnMapSizeChanged);
		}

		private void Start()
		{

		}

		public void StartGame()
		{
			if (started)
				return;
			started = true;
			Game.MapManager.GenerateMap(Game.MAP_SIZE, Game.MAP_SIZE);
			SceneManager.LoadScene("Scene0");
		}

		public void OnInitialPopulationChanged()
		{
			if (Game != null && Game.MapManager != null)
				Game.MapManager.InitialPopulationAmount = int.Parse(InitialPopulation.text);
		}

		private void OnTreesValueChanged(float value)
		{
			if (Game != null && Game.MapManager != null)
				Game.MapManager.TreesAmount = value;
		}

		private void OnRocksValueChanged(float value)
		{
			if (Game != null && Game.MapManager != null)
				Game.MapManager.RocksAmount = value;
		}

		private void OnWaterValueChanged(float value)
		{
			if (Game != null && Game.MapManager != null)
				Game.MapManager.WaterAmount = value;
		}

		private void OnMapSizeChanged(int value)
		{
			if (Game != null)
			{
				switch (value)
				{
					case 0:
						Game.MAP_SIZE = 100;
						break;
					case 1:
						Game.MAP_SIZE = 150;
						break;
					case 2:
						Game.MAP_SIZE = 210;
						break;
					case 3:
						Game.MAP_SIZE = 300;
						break;
					default:
						Game.MAP_SIZE = 100;
						break;
				}
			}
		}

	}
}