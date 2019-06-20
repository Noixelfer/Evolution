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
		private bool started = false;
		private Game Game => Game.Instance;

		private void Awake()
		{
			TreesSlider?.onValueChanged.AddListener(OnTreesValueChanged);
			RocksSlider?.onValueChanged.AddListener(OnRocksValueChanged);
			WaterSlider?.onValueChanged.AddListener(OnWaterValueChanged);
			if (Game != null && Game.MapManager != null)
			{
				TreesSlider.value = Game.MapManager.TreesAmount;
				RocksSlider.value = Game.MapManager.RocksAmount;
				WaterSlider.value = Game.MapManager.WaterAmount;
			}
		}

		private void OnDestroy()
		{
			TreesSlider?.onValueChanged.RemoveListener(OnTreesValueChanged);
			RocksSlider?.onValueChanged.RemoveListener(OnRocksValueChanged);
			WaterSlider?.onValueChanged.RemoveListener(OnWaterValueChanged);
		}

		private void Start()
		{

		}

		public void StartGame()
		{
			if (started)
				return;
			started = true;
			Game.MapManager.GenerateMap(100, 100);
			SceneManager.LoadScene("Scene0");
		}

		public void OnInitialPopulationChanged()
		{
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

	}
}