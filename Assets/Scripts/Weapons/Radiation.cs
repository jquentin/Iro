using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiation : MonoBehaviour 
{
	
	WeaponRadiation _weapon;
	WeaponRadiation weapon
	{
		get
		{
			if (_weapon == null)
				_weapon = GetComponentInParent<WeaponRadiation>();
			return _weapon;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		weapon.Touched(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		weapon.Touched(other);
	}
}
