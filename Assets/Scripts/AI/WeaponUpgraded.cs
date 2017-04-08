using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class WeaponUpgraded : IroCharacterConditional {

	public Weapon weapon;

	public override TaskStatus OnUpdate ()
	{
		if (weapon.isUpgraded)
		{
			return TaskStatus.Success;
		}
		else
		{
			return TaskStatus.Failure;
		}
	}


}
