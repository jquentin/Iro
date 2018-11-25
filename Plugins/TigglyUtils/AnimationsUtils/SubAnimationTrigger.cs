using UnityEngine;
using System.Collections;

public class SubAnimationTrigger : MonoBehaviour {

	public void TriggerSubAnimation(string triggerName)
	{
		foreach(Animator a in GetComponentsInChildren<Animator>())
			a.SetTrigger(triggerName);
	}
}
