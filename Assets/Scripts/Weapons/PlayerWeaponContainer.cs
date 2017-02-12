using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponContainer : WeaponContainer {

	readonly List<KeyCode> keys = new List<KeyCode>(){KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};

	void Update () 
	{
		if (!isLocalPlayer)
			return;
		for (int i = 0 ; i < keys.Count ; i++)
		{
			if (Input.GetKeyDown(keys[i]))
				CmdSetWeapon(i);
		}
	}

}
