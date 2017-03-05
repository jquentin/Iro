using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMachineGun : WeaponGun {

	public float timeBetweenShots = 0.2f;

	public float angleVariation = 2f;

	float lastShotTime = float.MinValue;

	[NonSerialized]
	public bool isStraightMode = false;

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

	void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButton(0))
		{
			if (Time.time - lastShotTime > timeBetweenShots)
			{
				CmdShoot();
				lastShotTime = Time.time;
			}
		}
	}

}
