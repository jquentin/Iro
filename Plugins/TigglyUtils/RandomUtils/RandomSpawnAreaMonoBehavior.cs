using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class implementing the RandomSpawnArea interface, that is also a MonoBehavior.
/// Using this class to declare a variable in a script will allow Unity Editor to
/// expose the variable, leaving you free to select an instance of any sub-class.
/// Example:
/// public class Spawner : MonoBehavior {
/// 	public RandomSpawnAreaMonoBehavior spawnArea;
/// }
/// The editor will let you drag in any object inheriting RandomSpawnAreaMonoBehavior,
/// no matter what technical implementation this object uses.
/// </summary>
public abstract class RandomSpawnAreaMonoBehavior : MonoBehaviour, RandomSpawnArea 
{ 

	public abstract Vector3 PickRandomPosition(bool local);

}

