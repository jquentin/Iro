using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PersistentInt : PersistentType<int> {

	public PersistentInt(string key, int initialValue):base(key, initialValue){}

	protected override int Deserialize ()
	{
		return PlayerPrefs.GetInt(key, initialValue);
	}

	protected override void Serialize (int value)
	{
		PlayerPrefs.SetInt(key, value);
	}
}
