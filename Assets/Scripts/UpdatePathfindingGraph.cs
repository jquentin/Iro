using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathfindingGraph : MonoBehaviour {

	public float timeBetweenScans = 1f;

	float timeLastScan;

	AstarPath _path;
	AstarPath path
	{
		get
		{
			if (_path == null) _path = GetComponent<AstarPath>();
			return _path;
		}
	}

	void Start()
	{
		timeLastScan = Time.time;
	}

	void Update () 
	{
		if (Time.time > timeLastScan + timeBetweenScans)
		{
			path.Scan();
			timeLastScan = Time.time;
		}
	}
}
