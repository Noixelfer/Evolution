using Evolution;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float ZoomSensitivity = 3f;
	public float MoveSensitivity = 10f;

	private Camera Camera;
	private readonly float MIN_DISTANCE = 1f;
	private readonly float MAX_DISTANCE = 20f;
	private readonly float FAST_MODE_MULTIPLIER = 4f;

	private void Start()
	{
		Camera = GetComponent<Camera>();
	}

	private void Update()
	{
		if (Camera == null)
		{
			Debug.LogError("There is no camera component on this object!!!");
			return;
		}

		ZoomInOut();
		MoveCamera();
	}

	private void ZoomInOut()
	{
		var zoomAmount = -Input.GetAxis(Constants.MOUSE_SCROLLWHEEL_AXIS) * ZoomSensitivity;
		zoomAmount *= 0.1f + (Camera.orthographicSize / MAX_DISTANCE) * 0.9f;
		Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize + zoomAmount * FastMode(), MIN_DISTANCE, MAX_DISTANCE);
	}

	private void MoveCamera()
	{
		var ortographicSize = Camera.orthographicSize;
		var horizontalMovement = Input.GetAxis(Constants.HORIZONTAL_AXIS);
		var verticalMovement = Input.GetAxis(Constants.VERTICAL_AXIS);
		transform.position += new Vector3(horizontalMovement * MoveSensitivity * ortographicSize / MAX_DISTANCE * FastMode(), verticalMovement * MoveSensitivity * ortographicSize / MAX_DISTANCE * FastMode());
	}

	private float FastMode()
	{
		if (Input.GetKey(KeyCode.LeftShift))
			return FAST_MODE_MULTIPLIER;
		return 1;
	}
}
