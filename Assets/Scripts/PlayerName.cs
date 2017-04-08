using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour {

	bool initialized = false;

	TextMesh _textMesh;
	TextMesh textMesh
	{
		get
		{
			if (_textMesh == null) _textMesh = GetComponent<TextMesh>();
			return _textMesh;
		}
	}
	PlayerController _parentController;
	PlayerController parentController
	{
		get
		{
			if (_parentController == null) _parentController = GetComponentInParent<PlayerController>();
			return _parentController;
		}
	}

	void Update () 
	{
		if (!initialized && !string.IsNullOrEmpty(parentController.playerName))
		{
			textMesh.text = parentController.playerName;
			initialized = true;
		}
	}

}
