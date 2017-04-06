using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WeaponGun : Weapon {

	public float bulletSpeed = 1f;

	public Transform gunEnd;

	public List<AudioClip> shootSound;

	public ColorBullet bulletPrefab;

	protected abstract ColorBullet currentBulletPrefab
	{
		get;
	}

	protected void Shoot (float angleShift)
	{
		if (!isServer)
			return;
		RpcPlayShootSound();
		ColorBullet bullet = Instantiate(currentBulletPrefab, gunEnd);
		Debug.Log(bullet.name);
		bullet.color = this.color;
		bullet.owner = this.owner;
		bullet.transform.localPosition = Vector3.zero;
		bullet.transform.localRotation = bulletPrefab.transform.localRotation;
		bullet.transform.parent = null;
		bullet.Shoot(controller.angle + angleShift, bulletSpeed);
		NetworkServer.Spawn(bullet.gameObject);
	}

	[ClientRpc]
	void RpcPlayShootSound()
	{
		audioSource.PlayOneShotControlled(shootSound, AudioType.Sound);
	}


}
