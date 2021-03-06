﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AICharacterController : PlayerController {

//	Vector3 velocity = Vector3.zero;

	void Awake()
	{
		GetComponent<BehaviorDesigner.Runtime.BehaviorTree>().StartWhenEnabled = true;
//		GetComponent<NetworkIdentity>().serverOnly = true;
	}

	static int indexAI = 1;

	void Start()
	{
		if (isServer)
		{
			SetPlayerName(string.Format("AI-{0}", indexAI));
			indexAI++;
		}
	}

	void Update()
	{
//		velocity = Vector3.zero;

//		if (transform.position.x > 10f)
//			velocity = 1f * Vector3.left;
//		else if (transform.position.x < -10f)
//			velocity = 1f * Vector3.right;
//		
//		rigidbody.velocity = velocity;
//		rigidbody.angularVelocity = 0f;
	}

}
