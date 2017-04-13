using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : ColorObject 
{

	public List<AudioClip> explodeSound;

	[ClientRpc]
	void RpcInit(float scale, Color color, bool playSound)
	{
		OfflineInit(scale, color, playSound);
	}
	void OfflineInit(float scale, Color color, bool playSound)
	{
		if (playSound)
		{
			audioSource.PlayOneShotControlled(explodeSound, AudioType.Sound);
		}
		this.color = color;
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", transform.localScale * scale,
			"time", 0.5f,
			"oncomplete", "DestroyExlosion"));
		iTween.FadeTo(gameObject, 0f, 0.5f);
	}
	void Init(float scale, Color color, bool playSound)
	{
		ModeDependantCall(RpcInit, OfflineInit, scale, color, playSound);
	}

	public static void CreateExplosion(Explosion explosionPrefab, Vector2 pos, float scale, Color color, bool playSound = true)
	{
		Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
		SpawnIfOnline(explosion.gameObject);
		explosion.Init(scale, color, playSound);

//		Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, scale * 10f);
//		foreach(Collider2D collider in colliders)
//		{
//			ColorBeing being = collider.GetComponentInParent<ColorBeing>();
//			if (being != null)
//			{
//				being.GetComponent<Rigidbody2D>().AddForce(((Vector2)being.transform.position - pos).normalized * scale * 10f, ForceMode2D.Impulse);
//			}
//		}
	}

	void DestroyExlosion()
	{
		Destroy(gameObject);
	}

}
