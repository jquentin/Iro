using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMissileLauncher : Weapon {

	public List<AudioClip> missileSound;
	public List<AudioClip> missileNotReadySound;

	public float timeToReload = 1f;

	public Transform gunEnd;

	public ColorMissile missilePrefab;

	bool isReadyToShoot = true;

	[NonSerialized]
	public bool isIllimitedMode = false;

	public override bool isUpgraded {
		get {
			return isIllimitedMode;
		}
	}

	protected void CmdShoot ()
	{
		Shoot(Vector2.zero);
	}

	[Command]
	protected void CmdShoot (Vector2 target)
	{
		OfflineShoot(target);
	}
	protected void OfflineShoot (Vector2 target)
	{
		PlayMissileSound();
		ColorMissile missile = Instantiate(missilePrefab, gunEnd);
		missile.color = this.color;
		missile.transform.localPosition = Vector3.zero;
		missile.transform.localRotation = missilePrefab.transform.localRotation;
		missile.transform.parent = null;
		missile.Shoot(target);
	}
	protected void Shoot (Vector2 target)
	{
		ModeDependantCall(CmdShoot, OfflineShoot, target);
	}

	[ClientRpc]
	void RpcPlayMissileSound()
	{
		OfflinePlayMissileSound();
	}
	void OfflinePlayMissileSound()
	{
		audioSource.PlayOneShotControlled(missileSound, AudioType.Sound);
	}
	void PlayMissileSound()
	{
		ModeDependantCall(RpcPlayMissileSound, OfflinePlayMissileSound);
	}

	protected virtual void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			if (!isReadyToShoot && !isIllimitedMode)
			{
				audioSource.PlayOneShotControlled(missileNotReadySound, AudioType.Sound);
				return;
			}
			Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Shoot(target);
			isReadyToShoot = false;
			CancelInvoke("Reload");
			Invoke("Reload", timeToReload);
		}
	}

	void Reload()
	{
		isReadyToShoot = true;
	}
}
