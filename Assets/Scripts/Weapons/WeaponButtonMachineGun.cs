using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonMachineGun : WeaponButton {
	
	public GameObject normal;
	public GameObject straight;

	public override void UpdateMode (ColorBeing being)
	{
		bool isStraight = being.GetComponent<WeaponMachineGun>().isStraightMode;
		straight.SetActive(isStraight);
		normal.SetActive(!isStraight);
		
	}
}
