using TMPro;
using UnityEngine;

namespace Evolution.Character
{
	public class UIAgentStatus : MonoBehaviour
	{
		private TextMeshPro agentStatus;
		// Start is called before the first frame update
		private void Start()
		{
			agentStatus = GetComponent<TextMeshPro>();
		}

		public void SetStatus(string status)
		{
			if (agentStatus != null)
				agentStatus.text = status;
		}
	}
}
