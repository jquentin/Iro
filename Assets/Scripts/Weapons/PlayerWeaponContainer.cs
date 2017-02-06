using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponContainer : WeaponContainer {

	void Start () 
	{
		SetWeapon(0);
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			SetWeapon(0);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			SetWeapon(1);
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			SetWeapon(2);
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			SetWeapon(3);
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			SetWeapon(4);
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			SetWeapon(5);
	}

}
