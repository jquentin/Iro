using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSceneManager : MonoBehaviour {

	public GameObject aiPlayerPrefab;

	GlobalControlManager controlManager;
	NetworkManager networkManager;

	public NetworkIdentity[] networkIdentitiesToWake;

	void OnEnable()
	{
		networkManager = GameObject.FindObjectOfType<NetworkManager>();
		controlManager = GameObject.FindObjectOfType<GlobalControlManager>();
	}

	IEnumerator Start () 
	{
		if (TigglyNetworkBehaviour.offlineMode)
		{
			networkManager.gameObject.SetActive(false);

			GameObject spawnedPlayer = GlobalControlManager.SpawnPlayer(networkManager.playerPrefab);
			GameObject spawnedAI1 = GlobalControlManager.SpawnPlayer(aiPlayerPrefab);
			GameObject spawnedAI2 = GlobalControlManager.SpawnPlayer(aiPlayerPrefab);
			GameObject spawnedAI3 = GlobalControlManager.SpawnPlayer(aiPlayerPrefab);

			yield return new WaitForEndOfFrame();
			foreach(NetworkIdentity ni in networkIdentitiesToWake)
				ni.gameObject.SetActive(true);
			spawnedPlayer.gameObject.SetActive(true);
			spawnedAI1.gameObject.SetActive(true);
			spawnedAI2.gameObject.SetActive(true);
			spawnedAI3.gameObject.SetActive(true);

//			controlManager.gameObject.SetActive(true);
		}
	}
	

}
