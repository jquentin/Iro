using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SharedWeapon : SharedVariable<Weapon> 
{

	public static implicit operator SharedWeapon(Weapon value) { return new SharedWeapon { mValue = value }; }

}
