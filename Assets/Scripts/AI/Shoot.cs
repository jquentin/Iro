using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class Shoot : IroCharacterAction {

	float timeStart;

	public float timeBeforeEnd = 1f;

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	WeaponSimpleGun _gun;
	WeaponSimpleGun gun
	{
		get
		{
			if (_gun == null)
				_gun = GetComponent<WeaponSimpleGun>();
			return _gun;
		}
	}

	public override void OnStart ()
	{
		timeStart = Time.time;
	}

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
			controller.angle = PlayerController.SignedAngle(targetGO.Value.transform.position - this.transform.position);
		if (Time.time - timeStart >= timeBeforeEnd)
		{
			gun.Shoot();
			timeStart = Time.time;
		}
		return TaskStatus.Running;
	}
}
