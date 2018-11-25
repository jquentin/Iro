using UnityEngine;
using System.Collections;

public class ChangeLayer : MonoBehaviour {

	public void SwitchToLayer(int o, int layer)
	{
		gameObject.layer = layer;
	}

	public void SwitchObjectToLayer(string childNameAndLayerName)
	{
		string[] split = childNameAndLayerName.Split(';');
		Transform t = transform.FindChildren(split[0])[0];
		t.gameObject.layer = LayerMask.NameToLayer(split[1]);
	}
	public void SwitchObjectToLayerRecursive(string childNameAndLayerName)
	{
		string[] split = childNameAndLayerName.Split(';');
		Transform t = transform.FindChildren(split[0])[0];
		t.gameObject.layer = LayerMask.NameToLayer(split[1]);
		foreach ( Transform child in t.GetChildrenList(true))
			child.gameObject.layer = LayerMask.NameToLayer(split[1]);
	}
}
