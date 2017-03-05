using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombDropper : Weapon {

	float timeLastDrop;

	public Bomb bombPrefab;

	public float timeBetweenDrops = 1f;

	[NonSerialized]
	public bool isIllimitedMode = false;

	bool isReadyToShoot = true;

	[Command]
	protected void CmdDropBomb ()
	{
		Bomb bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
		bomb.color = this.color;
		NetworkServer.Spawn(bomb.gameObject);
	}

	protected virtual void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			if (!isReadyToShoot && !isIllimitedMode)
			{
//				audioSource.PlayOneShotControlled(missileNotReadySound, AudioType.Sound);
				return;
			}
			CmdDropBomb();
			isReadyToShoot = false;
			CancelInvoke("Reload");
			Invoke("Reload", bombPrefab.timeBeforeExplosion);
		}
	}

	void Reload()
	{
		isReadyToShoot = true;
	}
}
