using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils {

	public static float GetHue(this Color color)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		return h;
	}

	public static float GetSaturation(this Color color)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		return s;
	}

	public static float GetV(this Color color)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		return v;
	}

	public static float GetHueDif(float hue1, float hue2)
	{
		float hueDif = (hue1 - hue2) % 1f;
		if (hueDif < 0f) hueDif += 1f;
		if (hueDif > 0.5f) hueDif = 1f - hueDif;
		return hueDif;
	}

	public static float GetHueDif(Color c1, Color c2)
	{
		float h1 = c1.GetHue();
		float h2 = c2.GetHue();
		return GetHueDif(h1, h2);
	}

}
