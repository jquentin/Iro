using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PersistentBool : PersistentType<bool> {

	public PersistentBool(string key, bool initialValue):base(key, initialValue){}
		
	int BoolToInt(bool b)
	{
		return (b ? 1 : 0);
	}

	bool IntToBool(int i)
	{
		return (i > 0);
	}

	protected override bool Deserialize ()
	{
		return IntToBool(PlayerPrefs.GetInt(key, BoolToInt(initialValue)));
	}

	protected override void Serialize (bool value)
	{
		PlayerPrefs.SetInt(key, BoolToInt(value));
	}
}
