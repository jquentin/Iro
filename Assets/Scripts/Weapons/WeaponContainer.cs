using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WeaponContainer : TigglyNetworkBehaviour {

	public List<Weapon> weaponComponents;

	[SyncVar(hook="OnChangeWeapon")]
	int _currentWeaponIndex;
	public int currentWeaponIndex { get { return _currentWeaponIndex; } set { _currentWeaponIndex = value; if (offlineMode) OnChangeWeapon(value); } }
	Weapon currentWeapon;

	void Start () 
	{
		OnChangeWeapon(currentWeaponIndex);
		WeaponButtonsContainer.instance.UpdateButtonsMode(GetComponent<ColorBeing>());
	}

	[Command]
	void CmdSetWeapon(int index)
	{
		OfflineSetWeapon(index);
	}
	void OfflineSetWeapon(int index)
	{
		Debug.Log("CmdSetWeapon");
//		for (int i = 0 ; i < weaponComponents.Count ; i++)
//		{
//			weaponComponents[i].enabled = (index == i);
//		}
		currentWeaponIndex = index;
	}
	protected void SetWeapon(int index)
	{
		ModeDependantCall(CmdSetWeapon, OfflineSetWeapon, index);
	}

	void OnChangeWeapon(int weaponIndex)
	{
		Debug.Log("OnChangeWeapon");
		for (int i = 0 ; i < weaponComponents.Count ; i++)
		{
			weaponComponents[i].enabled = (weaponIndex == i);
		}
		if (isLocalPlayer)
			WeaponButtonsContainer.instance.UpdateButtons(weaponIndex);
	}
}
