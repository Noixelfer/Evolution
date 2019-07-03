using UnityEngine;

namespace Evolution.UI
{
	public class ViewManager : MonoBehaviour
	{
		private GameObject Canvas;
		private UIAgentViewer UIAgentViewer;

		private void Awake()
		{
			Canvas = GameObject.Find("Canvas");
			UIAgentViewer = Canvas.GetComponentInChildren<UIAgentViewer>(true);
		}

		private void Start()
		{
			if (UIAgentViewer != null)
			{
				Game.Instance.SelectionManager.OnAgentSelected += UIAgentViewer.ShowAgent;
			}
		}

		private void OnDestroy()
		{
			if (UIAgentViewer != null)
			{
				Game.Instance.SelectionManager.OnAgentSelected -= UIAgentViewer.ShowAgent;
			}
		}

		private void Update()
		{
			HandleInput();
		}

		private void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				UIAgentViewer.gameObject.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.I))
				UIAgentViewer.gameObject.SetActive(true);
		}
	}
}
