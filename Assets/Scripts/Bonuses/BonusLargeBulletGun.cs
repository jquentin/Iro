using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLargeBulletGun : Bonus {
	
	protected override void ApplyBonusEffects ()
	{
		being.GetComponent<WeaponSimpleGun>().isLargeMode = true;
	}

	protected override void FinishBonusEffects ()
	{
		being.GetComponent<WeaponSimpleGun>().isLargeMode = false;
	}
}
