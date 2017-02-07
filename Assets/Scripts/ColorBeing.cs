using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBeing : ColorObject, Shootable {

	public const float MAX_HUE_DIF_TO_HEAL = 0.15f;
	/// <summary>
	/// The ratio between the max health that can be affected to heal if perfect color match
	/// and the max health that can be affected to damage if worst color match
	/// </summary>
	public const float HEAL_FACTOR = 0.2f;

	public int maxHealth = 10;
	public int health { get; protected set; }

	public bool isDead { get { return (health <= 0); } }

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

	void Awake()
	{
		health = maxHealth;
	}

	protected override void Update()
	{
		base.Update();
//		BeShot(Color.blue, 10f);
	}

	public void BeShot (Color c, float force) 
	{
		float hue = c.GetHue();
		float hueDif = ColorUtils.GetHueDif(this.hue, hue);
//		Debug.LogFormat("hue dif = {0}", hueDif);
		float hitFactor = CalculateHitFactor(hueDif);
		int damage = CalculateDamage(hitFactor, force);
		health = health - damage;
		health = Mathf.Min(health, maxHealth);
		hpChangeSpawner.HealthChange(-damage, hitFactor);
		Debug.LogFormat("{0} shot by damage: {1}, leftHealth: {2}", name, damage, health);
		if (health <= 0)
			Die();
	}

	void Die()
	{
		Debug.LogFormat("{0} dead", name);
		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
			sr.color = sr.color.LowerAlpha(0.6f);
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
