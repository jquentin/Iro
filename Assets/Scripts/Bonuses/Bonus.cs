using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : TigglyMonoBehaviour {

	public float timeBonus;

	ColorBeing _being;
	protected ColorBeing being
	{
		get
		{
			if (_being == null)
				_being = GetComponent<ColorBeing>();
			return _being;
		}
	}

	public Bonus ApplyToBeing(ColorBeing being)
	{
		Bonus createdBonus = (Bonus) being.gameObject.AddComponent(this.GetType());
		createdBonus.timeBonus = this.timeBonus;
		createdBonus.ApplyBonus();
		return createdBonus;
	}

	void ApplyBonus()
	{
		audioSource.PlayOneShotControlled(LevelAudioManager.instance.bonusTaken, AudioType.Sound);
		ApplyBonusEffects();
		Invoke("FinishBonus", timeBonus);
		WeaponButtonsContainer.instance.UpdateButtonsMode(being);
	}

	void FinishBonus()
	{
		FinishBonusEffects();
		WeaponButtonsContainer.instance.UpdateButtonsMode(being);
		Destroy(this);
	}

	protected abstract void ApplyBonusEffects();

	protected abstract void FinishBonusEffects();

}
