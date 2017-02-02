using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorSprite : MonoBehaviour 
{

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

	public void UpdateColor (Color c) 
	{
		spriteRenderer.color = new Color(c.r, c.g, c.b, spriteRenderer.color.a);
	}
}
