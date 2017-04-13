using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OfflineScene : MonoBehaviour {

	public static string playerName = "Player";

	void Awake()
	{
		TigglyNetworkBehaviour.offlineMode = false;
	}

	void OnGUI () 
	{
		GUI.Label(new Rect(Screen.width - 100f, 10f, 80f, 30f), "Name");
		playerName = GUI.TextField(new Rect(Screen.width - 100f, 50f, 80f, 30f), playerName);
		if (GUI.Button(new Rect(10f, 180f, 200f, 20f), "Play Offline"))
		{
			NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
			TigglyNetworkBehaviour.offlineMode = true;
			Application.LoadLevel(nm.onlineScene);
		}
	}
}
