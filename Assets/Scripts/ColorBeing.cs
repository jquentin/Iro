using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBeing : MonoBehaviour {

	public const float MAX_HUE_DIF_TO_HEAL = 0.15f;
	/// <summary>
	/// The ratio between the max health that can be affected to heal if perfect color match
	/// and the max health that can be affected to damage if worst color match
	/// </summary>
	public const float HEAL_FACTOR = 0.2f;

	public Color color;

	float hue
	{
		get
		{
			return color.GetHue();
		}
	}

	public int maxHealth = 10;
	int health;

	void Awake()
	{
		health = maxHealth;
	}

	void Update()
	{
		BeShot(Color.blue, 10f);
	}

	public void BeShot (Color c, float force) 
	{
		float hue = c.GetHue();
		float hueDif = ColorUtils.GetHueDif(this.hue, hue);
		Debug.LogFormat("hue dif = {0}", hueDif);
		float damage = CalculateDamage(hueDif, force);
		Debug.LogFormat("shot by damage = {0}", damage);
	}

	public static float CalculateDamage(float hueDif, float force)
	{
//		float multiplier = (hueDif - MAX_HUE_DIF_TO_HEAL) * (1f / (0.5f - MAX_HUE_DIF_TO_HEAL));
		float multiplier = hueDif - MAX_HUE_DIF_TO_HEAL;
		if (multiplier < 0f)
			multiplier *= (1f / MAX_HUE_DIF_TO_HEAL) * HEAL_FACTOR;
		else
			multiplier *= (1f / (0.5f - MAX_HUE_DIF_TO_HEAL));
		return force * multiplier;
	}

}
