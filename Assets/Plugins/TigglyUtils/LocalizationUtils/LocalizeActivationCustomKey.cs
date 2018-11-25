using UnityEngine;
using System.Collections;

public class LocalizeActivationCustomKey : LocalizeActivation {

	[SerializeField]
	string key;
	
	protected override string keyToUse {
		get
		{
			return key;
		}
	}
}
