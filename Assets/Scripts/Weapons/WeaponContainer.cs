using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponContainer : MonoBehaviour {

	int currentWeaponIndex;
	Weapon currentWeapon;

	protected void SetWeapon(int index)
	{
		if (currentWeapon != null)
			Destroy(currentWeapon.gameObject);
		currentWeapon = Instantiate(WeaponManager.instance.weaponPrefabs[index], this.transform);
		currentWeapon.transform.localPosition = Vector3.zero;
		currentWeapon.transform.localRotation = Quaternion.identity;
		currentWeaponIndex = index;
	}
}
