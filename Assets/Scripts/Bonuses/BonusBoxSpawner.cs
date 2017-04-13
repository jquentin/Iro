using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BonusBoxSpawner : TigglyNetworkBehaviour {

	public float timeBetweenSpawns = 10f;

	public List<Transform> spawners;

	public BonusBox bonusBoxPrefab;

	Dictionary<Transform, BonusBox> spawnedBonusBoxes = new Dictionary<Transform, BonusBox>();

	float timeLastSpawn;

	List<Transform> availableSpawners
	{
		get
		{
			CleanUpSpawned();
			List<Transform> availableSpawners = spawners.FindAll((Transform obj) => !spawnedBonusBoxes.ContainsKey(obj));
			return availableSpawners;
		}
	}

	void Start ()
	{
		if (isServer)
			timeLastSpawn = Time.time;
	}

	void Update()
	{

		if (!isServer)
			return;
		if (Time.time >= timeLastSpawn + timeBetweenSpawns)
			SpawnBonusBox();
	}

	void SpawnBonusBox()
	{
		List<Transform> availableSpawners = this.availableSpawners;
		if (availableSpawners.Count == 0)
			return;
		Transform chosenSpawner = availableSpawners[Random.Range(0, availableSpawners.Count)];
//		BonusBox createdBox = Instantiate(bonusBoxPrefab, chosenSpawner.position, Quaternion.identity);
		BonusBox createdBox = BonusBox.CreateBonusBox(bonusBoxPrefab, chosenSpawner.position, Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f));
		spawnedBonusBoxes.Add(chosenSpawner, createdBox);
		SpawnIfOnline(createdBox.gameObject);
		timeLastSpawn = Time.time;
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
