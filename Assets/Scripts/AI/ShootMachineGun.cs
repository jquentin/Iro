using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class ShootMachineGun : IroCharacterAction {

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	WeaponMachineGun _gun;
	WeaponMachineGun gun
	{
		get
		{
			if (_gun == null)
				_gun = GetComponent<WeaponMachineGun>();
			return _gun;
		}
	}

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
			controller.angle = PlayerController.SignedAngle(targetGO.Value.transform.position - this.transform.position);
		gun.TryShoot();
		return TaskStatus.Running;
	}
}
