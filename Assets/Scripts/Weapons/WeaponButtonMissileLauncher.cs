using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonMissileLauncher : WeaponButton {
	
	public GameObject illimited;
	public GameObject nonIllimited;

	public override void UpdateMode (ColorBeing being)
	{
		bool isIllimited = being.GetComponent<WeaponMissileLauncher>().isIllimitedMode;
		illimited.SetActive(isIllimited);
		nonIllimited.SetActive(!isIllimited);
	}
}
