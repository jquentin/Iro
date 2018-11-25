using UnityEngine;
using System.Collections;

public static class ColorUtils
{

	public static readonly Color WhiteClear = new Color(1f, 1f, 1f, 0f);

	public static Color Darkened(this Color color, float amount)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		v -= amount;
		return Color.HSVToRGB(h, s, v);
	}

	public static Color LowerAlpha(this Color color, float amount)
	{
		return new Color(color.r, color.g, color.b, color.a - amount);
	}

	public static Color SetAlpha(this Color color, float amount)
	{
		return new Color(color.r, color.g, color.b, amount);
	}

	public static string GetHexCode(this Color color)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}", ToByte(color.r), ToByte(color.g), ToByte(color.b));
	}

	private static byte ToByte(float f)
	{
		f = Mathf.Clamp01(f);
		return (byte)(f * 255);
	}
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

	/// <summary>
	/// Gets the HSV value.
	/// </summary>
	public static float GetValue(this Color color)
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
