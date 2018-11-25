using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FlipLoop : MonoBehaviour {

	public enum Axis { X, Y }
	public Axis axis;
	public float timeBetweenFlips = 0.2f;

	SpriteRenderer _spriteRenderer;
	SpriteRenderer spriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
				_spriteRenderer = GetComponent<SpriteRenderer>();
			return _spriteRenderer;
		}
	}

	private float lastFlip;

	void Start()
	{
		lastFlip = Time.realtimeSinceStartup;
	}

	void Update () 
	{
		if (Time.realtimeSinceStartup - lastFlip >= timeBetweenFlips)
		{
			if (axis == Axis.X)
				spriteRenderer.flipX = !spriteRenderer.flipX;
			else
				spriteRenderer.flipY = !spriteRenderer.flipY;
			lastFlip = Time.realtimeSinceStartup;
		}
	}
}
