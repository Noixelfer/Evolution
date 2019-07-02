using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		public static List<T> FindObjectsOfTypeAll<T>(bool includeInactive = false)
		{
			List<T> results = new List<T>();
			SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<T>(includeInactive)));
			return results;
		}
	}
}
