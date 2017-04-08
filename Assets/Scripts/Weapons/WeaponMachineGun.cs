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
	public bool isStraightMode = false;

	public override bool isUpgraded {
		get {
			return isStraightMode;
		}
	}

	[Command]
	public void CmdSetStraightMode(bool enable)
	{
		isStraightMode = enable;
	} 

	void OnStraightModeChanged(bool largeModeValue)
	{
		isStraightMode = largeModeValue;
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
	protected void CmdShoot ()
	{
		float shotAngle = isStraightMode ? 0f : UnityEngine.Random.Range(-angleVariation, angleVariation);
		Shoot (shotAngle);
	}

	public void TryShoot()
	{
		if (Time.time - lastShotTime > timeBetweenShots)
		{
			CmdShoot();
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

}
