using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterController : PlayerController {

	Vector3 velocity = Vector3.zero;

	void Update()
	{
		if (transform.position.x > 10f)
			velocity = 1f * Vector3.left;
		else if (transform.position.x < -10f)
			velocity = 1f * Vector3.right;
		
		rigidbody.velocity = velocity;
		rigidbody.angularVelocity = 0f;
	}

}
