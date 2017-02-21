using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPChangeLabel : MonoBehaviour {

	public float vanishTime = 1f;
	public float yMove = 1f;

	float timeStart;
	float yStart;

	TextMesh _textMesh;
	TextMesh textMesh
	{
		get
		{
			if (_textMesh == null)
				_textMesh = GetComponent<TextMesh>();
			return _textMesh;
		}
	}

	public string text
	{
		set
		{
			textMesh.text = value;
		}
	}

	public float alpha
	{
		set
		{
			Color c = textMesh.color;
			c.a = value;
			textMesh.color = c;
		}
	}

	public void Init (int value, float hitFactor) 
	{
		timeStart = Time.time;
		yStart = transform.position.y;
		text = value.ToString("+#;-#;0");
		if (value == 0)
			textMesh.color = Color.white;
		else if (hitFactor > 0.9f)
			textMesh.color = Color.red;
		else if (hitFactor > 0.5f)
			textMesh.color = new Color(1f, 0.5f, 0f);
		else if (hitFactor > 0f)
			textMesh.color = Color.yellow;
		else
			textMesh.color = Color.green;
	}

	void Update () 
	{
		UpdateLife((Time.time - timeStart) / vanishTime);
	}

	void UpdateLife(float value)
	{
		transform.position = new Vector3(transform.position.x, yStart + value * yMove, transform.position.z);
		alpha = 1f - value;
		if (value >= 1f)
			Destroy(gameObject);
	}
}
