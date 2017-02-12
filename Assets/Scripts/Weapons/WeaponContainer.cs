using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WeaponContainer : TigglyNetworkBehaviour {

	public List<Weapon> weaponComponents;

	[SyncVar(hook="OnChangeWeapon")]
	public int currentWeaponIndex;
	Weapon currentWeapon;

	void Start () 
	{
		OnChangeWeapon(currentWeaponIndex);
	}

	[Command]
	protected void CmdSetWeapon(int index)
	{
		Debug.Log("CmdSetWeapon");
//		for (int i = 0 ; i < weaponComponents.Count ; i++)
//		{
//			weaponComponents[i].enabled = (index == i);
//		}
		currentWeaponIndex = index;
	}

	void OnChangeWeapon(int weaponIndex)
	{
		Debug.Log("OnChangeWeapon");
		for (int i = 0 ; i < weaponComponents.Count ; i++)
		{
			weaponComponents[i].enabled = (weaponIndex == i);
		}
	}
}
