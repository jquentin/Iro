using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ColorObject {

	public float force = 1f;

	public float timeBeforeExplosion = 2f;
	public float explodeRadius = 1f;

	public AudioClip fuseSnd;

	public Explosion explosionPrefab;

	float timeDrop;

	void Start () 
	{
		timeDrop = Time.time;
		audioSource.loop = true;
		audioSource.clip = fuseSnd;
		audioSource.PlayControlled(AudioType.Sound);
	}

	void Update () 
	{
		if (!isServer)
			return;
		if (Time.time >= timeDrop + timeBeforeExplosion)
		{
			Explode();
		}
	}


	void Explode()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);
		Explosion.CreateExplosion(explosionPrefab, transform.position, explodeRadius, color);
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
