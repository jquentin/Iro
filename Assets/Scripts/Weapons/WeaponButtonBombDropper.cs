using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonBombDropper : WeaponButton {

	public GameObject illimited;
	public GameObject nonIllimited;

	public override void UpdateMode (ColorBeing being)
	{
		bool isIllimited = being.GetComponent<BombDropper>().isIllimitedMode;
		illimited.SetActive(isIllimited);
		nonIllimited.SetActive(!isIllimited);
	}
}
