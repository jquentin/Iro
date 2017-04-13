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

	// A specific audio source so we can play with the pitch without affecting other sfx
	AudioSource _gunAudioSource;
	protected AudioSource gunAudioSource
	{
		get
		{
			if (_gunAudioSource == null)
				_gunAudioSource = this.gameObject.AddComponent<AudioSource>();
			return _gunAudioSource;
		}
	}

	protected void Shoot (float angleShift)
	{
		if (!isServer)
			return;
		PlayShootSoundOnClients();
		ColorBullet bullet = Instantiate(currentBulletPrefab, gunEnd);
		Debug.Log(bullet.name);
		bullet.color = this.color;
		bullet.owner = this.owner;
		bullet.transform.localPosition = Vector3.zero;
		bullet.transform.localRotation = bulletPrefab.transform.localRotation;
		bullet.transform.parent = null;
		bullet.Shoot(controller.angle + angleShift, bulletSpeed);
		SpawnIfOnline(bullet.gameObject);
		ShootingEffects();
	}

	[ClientRpc]
	void RpcShootingEffects()
	{
		OfflineShootingEffects();
	}
	void OfflineShootingEffects()
	{
		GetComponentInChildren<CharacterBodyController>().Shoot();
	}
	void ShootingEffects()
	{
		ModeDependantCall(RpcShootingEffects, OfflineShootingEffects);
	}

	protected abstract void PlayShootSoundOnClients();


}
