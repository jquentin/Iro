using UnityEngine;
using System.Collections;

public static class AnimatorExtensions
{

	public static IEnumerator PlayAsCoroutine(this Animator animator, string stateName)
	{
		animator.Play(stateName);
		yield return new WaitForEndOfFrame();
		yield return new WaitUntil( 
			() => 
			{
				AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
				return (!stateInfo.IsName(stateName) || stateInfo.normalizedTime >= 1f);
			});
	}
}
