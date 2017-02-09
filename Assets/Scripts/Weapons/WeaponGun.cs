using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : Weapon {

	public float bulletSpeed = 1f;

	public Transform gunEnd;

	public ColorBullet bulletPrefab;

	protected override void Shoot ()
	{
		Shoot(0f);
	}

	protected void Shoot (float angleShift)
	{
		ColorBullet bullet = Instantiate(bulletPrefab, gunEnd);
		bullet.color = this.color;
		bullet.transform.localPosition = Vector3.zero;
		bullet.transform.localRotation = bulletPrefab.transform.localRotation;
		bullet.transform.parent = null;
		bullet.Shoot(GetComponentInParent<IroCharacterController>().angle + angleShift, bulletSpeed);
	}

	protected virtual void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

}
