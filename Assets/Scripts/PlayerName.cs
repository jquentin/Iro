using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour {

	void Start () 
	{
		GetComponent<TextMesh>().text = GetComponentInParent<PlayerController>().playerName;
	}

}
