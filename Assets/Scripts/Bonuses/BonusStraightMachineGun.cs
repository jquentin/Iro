using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStraightMachineGun : Bonus {
	
	protected override void ApplyBonusEffects ()
	{
		being.GetComponent<WeaponMachineGun>().SetStraightMode(true);
	}

	protected override void FinishBonusEffects ()
	{
		being.GetComponent<WeaponMachineGun>().SetStraightMode(false);
	}
}
