using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class FollowCharacter : IroCharacterAction {

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	public SharedFloat minDistance;

	public SharedFloat maxDistance;

	public SharedFloat speed;

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
		{
			Vector2 dif = targetGO.Value.transform.position - this.transform.position;
			float distance = dif.magnitude;
			if (distance < minDistance.Value)
				controller.velocity = -(targetGO.Value.transform.position - this.transform.position).normalized * speed.Value;
			else if (distance > maxDistance.Value)
				controller.velocity = (targetGO.Value.transform.position - this.transform.position).normalized * speed.Value;
			else
				controller.velocity = Vector2.zero;
		}
		return TaskStatus.Running;
	}
}
