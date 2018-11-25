using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Random spawn area implementation using Transforms as bounds.
/// Used in Submarine but works only in specific conditions.
/// It has been left here only for retro-compatibility in Submarine, but
/// shouldn't be used in future code.
/// </summary>
[Obsolete("Deprecated. Use RandomSpawnAreaCollider2D instead.")]
public class RandomSpawnAreaTransformBounds : RandomSpawnAreaMonoBehavior {

	public List<Transform> bounds
	{
		get
		{
			return transform.GetChildrenList();
		}
	}
	
	void OnDrawGizmos ()
	{
		Gizmos.color = Color.green;
		for(int i = 0 ; i < bounds.Count ; i++)
		{
			int a = i;
			int b = (i + 1) % bounds.Count;
			Gizmos.DrawLine(bounds[a].position, bounds[b].position);
		}
	}

	public override Vector3 PickRandomPosition(bool local = false)
	{
		float totalWeight = 0f;
		List<float> weights = new List<float>();
		Vector3 minCoords = Vector3.zero;
		for (int i = 0 ; i < bounds.Count ; i++)
		{
			Vector3 pos = local ? bounds[i].localPosition : bounds[i].position;
			minCoords = new Vector3(Mathf.Min(minCoords.x, pos.x), Mathf.Min(minCoords.y, pos.y), Mathf.Min(minCoords.z, pos.z));
			float weight = UnityEngine.Random.Range(0f, 1f);
			weights.Add(weight);
			totalWeight += weight;
		}
		Vector3 res = Vector3.zero;
		for (int i = 0 ; i < bounds.Count ; i++)
		{
			Vector3 pos = local ? bounds[i].localPosition : bounds[i].position;
			pos -= minCoords;
			res += pos * weights[i] / totalWeight;
		}
		res += minCoords;
		return res;
	}

	[ContextMenu("TestSpawn")]
	public void TestSpawn()
	{
		GameObject go = new GameObject("TestSpawn");
		go.transform.position = PickRandomPosition();
		go.transform.parent = transform;
	}

}
