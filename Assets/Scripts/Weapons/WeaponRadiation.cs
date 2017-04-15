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
	public bool _isActivated = false;
	public bool isActivated { get { return _isActivated; } set { _isActivated = value; if (offlineMode) OnActiveStateChanged(value); } }

	[NonSerialized]
	public bool isIllimitedMode = false;

	public override bool isUpgraded {
		get {
			return isIllimitedMode;
		}
	}

	public float timeToReload = 0.1f;

	bool isReadyToShoot = true;

	[Command]
	protected void CmdStartRadiate ()
	{
		OfflineStartRadiate();
	}
	protected void OfflineStartRadiate ()
	{
		isActivated = true;
	}
	protected void StartRadiate ()
	{
//		ModeDependantCall(CmdStartRadiate, OfflineStartRadiate);
		if (offlineMode)
			OfflineStartRadiate();
		else
			CmdStartRadiate();
	}

	[Command]
	protected void CmdStopRadiate ()
	{
		OfflineStopRadiate();
	}
	protected void OfflineStopRadiate ()
	{
		isActivated = false;
	}
	protected void StopRadiate ()
	{
//		ModeDependantCall(CmdStopRadiate, OfflineStopRadiate);

		if (offlineMode)
			OfflineStopRadiate();
		else
			CmdStopRadiate();
	}

	void OnActiveStateChanged(bool activated)
	{
		this._isActivated = activated;
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
			audioSource.PlayOneShotControlled(touchSound, AudioType.Sound);
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
			StartRadiate();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			StopRadiate();
		}
	}

}
