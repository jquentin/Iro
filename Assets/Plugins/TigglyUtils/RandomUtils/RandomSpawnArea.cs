using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for classes that describe an area in which we want to 
/// pick random positions, typically to spawn objects in.
/// </summary>
public interface RandomSpawnArea 
{

	Vector3 PickRandomPosition(bool local);

}

