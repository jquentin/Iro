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
	void CmdSetLargeMode(bool enable)
	{
		OfflineSetLargeMode(enable);
	}
	void OfflineSetLargeMode(bool enable)
	{
		isLargeMode = enable;
	} 
	public void SetLargeMode(bool enable)
	{
		ModeDependantCall(CmdSetLargeMode, OfflineSetLargeMode, enable);
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
	void CmdShoot ()
	{
		OfflineShoot ();
	}
	void OfflineShoot()
	{
		Shoot(0f);
	}
	public void Shoot ()
	{
		ModeDependantCall(CmdShoot, OfflineShoot);
	}

	void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	protected override void PlayShootSoundOnClients ()
	{
		PlayShootSound();
	}

	[ClientRpc]
	void RpcPlayShootSound ()
	{
		OfflinePlayShootSound();
	}
	void OfflinePlayShootSound ()
	{
		gunAudioSource.pitch = isLargeMode ? 0.7f : 1f;
		gunAudioSource.PlayOneShotControlled(shootSound, AudioType.Sound);
	}
	protected void PlayShootSound ()
	{
		ModeDependantCall(RpcPlayShootSound, OfflinePlayShootSound);
	}
}
