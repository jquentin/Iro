using UnityEngine;
using System.Collections;

public static class LayerMaskUtils
{
	public static bool Includes (this LayerMask layerMask, int layer)
	{
		return ((layerMask.value & (1 << layer)) > 0);
	}

	public static bool Includes (this LayerMask layerMask, GameObject go)
	{
		return layerMask.Includes(go.layer);
	}
}
