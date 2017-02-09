using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMissileLauncher : Weapon {

	public List<AudioClip> missileSound;
	public List<AudioClip> missileNotReadySound;

	public float timeToReload = 1f;

	public Transform gunEnd;

	public ColorMissile missilePrefab;

	bool isReadyToShoot = true;

	protected override void Shoot ()
	{
		Shoot(Vector2.zero);
	}

	protected void Shoot (Vector2 target)
	{
		audioSource.PlayOneShotControlled(missileSound, AudioType.Sound);
		ColorMissile missile = Instantiate(missilePrefab, gunEnd);
		missile.color = this.color;
		missile.transform.localPosition = Vector3.zero;
		missile.transform.localRotation = missilePrefab.transform.localRotation;
		missile.transform.parent = null;
		missile.Shoot(target);
	}

	protected virtual void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!isReadyToShoot)
			{
				audioSource.PlayOneShotControlled(missileNotReadySound, AudioType.Sound);
				return;
			}
			Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Shoot(target);
			isReadyToShoot = false;
			Invoke("Reload", timeToReload);
		}
	}

	void Reload()
	{
		isReadyToShoot = true;
	}
}
