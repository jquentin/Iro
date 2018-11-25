using UnityEngine;
using System.Collections;

public class ResetParentPosition : MonoBehaviour {

	public void Reset()
	{
		transform.parent.position = transform.position;
		transform.localPosition = Vector3.zero;
	}
}
