using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class WeaponUpgraded : IroCharacterConditional {

	public SharedWeapon weapon;

	public override TaskStatus OnUpdate ()
	{
		if (weapon.Value.isUpgraded)
		{
			return TaskStatus.Success;
		}
		else
		{
			return TaskStatus.Failure;
		}
	}


}
