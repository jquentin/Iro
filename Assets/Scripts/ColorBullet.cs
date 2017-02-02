using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBullet : ColorObject {

	public float force = 1f;

	void OnTriggerEnter2D (Collider2D other) 
	{
		ColorBeing being = other.GetComponentInParent<ColorBeing>();
		if (being != null && !being.isDead)
		{
			being.BeShot(color, force);
			Destroy(gameObject);
		}
	}

}
