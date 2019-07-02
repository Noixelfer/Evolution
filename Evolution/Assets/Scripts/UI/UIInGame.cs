using UnityEngine;
using UnityEngine.UI;

namespace Evolution.UI
{
	public class UIInGame : MonoBehaviour
	{
		public Button Options;

		private void Awake()
		{
			Options?.onClick.AddListener(OpenOptionsPanel);
		}

		private void OnDestroy()
		{
			Options?.onClick.RemoveListener(OpenOptionsPanel);
		}

		private void OpenOptionsPanel()
		{
			if (Game.Instance.UIManager != null)
				Game.Instance.UIManager.UISettingsPannel?.Visible(true);
		}
	}
}
