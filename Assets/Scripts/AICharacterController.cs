using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterController : PlayerController {

	void Update()
	{
		rigidbody.velocity = Vector2.zero;
		rigidbody.angularVelocity = 0f;
	}

}
