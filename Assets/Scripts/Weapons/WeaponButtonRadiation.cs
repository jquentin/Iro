using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonRadiation : WeaponButton {
	
	public GameObject normal;

	public override void UpdateMode (ColorBeing being)
	{
		normal.SetActive(true);
	}
}
