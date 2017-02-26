using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorBeing : ColorObject, Shootable {

	public const float MAX_HUE_DIF_TO_HEAL = 0.15f;
	/// <summary>
	/// The ratio between the max health that can be affected to heal if perfect color match
	/// and the max health that can be affected to damage if worst color match
	/// </summary>
	public const float HEAL_FACTOR = 0.2f;

	public int maxHealth = 10;
	[SyncVar(hook = "OnHealthChanged")]
	public int health;

	public HealthBar healthBar;

	public bool isDead { get { return (health <= 0); } }

	Vector3 initPos;

	HPChangeSpawner _hpChangeSpawner;
	HPChangeSpawner hpChangeSpawner
	{
		get
		{
			if (_hpChangeSpawner == null)
				_hpChangeSpawner = GetComponentInChildren<HPChangeSpawner>();
			return _hpChangeSpawner;
		}
	}

	protected override void Start()
	{
		base.Start();
		health = maxHealth;
		initPos = transform.position;
	}

	protected override void Update()
	{
		base.Update();
//		BeShot(Color.blue, 10f);
	}

	public void BeShot (Color c, float force) 
	{
		if (!isServer)
			return;
		float hue = c.GetHue();
		float hueDif = ColorUtils.GetHueDif(this.hue, hue);
//		Debug.LogFormat("hue dif = {0}", hueDif);
		float hitFactor = CalculateHitFactor(hueDif);
		int damage = CalculateDamage(hitFactor, force);
		health = Mathf.Min(health - damage, maxHealth);
		RpcSpawnHealthChange(damage, hitFactor);
	}

	void OnHealthChanged(int health)
	{
		this.health = health;
		healthBar.UpdateBar((float)health / (float) maxHealth);
		if (health <= 0)
			Die();
	}

	[ClientRpc]
	void RpcSpawnHealthChange(int damage, float hitFactor)
	{
		Debug.LogFormat("{0} shot by damage: {1}, leftHealth: {2}", name, damage, health);
		hpChangeSpawner.HealthChange(-damage, hitFactor);
	}

	void Die()
	{
		Debug.LogFormat("{0} dead", name);
		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
			sr.color = sr.color.LowerAlpha(0.6f);
		foreach(Collider2D c in GetComponentsInChildren<Collider2D>(true))
			c.enabled = false;
		Invoke("Respawn", 1f);
	}

	void Respawn()
	{
		StartCoroutine(Respawn_CR());
	}
	void ActualRespawn()
	{
		transform.position = initPos;
		if (!isLocalPlayer)
			return;
		CmdResuscitate();
	}

	[Command]
	void CmdResuscitate()
	{
		health = maxHealth;
	}

	void SetRenderersAlpha(float alpha)
	{
		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
			sr.color = sr.color.SetAlpha(alpha);
	}

	IEnumerator Respawn_CR()
	{
		SetRenderersAlpha(0f);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0.5f);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0f);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0.5f);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0f);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(1f);
		ActualRespawn();
		yield return new WaitForSeconds(0.15f);
		foreach(Collider2D c in GetComponentsInChildren<Collider2D>(true))
			c.enabled = true;
	}

	static float CalculateHitFactor(float hueDif)
	{
		float multiplier = hueDif - MAX_HUE_DIF_TO_HEAL;
		if (multiplier < 0f)
			multiplier *= (1f / MAX_HUE_DIF_TO_HEAL) * HEAL_FACTOR;
		else
			multiplier *= (1f / (0.5f - MAX_HUE_DIF_TO_HEAL));
		return multiplier;
	}

	static int CalculateDamage(float hitFactor, float force)
	{
		if (force * hitFactor > 0f)
			return Mathf.CeilToInt(force * hitFactor);
		else
			return Mathf.FloorToInt(force * hitFactor);
	}



}
