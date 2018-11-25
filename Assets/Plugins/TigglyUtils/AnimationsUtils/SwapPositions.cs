using UnityEngine;
using System.Collections;

public class SwapPositions : MonoBehaviour {

	public GameObject[] objects;

	public float timeBetweenSwapRounds = 5f;
	public float timeBetweenSwaps = 0.5f;
	public float timeSwaps = 0.5f;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutQuad;

	void Start () 
	{
		InvokeRepeating("SwapRound", 1f, timeBetweenSwapRounds);
	}

	void SwapRound()
	{
		for (int i = 0 ; i < objects.Length ; i++)
		{
			GameObject go = objects[i];
			GameObject nextGo = objects[(i + 1) % objects.Length];
			iTween.MoveTo(go, iTween.Hash(
				"position", nextGo.transform.localPosition,
				"time", timeSwaps,
				"easetype", easeType,
				"islocal", true,
				"delay", timeBetweenSwaps * i));
		}
	}

}
