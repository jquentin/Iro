using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSimpleGun : WeaponGun 
{
	
	public ColorBullet largeBulletPrefab;

	[SyncVar(hook="OnLargeModeChanged")]
	public bool isLargeMode = false;

	[Command]
	public void CmdSetLargeMode(bool enable)
	{
		isLargeMode = enable;
	} 

	void OnLargeModeChanged(bool largeModeValue)
	{
		isLargeMode = largeModeValue;
		WeaponButtonsContainer.instance.UpdateButtonsMode(owner);
	}

	protected override ColorBullet currentBulletPrefab 
	{
		get 
		{
			if (isLargeMode)
				return largeBulletPrefab;
			else
				return bulletPrefab;
		}
	}

	[Command]
	protected void CmdShoot ()
	{
		Shoot(0f);
	}

	void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			CmdShoot();
		}
	}
}
