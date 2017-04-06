using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class Shoot : Action {

	float timeStart;

	public float timeBeforeEnd = 1f;

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	public override void OnStart ()
	{
//		WeaponContainer weaponContainer = GetComponent<WeaponContainer>();
//		weaponContainer.weaponComponents[weaponContainer.currentWeaponIndex].Shoot();
//		GetComponent<WeaponSimpleGun>().CmdShoot();
		timeStart = Time.time;
	}

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
			GetComponent<PlayerController>().angle = PlayerController.SignedAngle(targetGO.Value.transform.position - this.transform.position);
		if (Time.time - timeStart >= timeBeforeEnd)
		{
			GetComponent<WeaponSimpleGun>().CmdShoot();
			timeStart = Time.time;
		}
		return TaskStatus.Running;
	}
}
