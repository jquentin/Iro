using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMachineGun : WeaponGun {

	public float timeBetweenShots = 0.2f;

	public float angleVariation = 2f;

	float lastShotTime = float.MinValue;

	[SyncVar(hook="OnStraightModeChanged")]
	public bool _isStraightMode = false;
	public bool isStraightMode { get { return _isStraightMode; } set { _isStraightMode = value; if (offlineMode) OnStraightModeChanged(value); } }

	public override bool isUpgraded {
		get {
			return isStraightMode;
		}
	}

	[Command]
	void CmdSetStraightMode(bool enable)
	{
		OfflineSetStraightMode(enable);
	}
	void OfflineSetStraightMode(bool enable)
	{
		isStraightMode = enable;
	} 
	public void SetStraightMode(bool enable)
	{
//		ModeDependantCall(CmdSetStraightMode, OfflineSetStraightMode, enable);
		if (offlineMode)
			OfflineSetStraightMode(enable);
		else
			CmdSetStraightMode(enable);
	}

	void OnStraightModeChanged(bool largeModeValue)
	{
		_isStraightMode = largeModeValue;
		WeaponButtonsContainer.instance.UpdateButtonsMode(owner);
	}

	protected override ColorBullet currentBulletPrefab 
	{
		get 
		{
			return bulletPrefab;
		}
	}

	[Command]
	void CmdShoot ()
	{
		OfflineShoot();
	}
	void OfflineShoot ()
	{
		float shotAngle = isStraightMode ? 0f : UnityEngine.Random.Range(-angleVariation, angleVariation);
		Shoot (shotAngle);
	}
	protected void Shoot ()
	{
//		ModeDependantCall(CmdShoot, OfflineShoot);
		if (offlineMode)
			OfflineShoot();
		else
			CmdShoot();
	}


	public void TryShoot()
	{
		if (Time.time - lastShotTime > timeBetweenShots)
		{
			Shoot();
			lastShotTime = Time.time;
		}
	}

	void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButton(0))
		{
			TryShoot();
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
		gunAudioSource.pitch = 1.3f;
		gunAudioSource.PlayOneShotControlled(shootSound, AudioType.Sound);
	}
	protected void PlayShootSound ()
	{
//		ModeDependantCall(RpcPlayShootSound, OfflinePlayShootSound);
		if (offlineMode)
			OfflinePlayShootSound();
		else
			RpcPlayShootSound();
	}

}
