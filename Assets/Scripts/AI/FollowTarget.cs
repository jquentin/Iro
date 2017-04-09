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

	[Tooltip("Move around the target once it's at an accepted distance")]
	public SharedBool moveAroundWhenReached;

	[Tooltip("The time it will take between direction changes when moving around the target after it's reached")]
	public SharedFloat timePingPongMoveAround;

	Pathfinding.Path currentPath;
	int currentPathIndex;
	bool isCalculatingPath = false;

	float lastUpdatePath;

	bool isMovingAround = false;
	bool isMovingAroundTrigo;

	public override TaskStatus OnUpdate ()
	{
		Debug.Log(targetGO.Value);
		if (targetGO.Value != null)
		{
			Vector2 difToTarget = targetGO.Value.transform.position - this.transform.position;
			float distanceToTarget = difToTarget.magnitude;
			if (!isCalculatingPath && Time.time > lastUpdatePath + updatePathDelay.Value)
			{
				Vector3 target = targetGO.Value.transform.position;
				if (distanceToTarget < minDistance.Value)
					target = this.transform.position - (targetGO.Value.transform.position - this.transform.position).normalized * 100f;
				Seeker seeker = controller.GetComponent<Seeker>();
				isCalculatingPath = true;
				seeker.StartPath(seeker.transform.position, target, ((Pathfinding.Path p) => {currentPath = p; currentPathIndex = 2;isCalculatingPath = false;lastUpdatePath = Time.time;}));
			}
			if (currentPath != null && currentPath.vectorPath.Count > currentPathIndex)
			{
				Vector3 nextTarget = currentPath.vectorPath[currentPathIndex];
				Vector2 difToNextNode = nextTarget - this.transform.position;
				while (difToNextNode.magnitude < 0.1f)
				{
					currentPathIndex++;
					if (currentPathIndex >= currentPath.vectorPath.Count)
					{
						currentPath = null;
						break;
					}
					nextTarget = currentPath.vectorPath[currentPathIndex];
					difToNextNode = nextTarget - this.transform.position;
				}
				if (distanceToTarget < minDistance.Value || distanceToTarget > maxDistance.Value)
				{
					controller.velocity = (nextTarget - this.transform.position).normalized * speed.Value;
					if (isMovingAround)
					{
						StopCoroutine("ChangeMoveAroundDirection");
						isMovingAround = false;
					}
				}
				else if (moveAroundWhenReached.Value)
				{
					if (!isMovingAround)
					{
						StartCoroutine("ChangeMoveAroundDirection");
						isMovingAround = true;
					}
					controller.velocity = Vector3.Cross(difToTarget, isMovingAroundTrigo ? Vector3.forward : Vector3.back);
				}
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

	IEnumerator ChangeMoveAroundDirection()
	{
		yield return new WaitForSeconds(0.5f * timePingPongMoveAround.Value);
		while(true)
		{
			isMovingAroundTrigo = !isMovingAroundTrigo;
			yield return new WaitForSeconds(timePingPongMoveAround.Value);
		}
	}
}
