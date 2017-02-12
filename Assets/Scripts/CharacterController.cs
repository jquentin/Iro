using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour {

	public float moveSpeed = 1f;

	public Transform body;

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

	public float angle
	{
		get
		{
			return body.eulerAngles.z - 90f;
		}
		protected set
		{
			body.eulerAngles = Vector3.back * (-value - 90f);
		}
	}

}
