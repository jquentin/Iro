using UnityEngine;
using System.Collections;

public class RandomDirection : MonoBehaviour {
	
	void Awake () 
	{
		if (Random.Range(0f, 1f) > 0.5f)
		{
			Vector3 scale = transform.localScale;
			scale.x = -scale.x;
			transform.localScale = scale;
		}
	}

}
