using UnityEngine;
using System.Collections;

public class TestPersistentBool : MonoBehaviour {

	public PersistentBool persistentBool;

	public bool setValue;
	bool lastSetValue;

	// Use this for initialization
	void Start () {
		print("value = " + (persistentBool ? "true" : "false"));
		setValue = lastSetValue = persistentBool;
	}
	
	// Update is called once per frame
	void Update () {
		if (setValue != lastSetValue)
		{
			persistentBool.SetValue(setValue);
			lastSetValue = setValue;
		}
	}
}
