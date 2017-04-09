using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Iro")]
public class FollowTarget : IroCharacterAction {

	[Tooltip("The character to shoot at")]
	public SharedGameObject targetGO;

	public SharedFloat minDistance;

	public SharedFloat maxDistance;

	public SharedFloat speed;

	public SharedFloat updatePathDelay;

	Pathfinding.Path currentPath;
	int currentPathIndex;
	bool isCalculatingPath = false;

	float lastUpdatePath;

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
		{
			float distance = (targetGO.Value.transform.position - this.transform.position).magnitude;
			if (!isCalculatingPath && Time.time > lastUpdatePath + updatePathDelay.Value)
			{
				Vector3 target = targetGO.Value.transform.position;
				if (distance < minDistance.Value)
					target = this.transform.position - (targetGO.Value.transform.position - this.transform.position).normalized * 100f;
				Seeker seeker = controller.GetComponent<Seeker>();
				isCalculatingPath = true;
				seeker.StartPath(seeker.transform.position, target, ((Pathfinding.Path p) => {currentPath = p; currentPathIndex = 2;isCalculatingPath = false;lastUpdatePath = Time.time;}));
			}
			if (currentPath != null && currentPath.vectorPath.Count > currentPathIndex)
			{
				Vector3 nextTarget = currentPath.vectorPath[currentPathIndex];
				Vector2 dif = nextTarget - this.transform.position;
				while (dif.magnitude < 0.1f)
				{
					currentPathIndex++;
					if (currentPathIndex >= currentPath.vectorPath.Count)
					{
						currentPath = null;
						break;
					}
					nextTarget = currentPath.vectorPath[currentPathIndex];
					dif = nextTarget - this.transform.position;
				}
				if (distance < minDistance.Value || distance > maxDistance.Value)
					controller.velocity = (nextTarget - this.transform.position).normalized * speed.Value;
				else
					controller.velocity = Vector2.zero;
			}

//			Vector2 dif = targetGO.Value.transform.position - this.transform.position;
//			float distance = dif.magnitude;
//			if (distance < minDistance.Value)
//				controller.velocity = -(targetGO.Value.transform.position - this.transform.position).normalized * speed.Value;
//			else if (distance > maxDistance.Value)
//				controller.velocity = (targetGO.Value.transform.position - this.transform.position).normalized * speed.Value;
//			else
//				controller.velocity = Vector2.zero;
			return TaskStatus.Running;
		}
		return TaskStatus.Failure;
	}
}
