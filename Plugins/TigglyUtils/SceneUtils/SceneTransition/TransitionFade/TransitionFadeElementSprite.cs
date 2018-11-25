using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TransitionFadeElementSprite : TransitionFadeElement {

	SpriteRenderer _spriteRenderer;
	SpriteRenderer spriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
				_spriteRenderer = GetComponent<SpriteRenderer>();
			return _spriteRenderer;
		}
	}

	public override void UpdateValue(float value)
	{
		Color c = spriteRenderer.color;
		c.a = value;
		spriteRenderer.color = c;
	}

}
