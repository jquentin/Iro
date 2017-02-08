using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {

	public float rotatingSpeed = 1f;

	void Update () 
	{
		transform.Rotate(Vector3.back, Time.deltaTime * rotatingSpeed);
		
	}
}
