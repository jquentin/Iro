using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSimpleGun : WeaponGun 
{
	
	public ColorBullet largeBulletPrefab;

	[NonSerialized]
	public bool isLargeMode = false;


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
