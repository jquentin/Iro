//#define TMProFadeEnabled
// Add TMProFadeEnabled to your Player Settings Scripting Define Symbols to enable fading of Text Mesh Pro components

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if TMProFadeEnabled
using TMPro;
#endif

public class AlphaTweenable : Tweenable {

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
	private List<Renderer> _renderers;
	private List<Renderer> renderers 
	{
		get
		{
			if (_renderers == null)
			{
				_renderers = new List<Renderer> (GetComponentsInChildren<Renderer>());
#if TMProFadeEnabled
				_renderers.RemoveAll(renderer => renderer.GetComponent<TextMeshPro>() != null);
#endif
				_renderers.RemoveAll(renderer => renderer is SpriteRenderer);
			}
			return _renderers;
		}
	}
#if TMProFadeEnabled
	private TextMeshPro[] _textMeshProRenderers;
	private TextMeshPro[] textMeshProRenderers
	{
		get
		{
			if (_textMeshProRenderers == null)
				_textMeshProRenderers = GetComponentsInChildren<TextMeshPro>(true);
			return _textMeshProRenderers;
		}
	}
#endif
	private UIWidget[] _NGUIWidgets;
	private UIWidget[] NGUIWidgets
	{
		get
		{
			if (_NGUIWidgets == null)
				_NGUIWidgets = GetComponentsInChildren<UIWidget>();
			return _NGUIWidgets;
		}
	}
	private AnimatedColor[] _animatedColorComponents;
	private AnimatedColor[] animatedColorComponents
	{
		get
		{
			if (_animatedColorComponents == null)
				_animatedColorComponents = GetComponentsInChildren<AnimatedColor>();
			return _animatedColorComponents;
		}
	}

	private Collider[] _colliders = null;
	private Collider[] colliders
	{
		get
		{
			if (_colliders == null)
				_colliders = GetComponentsInChildren<Collider>();
			return _colliders;
		}
	}
	public float alpha { get; private set; }
	public bool disableColliders = false;
	public bool ignoreTimeScale = false;

	void Awake()
	{
		if (spriteRenderers.Length > 0)
			alpha = spriteRenderers[0].color.a;
		else if (renderers.Count > 0)
			alpha = renderers[0].material.color.a;
#if TMProFadeEnabled
		else if (textMeshProRenderers.Length > 0)
			alpha = textMeshProRenderers[0].color.a;
#endif
		if (NGUIWidgets.Length > 0)
			alpha = NGUIWidgets[0].color.a;
		if (animatedColorComponents.Length > 0)
			alpha = animatedColorComponents[0].color.a;
	}

	public void UpdateAlpha(float value)
	{
		foreach(SpriteRenderer r in spriteRenderers)
			r.color = new Color(r.color.r, r.color.g, r.color.b, value);
		foreach(Renderer r2 in renderers)
			r2.material.color = new Color(r2.material.color.r, r2.material.color.g, r2.material.color.b, value);
		
#if TMProFadeEnabled
		foreach(TextMeshPro r3 in textMeshProRenderers)
		{
			if (Mathf.Max(r3.color.r, r3.color.g, r3.color.b) <= 1f){
				r3.color = new Color(r3.color.r, r3.color.g, r3.color.b, value);
			}else{
				Color temp = new Color(r3.color.r, r3.color.g, r3.color.b, value);
				r3.faceColor = (Color32)temp;
//				r3.faceColor = new Color32(r3.color.r, r3.color.g, r3.color.b, (byte) (value * 255f));
			}

		}
#endif
		foreach(UIWidget w in NGUIWidgets)
			w.color = new Color(w.color.r, w.color.g, w.color.b, value);
		foreach(AnimatedColor ac in animatedColorComponents)
			ac.color = new Color(ac.color.r, ac.color.g, ac.color.b, value);
		if (disableColliders)
		{
			foreach(Collider c in GetComponentsInChildren<Collider>())
				c.enabled = (value != 0f);
		}
		alpha = value;
	}

	public void TweenAlphaFrom(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenAlpha(float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", alpha,
			"to", to,
			"time", time,
			"delay", delay,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
	
	public void TweenAlphaPingPong(float from, float to, float time, float delay = 0f, iTween.EaseType easeType = iTween.EaseType.easeOutExpo)
	{
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", from,
			"to", to,
			"time", time,
			"delay", delay,
			"loopType", iTween.LoopType.pingPong,
			"onupdate", "UpdateAlpha",
			"ignoretimescale", ignoreTimeScale,
			"name", TweenName,
			"easetype", easeType));
	}
}
