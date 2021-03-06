using System.Collections;
using System;
using System.Linq;
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
	public int _health;
	public int health 
	{
		get
		{
			return _health;
		} 
		set 
		{ 
			_health = value; 
			if (offlineMode) 
				OnHealthChanged(value); 
		} 
	}

	public HealthBar healthBar;

	public bool isDead { get { return (health <= 0); } }

	public Action OnDead;

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

	PlayerController _controller;
	PlayerController controller
	{
		get
		{
			if (_controller == null)
				_controller = GetComponent<PlayerController>();
			return _controller;
		}
	}

	public string playerName { get { return controller.playerName; } }

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

	/// <summary>
	/// Function called when the player gets shot or damaged.
	/// </summary>
	/// <param name="c">the color of the damaging effect</param>
	/// <param name="force">the force of the damage.</param>
	/// <param name="direction">The direction.</param>
	/// <param name="pushForce">The force of the push. If it is -1, then force is used.</param>
	public void BeShot (Color c, float force, Vector2 direction, float pushForce = -1f) 
	{
		if (!isServer)
			return;
		float hue = c.GetHue();
		float hueDif = ColorUtils.GetHueDif(this.hue, hue);
		float hitFactor = CalculateHitFactor(hueDif);
//		Debug.LogFormat("color shooter = {0} : hue: {1} ; color shootee = {2} : hue: {3} ; hue dif = {4} ; hitFactor = {5}", c, hue, this.color, this.hue, hueDif, hitFactor);
		int damage = CalculateDamage(hitFactor, force);
		health = Mathf.Min(health - damage, maxHealth);
		SpawnHealthChange(damage, hitFactor);
		Vector2 push = ((pushForce < 0f) ? force : pushForce) * direction.normalized;
		GetPushed(push);
	}

	[Command]
	void CmdChangeHue(float value)
	{
		OfflineChangeHue(value);
	}
	void OfflineChangeHue(float value)
	{
		float h, s, v;
		Color.RGBToHSV(this.color, out h, out s, out v);
		h += value % 1f;
		this.color = Color.HSVToRGB(h, 1f, 1f);
	}
	public void ChangeHue(float value)
	{
//		ModeDependantCall(CmdChangeHue, OfflineChangeHue, value);
		if (offlineMode)
			OfflineChangeHue(value);
		else
			CmdChangeHue(value);
	}

	[ClientRpc]
	void RpcGetPushed(Vector2 force)
	{
		OfflineGetPushed(force);
	}
	void OfflineGetPushed(Vector2 force)
	{
		GetComponent<PlayerController>().GetRecoil();
		GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
	}
	void GetPushed(Vector2 force)
	{
//		ModeDependantCall(RpcGetPushed, OfflineGetPushed, force);
		if (offlineMode)
			OfflineGetPushed(force);
		else
			RpcGetPushed(force);
	}

	void OnHealthChanged(int health)
	{
		this._health = health;
		healthBar.UpdateBar((float)health / (float) maxHealth);
		if (health <= 0)
			Die();
	}

	[ClientRpc]
	void RpcSpawnHealthChange(int damage, float hitFactor)
	{
		OfflineSpawnHealthChange(damage, hitFactor);
	}
	void OfflineSpawnHealthChange(int damage, float hitFactor)
	{
		Debug.LogFormat("{0} shot by damage: {1}, leftHealth: {2}", name, damage, health);
		hpChangeSpawner.HealthChange(-damage, hitFactor);
	}
	void SpawnHealthChange(int damage, float hitFactor)
	{
//		ModeDependantCall(RpcSpawnHealthChange, OfflineSpawnHealthChange, damage, hitFactor);
		if (offlineMode)
			OfflineSpawnHealthChange(damage, hitFactor);
		else
			RpcSpawnHealthChange(damage, hitFactor);
	}

	void Die()
	{
		Debug.LogFormat("{0} dead", name);
//		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
//			sr.color = sr.color.LowerAlpha(0.6f);
		foreach(Collider2D c in GetComponentsInChildren<Collider2D>(true))
			c.enabled = false;
		Respawn();
		if (OnDead != null)
			OnDead();
	}

	void Respawn()
	{
		StartCoroutine(Respawn_CR());
	}
	void ActualRespawn()
	{
		transform.position = initPos;
		if (isLocalPlayer || isServer && !(this is ControllableColorBeing))
			Resuscitate();
	}

	[Command]    
	void CmdResuscitate()
	{  
		OfflineResuscitate();  
	}  
	void OfflineResuscitate()
	{   
		health = maxHealth;
	}   
	void Resuscitate()
	{ 
		if (offlineMode)
			OfflineResuscitate();
		else
			CmdResuscitate();   
	}

	void SetRenderersAlpha(float alpha, Dictionary<SpriteRenderer, float> renderersAlpha)
	{
		foreach(SpriteRenderer sr in renderersAlpha.Keys)
			sr.color = sr.color.SetAlpha(alpha * renderersAlpha[sr]);
	}
		
	IEnumerator Respawn_CR()
	{
		List<SpriteRenderer> spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true).ToList();
		Dictionary<SpriteRenderer, float> renderersAlpha = spriteRenderers.ToDictionary(sr => sr, sr => sr.color.a);

		SetRenderersAlpha(0.5f, renderersAlpha);
		yield return new WaitForSeconds(1f);

		SetRenderersAlpha(0f, renderersAlpha);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0.5f, renderersAlpha);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0f, renderersAlpha);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0.5f, renderersAlpha);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(0f, renderersAlpha);
		yield return new WaitForSeconds(0.15f);
		SetRenderersAlpha(1f, renderersAlpha);
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

	protected override void OnColorChangedVirtual (Color color)
	{
		base.OnColorChangedVirtual (color);
		if (!isLocalPlayer)
		{
			GameObject playerGO = GameObject.FindGameObjectsWithTag("Player").First((GameObject arg) => arg.GetComponent<PlayerController>().isLocalPlayer);
			if (playerGO == null) return;
			ColorWheel playerColorWheel = playerGO.GetComponentInChildren<ColorWheel>(true);
			if (playerColorWheel == null) return;
			playerColorWheel.UpdateSelector(this, color.GetHue());
		}
	}

}
