using System.Linq;
using UnityEngine;

namespace Evolution.UI
{
	public class UIManager
	{
		private UIMainMenu uiMainMenuCached;
		private UISettingsPannel uiSettingsPannelCached;
		private UILog uiLogCached;

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

		public UILog UILog
		{
			get
			{
				if (uiLogCached == null)
					uiLogCached = Utils.Utility.FindObjectsOfTypeAll<UILog>(true).FirstOrDefault();
				return uiLogCached;
			}
		}
	}
}
