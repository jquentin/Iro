using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButton(0))
		{
			if (Time.time - lastShotTime > timeBetweenShots)
			{
				float shotAngle = isStraightMode ? 0f : UnityEngine.Random.Range(-angleVariation, angleVariation);
				CmdShoot(shotAngle);
				lastShotTime = Time.time;
			}
		}
	}

}
