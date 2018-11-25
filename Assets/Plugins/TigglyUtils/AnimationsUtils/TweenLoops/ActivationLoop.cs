using UnityEngine;
using System.Collections;

public class ActivationLoop : MonoBehaviour {

	public float timeBetweenEvents = 0.2f;
	public bool differentTimeBack = false;
	public float timeBetweenEventsBack = 0.2f;

	float timeBeforeNextEvent
	{
		get
		{
			if (forward)
				return timeBetweenEvents;
			else
			{
				if (differentTimeBack)
					return timeBetweenEventsBack;
				else
					return timeBetweenEvents;
			}
		}
	}

	bool forward;

	public GameObject target;

	private float lastTimeEvent;

	public bool initialState = false;

	void Start()
	{
		lastTimeEvent = Time.realtimeSinceStartup;
		target.SetActive(initialState);
		forward = !initialState;
	}

	void Update () 
	{
		if (Time.realtimeSinceStartup - lastTimeEvent >= timeBeforeNextEvent)
		{
			target.SetActive(!target.activeSelf);
			lastTimeEvent = Time.realtimeSinceStartup;
			forward = !target.activeSelf;
		}
	}
}
