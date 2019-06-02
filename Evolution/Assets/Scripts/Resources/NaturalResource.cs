using Evolution.Actions;
using Evolution.Craftable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Resourcess
{
	public abstract class NaturalResource : BaseInteractable, INaturalResource, ISerializable
	{
		public NaturalResourceDefinition NaturalResourceDefinition;

		public void LoadFromJson()
		{
			if (ID.Equals(""))
				return;
			NaturalResourceDefinition = Utils.Utility.LoadFromJson<NaturalResourceDefinition>(Paths.Paths.NATURAL_RESOURCES_JSON + ID);
			if (NaturalResourceDefinition == null)
				Debug.LogError("Failed to load Natural resource with id " + ID);
		}
	}
}