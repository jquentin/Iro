using UnityEngine;
using System.Collections;

public class ColorTweenable : Tweenable {
	
	private SpriteRenderer[] _spriteRenderers = null;
	private SpriteRenderer[] spriteRenderers 
	{
		get
		{
			if (_spriteRenderers == null)
				_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
			return _spriteRenderers;
		}
	}
	private Renderer[] _renderers;
	private Renderer[] renderers 
	{
		get
		{
			if (_renderers == null)
				_renderers = GetComponentsInChildren<Renderer>();
			return _renderers;
		}
	}
	public Color color { get; private set; }
	
	void Awake()
	{
		color = Color.white;
	}
	
	public void UpdateColor(Color value)
	{
		foreach(SpriteRenderer r in spriteRenderers)
			r.color = value;
		foreach(Renderer r2 in renderers)
			r2.material.color = value;
		color = value;
	}
	
	public void TweenColorFrom(Color from, Color to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateColor",
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenColor(Color to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", color,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateColor",
			"name", TweenName,
			"easetype", easeType));
	}
}
