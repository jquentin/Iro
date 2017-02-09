using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : ColorObject 
{

	void Init(float scale, Color color)
	{
		this.color = color;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", transform.localScale * scale,
			"time", 0.5f));
		iTween.FadeTo(gameObject, 0f, 0.5f);
	}

	public static void CreateExplosion(Explosion explosionPrefab, Vector2 pos, float scale, Color color)
	{
		Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
		explosion.Init(scale, color);
	}

}
