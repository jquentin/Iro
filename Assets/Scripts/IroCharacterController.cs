using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IroCharacterController : CharacterController {

	public float moveSpeed = 1f;

	bool isMoving;

	void Update () 
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		if (vertical != 0f || horizontal != 0f)
		{
			transform.Translate(new Vector3(moveSpeed * horizontal, moveSpeed * vertical));
			isMoving = true;
		}
		else
		{
			isMoving = false;
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
