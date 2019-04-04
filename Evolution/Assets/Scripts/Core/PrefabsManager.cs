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

		private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
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

		public GameObject GetPrefab(string id)
		{
			if (prefabs.ContainsKey(id))
				return prefabs[id];
			else
			{
				if (paths.Any(pathData => GetPrefabForPathAndId(pathData.path, id) != null))
					return prefabs[id];
				else
				{
					Debug.LogError("There was no prefab with the id " + id + ". Maybe the path leading to that prefab is not saved in paths or there is no prefab with the given id");
					return null;
				}
			}
		}

		private GameObject GetPrefabForPathAndId(string path, string id)
		{
			var prefab = Resources.Load(path + id) as GameObject;
			if (prefab != null)
				prefabs.Add(id, prefab);
			return prefab;

		}
	}
}
