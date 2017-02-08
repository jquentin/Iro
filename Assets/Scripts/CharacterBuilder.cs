using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuilder : MonoBehaviour {

	public bool isPlayable;

	void Awake () 
	{
		IroCharacterController characterController = GetComponent<IroCharacterController>();
		ControllableColorBeing colorBeing = GetComponent<ControllableColorBeing>();
		PlayerWeaponContainer weaponContainer = GetComponentInChildren<PlayerWeaponContainer>();
		if (!isPlayable)
		{
			AICharacterController newCharacterController = gameObject.AddComponent<AICharacterController>();
			newCharacterController.body = characterController.body;
			newCharacterController.moveSpeed = characterController.moveSpeed;
			ColorBeing newColorBeing = gameObject.AddComponent<ColorBeing>();
			newColorBeing.color = colorBeing.color;
			newColorBeing.maxHealth = colorBeing.maxHealth;
			WeaponContainer newWeaponContainer = weaponContainer.gameObject.AddComponent<WeaponContainer>();

			Destroy(colorBeing.colorWheel.gameObject);
			Destroy(characterController);
			Destroy(colorBeing);
			Destroy(weaponContainer);
		}
	}
}
