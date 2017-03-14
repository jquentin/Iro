using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBullet : ColorObject {

	public float force = 1f;

	public Explosion explosionPrefab;

	public ColorBeing owner;

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (!isServer)
			return;
		Shootable shootable = other.GetComponentInParent<Shootable>();
		ColorBeing being = shootable as ColorBeing;
		if (shootable != null && (being == null || !being.isDead) && being != owner)
		{
			shootable.BeShot(color, force, GetComponent<Rigidbody2D>().velocity);
			Destroy(gameObject);
			Explosion.CreateExplosion(explosionPrefab, transform.position, transform.localScale.x, color, false);
		}
	}

	public void Shoot(float angle, float speed)
	{
		float radAngle = Mathf.Deg2Rad * angle;
		GetComponent<Rigidbody2D>().velocity = speed * new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
	}

}
