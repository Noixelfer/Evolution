using System.Linq;
using UnityEngine;

namespace Evolution.UI
{
	public class UIManager
	{
		private UIMainMenu uiMainMenuCached;
		private UISettingsPannel uiSettingsPannelCached;

		public UIMainMenu UIMainMenu
		{
			get
			{
				if (uiMainMenuCached == null)
					uiMainMenuCached = Utils.Utility.FindObjectsOfTypeAll<UIMainMenu>(true).FirstOrDefault();
				return uiMainMenuCached;
			}
		}

		public UISettingsPannel UISettingsPannel
		{
			get
			{
				if (uiSettingsPannelCached == null)
					uiSettingsPannelCached = Utils.Utility.FindObjectsOfTypeAll<UISettingsPannel>(true).FirstOrDefault();
				return uiSettingsPannelCached;
			}
		}
	}
}
