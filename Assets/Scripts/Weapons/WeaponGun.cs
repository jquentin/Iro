using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponGun : Weapon {

	public float bulletSpeed = 1f;

	public Transform gunEnd;

	public List<AudioClip> shootSound;

	public ColorBullet bulletPrefab;

	protected override void CmdShoot ()
	{
		CmdShoot(0f);
	}

	[Command]
	protected void CmdShoot (float angleShift)
	{
		RpcPlayShootSound();
		ColorBullet bullet = Instantiate(bulletPrefab, gunEnd);
		bullet.color = this.color;
		bullet.transform.localPosition = Vector3.zero;
		bullet.transform.localRotation = bulletPrefab.transform.localRotation;
		bullet.transform.parent = null;
		IroCharacterController cc = GetComponentInParent<IroCharacterController>();
		bullet.Shoot(cc.angle + angleShift, bulletSpeed);
		NetworkServer.Spawn(bullet.gameObject);
	}

	[ClientRpc]
	void RpcPlayShootSound()
	{
		audioSource.PlayOneShotControlled(shootSound, AudioType.Sound);
	}

	protected virtual void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			CmdShoot();
		}
	}

}
