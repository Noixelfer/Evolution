using UnityEngine;

namespace Evolution
{
	public class InputManager
	{
		public InputManager()
		{

		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
				Game.Instance.SelectionManager.SelectNextAgent();
		}
	}
}