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
		CmdShoot(Vector2.zero);
	}

	[Command]
	protected void CmdShoot (Vector2 target)
	{
		RpcPlayMissileSound();
		ColorMissile missile = Instantiate(missilePrefab, gunEnd);
		missile.color = this.color;
		missile.transform.localPosition = Vector3.zero;
		missile.transform.localRotation = missilePrefab.transform.localRotation;
		missile.transform.parent = null;
		missile.Shoot(target);
	}

	[ClientRpc]
	void RpcPlayMissileSound()
	{
		audioSource.PlayOneShotControlled(missileSound, AudioType.Sound);
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
			CmdShoot(target);
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
