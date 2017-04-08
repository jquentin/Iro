using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChangeColor : MonoBehaviour {


	public float timeBetweenPushes = 0.2f;

	public float force = 1f;

	public List<SpriteRenderer> externalRenderers; 

	float lastPush;

	float colorVelocity = 0f;


	SpriteRenderer _spriteRenderer;
	protected SpriteRenderer spriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
				_spriteRenderer = GetComponent<SpriteRenderer>();
			return _spriteRenderer;
		}
	}

	void Start()
	{
		lastPush = Time.time;
	}

	void Update () 
	{
		if (Time.time >= lastPush + timeBetweenPushes)
		{
			colorVelocity = Mathf.Clamp(force * Random.Range(-1f, 1f), -1f, 1f);
			lastPush = Time.time;
		}
		float hue = spriteRenderer.color.GetHue();
		hue = (hue + Time.deltaTime * colorVelocity) % 1f;
		Color c = Color.HSVToRGB(hue, 1f, 1f);
		spriteRenderer.color = c;
		foreach(SpriteRenderer sr in externalRenderers)
			sr.color = c;
	}
}
