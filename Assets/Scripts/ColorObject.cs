using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorObject : TigglyNetworkBehaviour {

	[SyncVar(hook="OnColorChanged")]
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

	public void UpdateColorSprites()
	{
		foreach(ColorSprite cs in GetComponentsInChildren<ColorSprite>(true))
			cs.UpdateColor(color);
		lastColor = color;
	}

	void OnColorChanged(Color color)
	{
		this.color = color;
		UpdateColorSprites();
		OnColorChangedVirtual(color);
	}


	protected virtual void OnColorChangedVirtual(Color color)
	{
	}

}
