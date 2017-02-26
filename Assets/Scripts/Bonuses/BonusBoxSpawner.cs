using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BonusBoxSpawner : NetworkBehaviour {

	public float timeBetweenSpawns = 10f;

	public List<Transform> spawners;

	public BonusBox bonusBoxPrefab;

	Dictionary<Transform, BonusBox> spawnedBonusBoxes = new Dictionary<Transform, BonusBox>();

	List<Transform> availableSpawners
	{
		get
		{
			CleanUpSpawned();
			List<Transform> availableSpawners = spawners.FindAll((Transform obj) => !spawnedBonusBoxes.ContainsKey(obj));
			return availableSpawners;
		}
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		InvokeRepeating("SpawnBonusBox", timeBetweenSpawns, timeBetweenSpawns);
	}

	void SpawnBonusBox()
	{
		List<Transform> availableSpawners = this.availableSpawners;
		if (availableSpawners.Count == 0)
			return;
		Transform chosenSpawner = availableSpawners[Random.Range(0, availableSpawners.Count)];
		BonusBox createdBox = Instantiate(bonusBoxPrefab, chosenSpawner.position, Quaternion.identity);
		spawnedBonusBoxes.Add(chosenSpawner, createdBox);
		NetworkServer.Spawn(createdBox.gameObject);
	}

	void CleanUpSpawned()
	{
		List<Transform> spawnersToRemove = new List<Transform>();
		foreach(Transform spawner in spawnedBonusBoxes.Keys)
		{
			BonusBox spawnedBonusBox = spawnedBonusBoxes[spawner];
			if (spawnedBonusBox == null)
				spawnersToRemove.Add(spawner);
		}
		foreach(Transform spawnerToRemove in spawnersToRemove)
			spawnedBonusBoxes.Remove(spawnerToRemove);
	}

}
