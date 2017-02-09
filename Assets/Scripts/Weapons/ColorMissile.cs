using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMissile : ColorObject {

	public float force = 1f;

	public float timeToTarget = 1f;
	public float explodeRadius = 1f;

	Vector3 initScale;

	public Explosion explosionPrefab;

	void Awake()
	{
		initScale = transform.localScale;
	}

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

	public void Shoot(Vector2 target)
	{
		Vector2 pos = transform.position;
		float speed = (target - pos).magnitude / timeToTarget;
		GetComponent<Rigidbody2D>().velocity = speed * (target - pos).normalized;
		Invoke("Explode", timeToTarget);
		GoUp();
	}

	void GoUp()
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", 2.5f * initScale,
			"time", timeToTarget * 0.5f,
			"easetype", iTween.EaseType.easeOutSine,
			"oncomplete", "GoDown"));
	}

	void GoDown()
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", initScale,
			"time", timeToTarget * 0.5f,
			"easetype", iTween.EaseType.easeInSine));
	}

	void Explode()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
		Explosion.CreateExplosion(explosionPrefab, transform.position, 4f, color);
		foreach(Collider2D collider in colliders)
		{
			Shootable shootable = collider.GetComponentInParent<Shootable>();
			if (shootable != null)
			{
				shootable.BeShot(color, force);
			}
		}
		Destroy(gameObject);
	}
}
