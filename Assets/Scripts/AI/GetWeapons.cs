using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class GetWeapons : IroCharacterConditional {
	
	public SharedWeapon simpleGun;

	public SharedWeapon machineGun;

	public SharedWeapon missileLauncher;

	public SharedWeapon bombDropper;

	public SharedWeapon radiation;

	public override TaskStatus OnUpdate ()
	{
		simpleGun.Value = being.GetComponent<WeaponSimpleGun>();
		machineGun.Value = being.GetComponent<WeaponMachineGun>();
		missileLauncher.Value = being.GetComponent<WeaponMissileLauncher>();
		bombDropper.Value = being.GetComponent<BombDropper>();
		radiation.Value = being.GetComponent<WeaponRadiation>();
		return TaskStatus.Success;
	}


}
