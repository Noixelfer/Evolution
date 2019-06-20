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
		ClampPosition();
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

	private void ClampPosition()
	{
		var ortographicSize = Camera.orthographicSize;
		var height = 2f * ortographicSize;
		float width = height * Camera.aspect;

		//left corner
		float newX = transform.position.x;
		float newY = transform.position.y;
		if (transform.position.y < height / 2.0f - 0.5f)
			newY = height / 2.0f - 0.5f;

		if (transform.position.y > Game.Instance.MAP_SIZE - height / 2.0f - 0.5f)
			newY = Game.Instance.MAP_SIZE - height / 2.0f - 0.5f;

		if (transform.position.x < width / 2.0f - 0.5f)
			newX = width / 2.0f - 0.5f;

		if (transform.position.x > Game.Instance.MAP_SIZE -  width / 2.0f - 0.5f)
			newX = Game.Instance.MAP_SIZE - width / 2.0f - 0.5f;

		transform.position = new Vector3(newX, newY, transform.position.z);
	}
}
