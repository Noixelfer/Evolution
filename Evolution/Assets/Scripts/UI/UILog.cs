using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Evolution.UI
{
	public enum Event
	{
		DIED = 0,
		BREED = 1,
		DISCOVERED = 2,
	}

	public class UILog : MonoBehaviour
	{
		public GameObject Chat;
		private List<TextMeshProUGUI> logs = new List<TextMeshProUGUI>();
		private int maxLogs = 6;
		private float[] opacityList = new float[] { 1, 1, 1, 1, 0.5f, 0.2f };

		public void AddLog(string text, Event eventType)
		{
			var newLog = CreateLog(text, eventType);
			if (logs.Count == maxLogs)
			{
				var lastText = logs[logs.Count - 1];
				logs.RemoveAt(logs.Count - 1);
				MonoBehaviour.Destroy(lastText.gameObject);
			}

			logs.Insert(0, newLog);
			for (int i = 0; i < logs.Count; i++)
			{
				var color = logs[i].color;
				color.a = opacityList[i];
				logs[i].color = color;
			}
		}

		private TextMeshProUGUI CreateLog(string text, Event eventType)
		{
			var log = Instantiate(Game.Instance.PrefabsManager.GetPrefab<TextMeshProUGUI>("LogText"), Chat.transform);
			log.text = text;
			log.color = GetColorBasedOnEvent(eventType);
			return log;
		}

		private Color GetColorBasedOnEvent(Event eventType)
		{
			Color result = Color.white;
			switch (eventType)
			{
				case Event.BREED:
					result = Color.cyan;
					break;
				case Event.DIED:
					result = Color.red;
					break;
				case Event.DISCOVERED:
					result = Color.yellow;
					break;
				default:
					result = Color.white;
					break;
			}
			return result;
		}
	}
}