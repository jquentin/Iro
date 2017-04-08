using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IroCharacterController : PlayerController {

	bool isMoving;

	void Start()
	{
		if (isLocalPlayer)
			CmdSetPlayerName(OfflineScene.playerName);
	}

	void FixedUpdate () 
	{
		if (!isLocalPlayer || owner.isDead)
			velocity = Vector2.zero;

		if (!isLocalPlayer || owner.isDead || !canMove)
			return;
		
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		velocity = new Vector2(moveSpeed * horizontal, moveSpeed * vertical);
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
}
