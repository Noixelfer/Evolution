using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Evolution.UI
{
	public class UITrait : MonoBehaviour
	{
		private TextMeshProUGUI traitName;
		private Image fillImage;

		private void Awake()
		{
			traitName = GetComponentInChildren<TextMeshProUGUI>();
			fillImage = transform.Find("PercentageBar").GetComponent<Image>();

		}
		public void SetName(string name)
		{
			if (traitName != null)
				traitName.text = name;
		}

		public void SetPercentage(float percentage)
		{
			percentage = Mathf.Clamp(percentage, 0, 1);
			fillImage.fillAmount = percentage;
		}
	}
}
