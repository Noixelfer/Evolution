using UnityEngine;
namespace Evolution
{
	public class Constants : MonoBehaviour
	{
		public static readonly float HOUR_IN_SECONDS = 10;
		public static readonly float SECONDS_IN_A_DAY = 2 * HOUR_IN_SECONDS;
		public static readonly float SECONDS_IN_A_MONTH = 30 * SECONDS_IN_A_DAY;
		public static readonly float SECONDS_IN_A_YEAR = 40 * SECONDS_IN_A_DAY;

		//-------------------------------------EDITOR------------------------------------------------------------------------------------
		public float RealTimeMultiplier = 100;


		//--------------------------------------------------------------------------------------------------------------------------------------------------------------
		//How much faster time passes comparead to real world time
		public static float REAL_TIME_MULTIPLIER = 100;
		public static readonly float AGENT_SPEED_HOUR = 10 / (HOUR_IN_SECONDS);
		public static readonly float COLLECT_APPLE_TIME = 1 * HOUR_IN_SECONDS;
		public static readonly float MINE_TIME = 4 * HOUR_IN_SECONDS;

		//Stats constants
		public static readonly float MINIMUM_BREED_AGE = 16f;
		public static readonly float MAXIMUM_BREED_AGE = 50f;

		public static readonly float HUNGER_DECREASE_RATE = 1 / (3 * SECONDS_IN_A_DAY);
		public static readonly float ENERGY_DECREASE_RATE = 1 / (2 * SECONDS_IN_A_DAY);
		public static readonly float ENERGY_REGEN_RESTING = 1 / (7 * HOUR_IN_SECONDS);


		private void Update()
		{
			REAL_TIME_MULTIPLIER = RealTimeMultiplier;
		}
	}
}
