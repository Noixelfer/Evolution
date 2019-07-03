using UnityEngine;
using UnityEngine.UI;
namespace Evolution.UI
{
	public class UISettingsPannel : MonoBehaviour
	{
		public InputField RealTimeMultiplierInputField;
		public InputField YearInDaysInputField;
		public InputField MonthInDaysInputField;
		public InputField DayInHoursInputField;
		public InputField HourInSecondsInputField;
		public Slider SoundSlider;
		public Toggle SoundToggle;

		public Button Exit;

		private int realTimeMultiplier = 100;
		private int yearInDays = 365;
		private int monthInDays = 30;
		private int dayInHours = 24;
		private int hourInSeconds = 360;

		private void Awake()
		{
			GetValuesFromConstants();

			RealTimeMultiplierInputField?.onEndEdit.AddListener(OnRealTimeMultiplierChanged);
			YearInDaysInputField?.onEndEdit.AddListener(OnYearInDaysInputChanged);
			MonthInDaysInputField?.onEndEdit.AddListener(OnMonthInDaysInputChanged);
			DayInHoursInputField?.onEndEdit.AddListener(OnDayInHoursChanged);
			HourInSecondsInputField?.onEndEdit.AddListener(OnHourInSecondsChanged);
			SoundSlider?.onValueChanged.AddListener(OnSoundSliderValueChanged);
			SoundToggle?.onValueChanged.AddListener(OnSoundTogglePressed);
			Exit?.onClick.AddListener(ExitSettings);

			OnRealTimeMultiplierChanged(realTimeMultiplier.ToString());
			OnHourInSecondsChanged(hourInSeconds.ToString());
			OnDayInHoursChanged(dayInHours.ToString());
			OnMonthInDaysInputChanged(monthInDays.ToString());
			OnYearInDaysInputChanged(yearInDays.ToString());

			if (RealTimeMultiplierInputField != null)
				RealTimeMultiplierInputField.text = realTimeMultiplier.ToString();
			if (YearInDaysInputField != null)
				YearInDaysInputField.text = yearInDays.ToString();
			if (MonthInDaysInputField != null)
				MonthInDaysInputField.text = monthInDays.ToString();
			if (DayInHoursInputField != null)
				DayInHoursInputField.text = dayInHours.ToString();
			if (HourInSecondsInputField != null)
				HourInSecondsInputField.text = hourInSeconds.ToString();
		}

		private void OnDestroy()
		{
			RealTimeMultiplierInputField?.onEndEdit.RemoveListener(OnRealTimeMultiplierChanged);
			YearInDaysInputField?.onEndEdit.RemoveListener(OnYearInDaysInputChanged);
			MonthInDaysInputField?.onEndEdit.RemoveListener(OnMonthInDaysInputChanged);
			DayInHoursInputField?.onEndEdit.RemoveListener(OnDayInHoursChanged);
			HourInSecondsInputField?.onEndEdit.RemoveListener(OnHourInSecondsChanged);
			SoundSlider?.onValueChanged.RemoveListener(OnSoundSliderValueChanged);
			SoundToggle?.onValueChanged.RemoveListener(OnSoundTogglePressed);
			Exit?.onClick.RemoveListener(ExitSettings);
		}

		private void GetValuesFromConstants()
		{
			realTimeMultiplier = Constants.REAL_TIME_MULTIPLIER;
			yearInDays = Constants.DAYS_IN_A_YEAR;
			monthInDays = Constants.DAYS_IN_A_MONTH;
			dayInHours = Constants.HOURS_IN_A_DAY;
			hourInSeconds = Constants.HOUR_IN_SECONDS;
		}

		private void OnRealTimeMultiplierChanged(string newValue)
		{
			if (int.TryParse(newValue, out var result))
			{
				result = Mathf.Clamp(result, 1, 1000);
				realTimeMultiplier = result;
				Constants.REAL_TIME_MULTIPLIER = result;
				RealTimeMultiplierInputField.text = result.ToString();
			}
		}

		private void OnYearInDaysInputChanged(string newValue)
		{
			if (int.TryParse(newValue, out var result))
			{
				result = Mathf.Clamp(result, 36, 3650);
				yearInDays = result;
				Constants.DAYS_IN_A_YEAR = result;
				YearInDaysInputField.text = result.ToString();
				OnMonthInDaysInputChanged(monthInDays.ToString());
			}
		}

		private void OnMonthInDaysInputChanged(string newValue)
		{
			if (int.TryParse(newValue, out var result))
			{
				result = Mathf.Clamp(result, Mathf.Min(10, yearInDays), yearInDays);
				monthInDays = result;
				Constants.DAYS_IN_A_MONTH = result;
				MonthInDaysInputField.text = result.ToString();
			}
		}

		private void OnDayInHoursChanged(string newValue)
		{
			if (int.TryParse(newValue, out var result))
			{
				result = Mathf.Clamp(result, 4, 1000);
				dayInHours = result;
				Constants.HOURS_IN_A_DAY = result;
				DayInHoursInputField.text = result.ToString();
			}
		}

		private void OnHourInSecondsChanged(string newValue)
		{
			if (int.TryParse(newValue, out var result))
			{
				result = Mathf.Clamp(result, 200, 36000);
				hourInSeconds = result;
				Constants.HOUR_IN_SECONDS = result;
				HourInSecondsInputField.text = result.ToString();
			}
		}

		private void OnSoundSliderValueChanged(float newValue)
		{
			if (SoundToggle != null)
				SoundToggle.isOn = newValue == 0;
			if (Game.Instance.GameSound != null)
				Game.Instance.GameSound.volume = newValue;
		}

		private void OnSoundTogglePressed(bool newValue)
		{
			if (!newValue)
			{
				if (SoundSlider != null)
					SoundSlider.value = 0.01f;
			}
			else
			{
				if (SoundSlider != null)
					SoundSlider.value = 0;
			}
		}

		private void ExitSettings()
		{
			Visible(false);
		}

		public void Visible(bool visible)
		{
			gameObject.SetActive(visible);
		}
	}
}
