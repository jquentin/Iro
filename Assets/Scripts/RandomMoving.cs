using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving : TigglyNetworkBehaviour {

	public float timeBetweenPushes = 0.2f;

	public float force = 1f;
	public float torque = 1f;

	public ForceMode2D forceMode;

	float lastPush;

	Rigidbody2D _rigidbody2D;
	protected Rigidbody2D rigidbody2D
	{
		get
		{
			if (_rigidbody2D == null)
				_rigidbody2D = GetComponent<Rigidbody2D>();
			return _rigidbody2D;
		}
	}

	void Start()
	{
		lastPush = Time.time;
	}

	void Update () 
	{
		if (!isServer)
			return;
		if (Time.time >= lastPush + timeBetweenPushes)
		{
			rigidbody2D.AddForce(new Vector2(Random.Range(-force, force), Random.Range(-force, force)), forceMode);
			rigidbody2D.AddTorque(Random.Range(-torque, torque), forceMode);
			lastPush = Time.time;
		}
	}
}
