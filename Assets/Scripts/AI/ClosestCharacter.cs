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

	public SharedFloat distanceClosestCharacter;

	[Tooltip("The bonus object variable that will be set")]
	public SharedGameObject closestBonus;

	public SharedFloat distanceClosestBonus;

	public override TaskStatus OnUpdate ()
	{
		List<PlayerController> otherPlayers = GameObject.FindObjectsOfType<PlayerController>().ToList().FindAll(p=>p.gameObject != this.gameObject).ToList();
		if (otherPlayers.Count == 0)
		{
			closestCharacterGO.Value = null;
			distanceClosestCharacter.Value = float.MaxValue;
		}
		else
		{
			closestCharacterGO.Value = otherPlayers.OrderBy((p)=>(p.transform.position - this.transform.position).sqrMagnitude).First().gameObject;
			distanceClosestCharacter.Value = (closestCharacterGO.Value.transform.position - this.transform.position).sqrMagnitude;
		}
		List<BonusBox> bonuses = GameObject.FindObjectsOfType<BonusBox>().ToList();
		if (bonuses.Count == 0)
		{
			closestBonus.Value = null;
			distanceClosestBonus.Value = float.MaxValue;
		}
		else
		{
			closestBonus.Value = bonuses.OrderBy((p)=>(p.transform.position - this.transform.position).sqrMagnitude).First().gameObject;
			distanceClosestBonus.Value = (closestBonus.Value.transform.position - this.transform.position).sqrMagnitude;
		}
		return TaskStatus.Success;
	}


}
