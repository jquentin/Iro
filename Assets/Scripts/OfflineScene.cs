using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineScene : MonoBehaviour {

	public static string playerName = "Player";

	void OnGUI () 
	{
		GUI.Label(new Rect(Screen.width - 100f, 10f, 80f, 30f), "Name");
		playerName = GUI.TextField(new Rect(Screen.width - 100f, 50f, 80f, 30f), playerName);
	}
}
