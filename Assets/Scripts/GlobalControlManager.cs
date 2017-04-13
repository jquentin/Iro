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
		SpawnPlayer(aiPlayerPrefab.gameObject);
	}
	public void SpawnAiPlayer()
	{
		ModeDependantCall(CmdSpawnAiPlayer, OfflineSpawnAiPlayer);
	}

	public static GameObject SpawnPlayer(GameObject playerPrefab)
	{
		GameObject spawned = Instantiate(playerPrefab);
		List<NetworkStartPosition> startPositions = GameObject.FindObjectsOfType<NetworkStartPosition>().ToList();
		RandomExhaustInt re = new RandomExhaustInt(0, startPositions.Count - 1, ExclusionMode.FixedRandomizedElmts, 1, "startPositions");
		spawned.transform.position = startPositions[re.Pick()].transform.position;
		SpawnIfOnline(spawned.gameObject);
		return spawned;
	}

}
