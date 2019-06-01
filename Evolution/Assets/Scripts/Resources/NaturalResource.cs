using Evolution.Craftable;
using System;
using UnityEngine;

namespace Evolution.Resourcess
{
	public class NaturalResource : MonoBehaviour, INaturalResource, ISerializable
	{
		public NaturalResourceDefinition NaturalResourceDefinition;
		public string id = "";

		public IResource Collect(ITool tool)
		{
			throw new NotImplementedException();
		}

		public void LoadFromJson()
		{
			if (id.Equals(""))
				return;
			NaturalResourceDefinition = Utils.Utility.LoadFromJson<NaturalResourceDefinition>(Paths.Paths.NATURAL_RESOURCES_JSON + id);
			if (NaturalResourceDefinition == null)
				Debug.LogError("Failed to load Natural resource with id " + id);
		}
	}
}