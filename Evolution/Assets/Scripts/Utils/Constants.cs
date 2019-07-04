using UnityEngine;
namespace Evolution
{
	public class Constants : MonoBehaviour
	{
		//-----------------------------VALUES THAT CAN BE CHANGED IN OPTIONS
		public static int REAL_TIME_MULTIPLIER = 200;
		public static int HOUR_IN_SECONDS = 5;
		public static int HOURS_IN_A_DAY = 24;
		public static int DAYS_IN_A_MONTH = 30;
		public static int DAYS_IN_A_YEAR = 360;


		//------------------------------VALUES IN SECONDS-----------------------------------------------
		public static int SECONDS_IN_A_DAY => HOURS_IN_A_DAY * HOUR_IN_SECONDS;
		public static int SECONDS_IN_A_MONTH => DAYS_IN_A_MONTH * SECONDS_IN_A_DAY;
		public static int SECONDS_IN_A_YEAR => DAYS_IN_A_YEAR * SECONDS_IN_A_DAY;

		//--------------------------------------------------------------------------------------------------------------------------------------------------------------
		public static float AGENT_SPEED_HOUR => 10.0f / (HOUR_IN_SECONDS);
		public static float COLLECT_APPLE_TIME => 1f * HOUR_IN_SECONDS;
		public static float MINE_TIME => 1f * HOUR_IN_SECONDS;

		//Agent
		public static float MAXIMUM_HUNGER = 250f;
		public static float MAXIMUM_ENERGY = 300f;

		//Stats constants
		public static readonly float MINIMUM_BREED_AGE = 16f;
		public static readonly float MAXIMUM_BREED_AGE = 50f;
		public static readonly int MAXIMUM_NUMBER_OF_CHILDS = 4;
		public static readonly int PAUSE_YEARS_BETWEEN_BREEDS = 1;
		public static readonly float ENERGY_CRITICAL_TRESHOLD = 0.2f;
		public static readonly float HUNGER_CRITICAL_TRESHOLD = 0.25f;
		public static float HUNGER_DECREASE_RATE => MAXIMUM_HUNGER / (7f * SECONDS_IN_A_DAY);
		public static float ENERGY_DECREASE_RATE => MAXIMUM_ENERGY / (4 * SECONDS_IN_A_DAY);
		public static float ENERGY_REGEN_RESTING => MAXIMUM_ENERGY / (7 * HOUR_IN_SECONDS);

		//Resources
		public static float APPLE_TREE_AMMOUNT = 0.35f;
		public static float APPLE_GROW_TIME => 1.2f * SECONDS_IN_A_DAY;

		//Axis
		public static readonly string MOUSE_SCROLLWHEEL_AXIS = "Mouse ScrollWheel";
		public static readonly string HORIZONTAL_AXIS = "Horizontal";
		public static readonly string VERTICAL_AXIS = "Vertical";
	}
}
