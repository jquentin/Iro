using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalControlManager : TigglyNetworkBehaviour {

	public CharacterBuilder aiPlayerPrefab;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			CmdRestartGame();
		}
	}

	void CmdRestartGame()
	{
		Application.LoadLevel (0); 
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 100f, 10f, 100f, 30f), "Spawn AI player"))
		{
			SpawnAiPlayer();
		}
	}

	[Command]
	void CmdSpawnAiPlayer()
	{
		OfflineSpawnAiPlayer();
	}
	void OfflineSpawnAiPlayer()
	{		
		CharacterBuilder spawnedAI = Instantiate(aiPlayerPrefab);
//		spawnedAI.isPlayable = false;
//		spawnedAI.Init();
		spawnedAI.transform.position = GameObject.FindObjectsOfType<NetworkStartPosition>().ToList().PickRandomElement().transform.position;
		SpawnIfOnline(spawnedAI.gameObject);
	}
	void SpawnAiPlayer()
	{
		ModeDependantCall(CmdSpawnAiPlayer, OfflineSpawnAiPlayer);
	}
}
