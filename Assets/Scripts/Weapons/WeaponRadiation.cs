using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponRadiation : Weapon {

	public List<AudioClip> radiationSound;
	public List<AudioClip> touchSound;

	public Radiation radiationObject;

	[SyncVar(hook="OnActiveStateChanged")]
	public bool isActivated = false;

	[NonSerialized]
	public bool isIllimitedMode = false;


	public float timeToReload = 0.1f;

	bool isReadyToShoot = true;

	[Command]
	protected void CmdStartRadiate ()
	{
		isActivated = true;
	}

	[Command]
	protected void CmdStopRadiate ()
	{
		isActivated = false;
	}

	void OnActiveStateChanged(bool activated)
	{
		this.isActivated = activated;
		radiationObject.gameObject.SetActive(activated);
	}

	public void Touched(Collider2D other)
	{
		if (!isServer)
			return;
		if (!isActivated)
			return;
		if (!isReadyToShoot)
			return;
		Shootable shootable = other.GetComponentInParent<Shootable>();
		ColorBeing being = shootable as ColorBeing;
		if (shootable != null && being != null && !being.isDead)
		{
			shootable.BeShot(owner.color, 1f, other.transform.position - transform.position, 10f);
//			Explosion.CreateExplosion(explosionPrefab, transform.position, transform.localScale.x, color, false);
			StartCoroutine(InvalidateShootAtEOF());
		}
	}

	// We wait until end of frame so other players can be damaged in the same frame
	IEnumerator InvalidateShootAtEOF()
	{
		yield return new WaitForEndOfFrame();
		isReadyToShoot = false;
		CancelInvoke("Reload");
		Invoke("Reload", timeToReload);
	}

	void Reload()
	{
		isReadyToShoot = true;
	}

	void Awake()
	{
		OnActiveStateChanged(false);
	}

	protected virtual void Update()
	{
		if (!isLocalPlayer || owner.isDead)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			CmdStartRadiate();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			CmdStopRadiate();
		}
	}

}
