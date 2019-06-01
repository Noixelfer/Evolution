using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Evolution.Utils
{
	public static class MapConstants
	{
		#region Grass
		public static readonly string GRASS_01 = "grass_01";
		public static readonly string GRASS_02 = "grass_02";
		public static readonly string GRASS_03 = "grass_03";
		public static readonly string GRASS_04 = "grass_04";
		public static readonly string GRASS_05 = "grass_05";
		public static readonly string GRASS_06 = "grass_06";
		public static readonly string GRASS_07 = "grass_07";
		public static readonly string GRASS_08 = "grass_08";
		public static readonly string GRASS_09 = "grass_09";
		public static readonly string GRASS_10 = "grass_10";
		public static readonly string GRASS_11 = "grass_11";
		#endregion
		public static IList<(int, int)> neighboursOffset = new ReadOnlyCollection<(int, int)>(new List<(int, int)>{ (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) });
}
}
