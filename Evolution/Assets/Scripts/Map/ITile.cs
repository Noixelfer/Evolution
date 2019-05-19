using UnityEngine;

public interface ITile
{
	bool Walkable { get; set; }
	bool Occupied { get; set; } 
	Vector2 MapPosition { get; set; }

	Vector3 GetRandomPosition();
}
