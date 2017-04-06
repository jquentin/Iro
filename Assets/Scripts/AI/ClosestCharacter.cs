using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class ClosestCharacter : Conditional {

	[Tooltip("The character object variable that will be set")]
	public SharedGameObject closestCharacterGO;

	public override TaskStatus OnUpdate ()
	{
		List<PlayerController> otherPlayers = GameObject.FindObjectsOfType<PlayerController>().ToList().FindAll(p=>p.gameObject != this.gameObject).ToList();
		if (otherPlayers.Count == 0)
		{
			closestCharacterGO.Value = null;
			return TaskStatus.Failure;
		}
		else
		{
			closestCharacterGO.Value = otherPlayers.OrderBy((p)=>(p.transform.position - this.transform.position).sqrMagnitude).First().gameObject;
			return TaskStatus.Success;
		}
	}


}
