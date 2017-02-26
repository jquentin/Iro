using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public Transform foreground;

	public void UpdateBar (float health) 
	{
		foreground.localScale = new Vector3(1f, health, 1f);
	}
}
