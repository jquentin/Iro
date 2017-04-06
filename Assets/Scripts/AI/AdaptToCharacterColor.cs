using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class AdaptToCharacterColor : IroCharacterAction {

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	[Tooltip("The distance from the target character's hue. 0 or 1 mean same color, 0.5 means opposite color")]
	public SharedFloat targetDistance;

	[Tooltip("The margin of acceptation from the target color")]
	public SharedFloat margin;

	public SharedFloat speed;

	public override TaskStatus OnUpdate ()
	{
		if (targetGO.Value != null)
		{
			float dif = ColorUtils.GetHueDif(being.color, targetGO.Value.GetComponent<ColorBeing>().color);
			if (Mathf.Abs(dif - targetDistance.Value) > margin.Value)
				being.CmdChangeHue(speed.Value * Time.deltaTime);
		}
		return TaskStatus.Running;
	}

}
