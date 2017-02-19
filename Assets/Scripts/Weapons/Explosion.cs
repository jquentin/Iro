﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : ColorObject 
{

	public List<AudioClip> explodeSound;

	[ClientRpc]
	void RpcInit(float scale, Color color)
	{
		audioSource.PlayOneShotControlled(explodeSound, AudioType.Sound);
		this.color = color;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", transform.localScale * scale,
			"time", 0.5f));
		iTween.FadeTo(gameObject, 0f, 0.5f);
	}

	public static void CreateExplosion(Explosion explosionPrefab, Vector2 pos, float scale, Color color)
	{
		Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
		NetworkServer.Spawn(explosion.gameObject);
		explosion.RpcInit(scale, color);
	}

}