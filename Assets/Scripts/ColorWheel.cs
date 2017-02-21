using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {

	public Transform selectorPivot;

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
		selectorPivot.localRotation = Quaternion.Euler(0f, 0f, hue * 360f);
		selectorPivot.GetComponentInChildren<SpriteRenderer>().color = Color.HSVToRGB((hue + 0.5f) % 1f, 1f, 1f);
	}
}
