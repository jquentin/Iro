using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBullet : ColorObject {

	public float force = 1f;

	void OnTriggerEnter2D (Collider2D other) 
	{
		Shootable shootable = other.GetComponentInParent<Shootable>();
		ColorBeing being = shootable as ColorBeing;
		if (shootable != null && (being == null || !being.isDead))
		{
			shootable.BeShot(color, force);
			Destroy(gameObject);
		}
	}

	public void Shoot(float angle, float speed)
	{
		float radAngle = Mathf.Deg2Rad * angle;
		GetComponent<Rigidbody2D>().velocity = speed * new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
	}

}
