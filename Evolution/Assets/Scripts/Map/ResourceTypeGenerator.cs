using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Evolution.Map
{
	public class ResourceTypeGenerator
	{
		[Serializable]
		private class ResourcesDataList
		{
			public List<ResourcesData> resourcesData;
		}

		[Serializable]
		private class ResourcesData
		{
			public string name;
			public int spawnChance;
		}

		private Dictionary<string, ResourcesDataList> cachedResources;
		private const string RESOURCE_DATA_FOLDER = "/ResourcesData/";

		public ResourceTypeGenerator()
		{
			cachedResources = new Dictionary<string, ResourcesDataList>();
		}

		public string GetResourcePrefabName(string resourceName)
		{
			string resourcePrefabName = "";
			if (cachedResources.ContainsKey(resourceName))
			{
				resourcePrefabName = GetResourceNameFromSpawnChance(cachedResources[resourceName]);
			}
			else
			{
				string filePath = Application.streamingAssetsPath + RESOURCE_DATA_FOLDER + resourceName;
				if (File.Exists(filePath))
				{
					string dataAsJson = File.ReadAllText(filePath);
					var data = JsonUtility.FromJson<ResourcesDataList>(dataAsJson);
					if (data != null)
					{
						cachedResources.Add(resourceName, data);
						resourcePrefabName = GetResourceNameFromSpawnChance(data);
					}
					else
					{
						Debug.LogError("Could not read json from " + filePath);
					}
				}
			}
			return resourcePrefabName;
		}

		/// <summary>
		/// Returns the name of the resource tile after getting it randomly from spawn chance
		/// </summary>
		/// <param name="resourcesDataList">The list with resources and spawn chances</param>
		/// <returns></returns>
		private string GetResourceNameFromSpawnChance(ResourcesDataList resourcesDataList)
		{
			if (resourcesDataList == null)
			{
				Debug.LogError("Tried to get resource from null ResourcesDataList!!!");
				return "";
			}
			int maxRandomNumber = 0;
			foreach (var resource in resourcesDataList.resourcesData)
			{
				maxRandomNumber += resource.spawnChance;
			}
			int randomNumber = UnityEngine.Random.Range(0, maxRandomNumber);
			int currentNumber = 0;
			for (int i = 0; i < resourcesDataList.resourcesData.Count; i++)
			{
				var currentResource = resourcesDataList.resourcesData[i];
				currentNumber += currentResource.spawnChance;
				if (currentNumber >= randomNumber)
					return currentResource.name;
			}
			return "";
		}

	}
}
