using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusIllimitedMissiles : Bonus {
	
	protected override void ApplyBonusEffects ()
	{
		being.GetComponent<WeaponMissileLauncher>().isIllimitedMode = true;
	}

	protected override void FinishBonusEffects ()
	{
		being.GetComponent<WeaponMissileLauncher>().isIllimitedMode = false;
	}
}
