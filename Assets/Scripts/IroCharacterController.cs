using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IroCharacterController : MonoBehaviour {

	public Transform body;

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
		body.LookAt(worldMousePos, Vector3.back);
	}
}
