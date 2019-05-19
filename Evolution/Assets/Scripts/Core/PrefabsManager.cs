using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Evolution
{
	public class PrefabsManager 
	{
		[Serializable]
		private class PathDataList
		{
			public List<PathData> pathsData;
		}

		[Serializable]
		private class PathData
		{
			public string name;
			public string path;
		}

		private Dictionary<string, Component> prefabs = new Dictionary<string, Component>();
		private List<PathData> paths = new List<PathData>();

		public PrefabsManager()
		{
			//Load all paths from paths.json
			string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "prefabPaths.json");
			if (File.Exists(filePath))
			{
				string dataAsJson = File.ReadAllText(filePath);
				var data = JsonUtility.FromJson<PathDataList>(dataAsJson);
				paths.AddRange(data.pathsData);
			}
		}

		public T GetPrefab<T>(string id) where T : Component
		{
			if (prefabs.ContainsKey(id))
				return prefabs[id] as T;
			else
			{
				if (paths.Any(pathData => GetPrefabForPathAndId<T>(pathData.path, id) != null))
					return prefabs[id] as T;
				else
				{
					Debug.LogError("There was no prefab with the id " + id + ". Maybe the path leading to that prefab is not saved in paths or there is no prefab with the given id");
					return null;
				}
			}
		}

		private T GetPrefabForPathAndId<T>(string path, string id) where T : Component
		{
			var prefab = Resources.Load<T>(path + id) as T;
			if (prefab != null)
				prefabs.Add(id, prefab);
			return prefab;
		}
	}
}
