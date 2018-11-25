using UnityEngine;
using System.Collections;

public class AnimatorDestroy : MonoBehaviour {


	public void DestroyObjectByName(string objectName)
	{
		Transform t = transform.Find(objectName, true);
		if (t != null)
			Destroy(t.gameObject);
	}
}
