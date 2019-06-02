using System.IO;
using UnityEngine;

namespace Evolution.Utils
{
	public class Utility : MonoBehaviour
	{
		public static T LoadFromJson<T>(string path)
		{
			if (File.Exists(path))
			{
				var jsonObject = File.ReadAllText(path);
				if (jsonObject != null && !jsonObject.Equals(""))
				{
					return JsonUtility.FromJson<T>(jsonObject);
				}
			}
			return default;
		}

		public static T AddComponentIfNotExisting<T>(GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null)
				component = gameObject.AddComponent<T>();
			return component;
		}
	}
}
