using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombDropper : Weapon {

	float timeLastDrop;

	public Bomb bombPrefab;
	bool isReadyToShoot = true;

	protected override void CmdShoot ()
	{
		throw new System.NotImplementedException ();
	}

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
			if (!isReadyToShoot)
			{
//				audioSource.PlayOneShotControlled(missileNotReadySound, AudioType.Sound);
				return;
			}
			CmdDropBomb();
			isReadyToShoot = false;
			Invoke("Reload", bombPrefab.timeBeforeExplosion);
		}
	}

	void Reload()
	{
		isReadyToShoot = true;
	}
}
