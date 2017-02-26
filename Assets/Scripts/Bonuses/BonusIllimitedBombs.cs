using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusIllimitedBombs : Bonus {
	
	protected override void ApplyBonusEffects ()
	{
		being.GetComponent<BombDropper>().isIllimitedMode = true;
	}

	protected override void FinishBonusEffects ()
	{
		being.GetComponent<BombDropper>().isIllimitedMode = false;
	}
}
