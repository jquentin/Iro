using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IroCharacterController : CharacterController {

	bool isMoving;

	void FixedUpdate () 
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		rigidbody.velocity = new Vector2(moveSpeed * horizontal, moveSpeed * vertical);
		if (vertical != 0f || horizontal != 0f)
		{
//			transform.Translate(new Vector3(moveSpeed * horizontal, moveSpeed * vertical));
			isMoving = true;
		}
		else
		{
			isMoving = false;
			rigidbody.angularVelocity = 0f;
		}
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldMousePos.z = 0f;
		Vector2 dif = worldMousePos - transform.position;
		angle = SignedAngle(dif);
	}

	public static float SignedAngle(Vector2 vector)
	{
		return Vector2.Angle(Vector2.right, vector) * ((vector.y >= 0f) ? 1f : -1f);
	}
}
