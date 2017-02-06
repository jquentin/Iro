using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

	public Transform body;

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
