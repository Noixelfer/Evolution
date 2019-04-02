using UnityEngine;

public interface ITile
{
	bool Walkable { get; set; }
	//bool 
	Vector3 GetRandomPosition();
}
