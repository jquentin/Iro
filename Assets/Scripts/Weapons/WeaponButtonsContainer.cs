using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonsContainer : MonoSingleton <WeaponButtonsContainer> {

	public List<WeaponButton> weaponButtons;

	public void UpdateButtons(int selectedWeapon)
	{
		for (int i = 0 ; i < weaponButtons.Count ; i++)
		{
			if (i == selectedWeapon)
				weaponButtons[i].Select();
			else
				weaponButtons[i].Unselect();
		}
	}

}
