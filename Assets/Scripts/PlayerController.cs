using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public float moveSpeed = 1f;

	public Transform body;

	protected bool canMove = true;

	const float RECOIL_TIME = 0.04f;

	void Awake()
	{
		owner.OnDead += OnPlayerDead;
	}

	Rigidbody2D _rigidbody;
	protected Rigidbody2D rigidbody
	{
		get
		{
			if (_rigidbody == null)
				_rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}

	ColorBeing _owner;
	protected ColorBeing owner
	{
		get
		{
			if (_owner == null)
				_owner = GetComponent<ColorBeing>();
			return _owner;
		}
	}

	public float angle
	{
		get
		{
			return body.eulerAngles.z - 90f;
		}
		set
		{
			body.eulerAngles = Vector3.back * (-value - 90f);
		}
	}

	public Vector2 velocity
	{
		get
		{
			return rigidbody.velocity;
		}
		protected set
		{
			rigidbody.velocity = value;
		}
	}

	public void GetRecoil()
	{
		canMove = false;
		Invoke("FinishRecoil", RECOIL_TIME);
	}

	void FinishRecoil()
	{
		canMove = true;
	}

	void OnPlayerDead()
	{
		rigidbody.velocity = Vector2.zero;
	}

	public static float SignedAngle(Vector2 vector)
	{
		return Vector2.Angle(Vector2.right, vector) * ((vector.y >= 0f) ? 1f : -1f);
	}

}
