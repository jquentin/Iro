using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterController : CharacterController {

	void Update()
	{
		rigidbody.velocity = Vector2.zero;
		rigidbody.angularVelocity = 0f;
	}

}
