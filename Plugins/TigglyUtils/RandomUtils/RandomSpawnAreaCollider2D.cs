using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Random spawn area using a collider2d as area.
/// This will pick a random position with a uniform distribution.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RandomSpawnAreaCollider2D : RandomSpawnAreaMonoBehavior
{

	Collider2D _collider2D;
	Collider2D collider2D
	{
		get
		{
			if (_collider2D == null)
				_collider2D = GetComponent<Collider2D>();
			return _collider2D;
		}
	}

	public List<Collider2D> excludedAreas = new List<Collider2D>();

	bool IsInArea(Vector3 pos, bool local)
	{
		Vector3 worldPos;
		if (local)
			worldPos = transform.TransformPoint(pos);
		else
			worldPos = pos;
		bool isInArea = collider2D.OverlapPoint(worldPos);
		if (isInArea)
		{
			// If is also in an excludedArea, return false
			foreach(Collider2D excludeArea in excludedAreas)
				if (excludeArea.OverlapPoint(worldPos))
					return false;
			return true;
		}
		else
		{
			return false;
		}
	}

	public override Vector3 PickRandomPosition(bool local = false)
	{
		// Get the appropriate bounding rect values, local or not
		Bounds bounds = collider2D.bounds;
		if (local)
		{
			bounds.min = transform.InverseTransformPoint(bounds.min);
			bounds.max = transform.InverseTransformPoint(bounds.max);
		}
		Vector2 pos = MathUtils.NaNVector2;
		// Pick a position in the bounding rect, check if it's in the area
		// If it's not, keep trying until it is
		while (pos.IsNaN())
		{
			Vector2 posAttempt = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
			if (IsInArea(posAttempt, local))
				pos = posAttempt;
		}
		return pos;
	}

	[ContextMenu("Test Spawn: World Position")]
	public void TestSpawnWorld()
	{
		GameObject go = new GameObject("TestSpawnWorld");
		go.transform.position = PickRandomPosition(false);
		go.transform.parent = transform;
	}


	[ContextMenu("Test Spawn: Local Position")]
	public void TestSpawnLocal()
	{
		GameObject go = new GameObject("TestSpawnLocal");
		go.transform.parent = transform;
		go.transform.localPosition = PickRandomPosition(true);
	}

}

