using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelSelector : MonoBehaviour {

	public TextMesh playerNameLabel;
	public SpriteRenderer visor;

	public void SetValue(float hue)
	{
		transform.localRotation = Quaternion.Euler(0f, 0f, hue * 360f);
	}

	public void SetName(string name)
	{
		visor.enabled = false;
		playerNameLabel.text = name;
	}

	void Update()
	{
		float angle = transform.rotation.eulerAngles.z ;
		bool inverseScale = angle > 90f && angle < 270f;
		if (inverseScale && playerNameLabel.transform.localScale.y > 0f || !inverseScale && playerNameLabel.transform.localScale.y < 0f)
		{
			playerNameLabel.transform.localScale = new Vector3(-playerNameLabel.transform.localScale.x, -playerNameLabel.transform.localScale.y, playerNameLabel.transform.localScale.z);
			playerNameLabel.anchor = (playerNameLabel.anchor == TextAnchor.MiddleLeft) ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
		}
		
	}


}
