using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonGun : WeaponButton {

	public GameObject normal;
	public GameObject large;

	public override void UpdateMode (ColorBeing being)
	{
		bool isLarge = being.GetComponent<WeaponSimpleGun>().isLargeMode;
		large.SetActive(isLarge);
		normal.SetActive(!isLarge);

	}

}
