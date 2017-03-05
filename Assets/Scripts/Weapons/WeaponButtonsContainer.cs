using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonsContainer : MonoSingleton <WeaponButtonsContainer> {

	public List<WeaponButton> weaponButtons;

	/// <summary>
	/// Updates the selected button according to the selected weapon.
	/// </summary>
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

	/// <summary>
	/// Updates the buttons visual according to the current bonuses.
	/// </summary>
	public void UpdateButtonsMode(ColorBeing being)
	{
		if (!being.isLocalPlayer)
			return;
		for (int i = 0 ; i < weaponButtons.Count ; i++)
		{
			weaponButtons[i].UpdateMode(being);
		}
	}

}
