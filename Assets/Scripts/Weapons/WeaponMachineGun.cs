using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMachineGun : WeaponGun {

	public float timeBetweenShots = 0.2f;

	float lastShotTime = float.MinValue;

	protected override void Update()
	{
		if (Input.GetMouseButton(0))
		{
			if (Time.time - lastShotTime > timeBetweenShots)
			{
				Shoot();
				lastShotTime = Time.time;
			}
		}
	}

}
