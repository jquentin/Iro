using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

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

	float _angle;
	public float angle
	{
		get
		{
			return _angle;
		}
		protected set
		{
			_angle = value;
			body.eulerAngles = Vector3.back * (-angle - 90f);
		}
	}

}
