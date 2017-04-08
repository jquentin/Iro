﻿using System;
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

	public override bool isUpgraded {
		get {
			return isLargeMode;
		}
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
	public void CmdShoot ()
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

	protected override void PlayShootSoundOnClients ()
	{
		RpcPlayShootSound();
	}

	[ClientRpc]
	protected void RpcPlayShootSound ()
	{
		audioSource.PlayOneShotControlled(shootSound, AudioType.Sound);
	}
}
