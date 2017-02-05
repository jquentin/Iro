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
		selectorPivot.localRotation = Quaternion.Euler(0f, 0f, hue * 360f);
	}
}
