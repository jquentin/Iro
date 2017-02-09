using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : TigglyMonoBehaviour {

	public Color color;

	protected float hue
	{
		get
		{
			return color.GetHue();
		}
	}

	Color lastColor;

	protected virtual void Start()
	{
		UpdateColorSprites();
	}

	protected virtual void Update()
	{
		if (color != lastColor)
			UpdateColorSprites();
	}

	void UpdateColorSprites()
	{
		foreach(ColorSprite cs in GetComponentsInChildren<ColorSprite>(true))
			cs.UpdateColor(color);
		lastColor = color;
	}

}
