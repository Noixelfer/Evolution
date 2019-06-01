using Evolution.Craftable;
using System;
using System.Collections.Generic;

namespace Evolution.Resourcess
{
	public interface INaturalResource
	{
		List<Tuple<IResource, float>> Resourcesss { get; }
		int Quantity { get; }
		ITool RequiredTool { get; }
		IResource Collect(ITool tool);
	}
}
