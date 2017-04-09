using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {

	public ColorWheelSelector selectorPivot;

	Dictionary<ColorBeing, ColorWheelSelector> otherPlayersSelectors = new Dictionary<ColorBeing, ColorWheelSelector>();

	public bool isShowing
	{
		get
		{
			return gameObject.activeSelf;
		}
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void UpdateSelector(float hue)
	{
		transform.localRotation = Quaternion.Euler(0f, 0f, -hue * 360f);
		selectorPivot.SetValue(hue);
//		selectorPivot.GetComponentInChildren<SpriteRenderer>().color = Color.HSVToRGB((hue + 0.5f) % 1f, 1f, 1f);
	}

	public void UpdateSelector(ColorBeing being, float hue)
	{
		if (!otherPlayersSelectors.ContainsKey(being))
		{
			ColorWheelSelector newPivot = Instantiate(selectorPivot, this.transform);
			otherPlayersSelectors.Add(being, newPivot);
			newPivot.SetName(being.playerName);
		}
		UpdateSelector(otherPlayersSelectors[being], hue);
	}

	void UpdateSelector(ColorWheelSelector pivot, float hue)
	{
		pivot.SetValue(hue);
	}
}
