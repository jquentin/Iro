using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPChangeSpawner : MonoBehaviour {

	public HPChangeLabel hpChangePrefab;

	public void HealthChange(int value, float hitFactor)
	{
		HPChangeLabel spawnedLabel = Instantiate(hpChangePrefab);
		spawnedLabel.transform.position = transform.position;
		spawnedLabel.Init(value, hitFactor);
	}
}
