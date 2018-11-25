using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SlicedSpriteRenderer : MonoBehaviour {

	public string sortingLayerName;
	public int sortingOrder;
	public Color color;

	public int sortingOrderCenter;
	public Color colorCenter;

	public enum Slice { TopLeft, Top, TopRight, Right, LowRight, Low, LowLeft, Left, Center }

	public Sprite topLeft;
	public Sprite top;
	public Sprite topRight;
	public Sprite right;
	public Sprite lowRight;
	public Sprite low;
	public Sprite lowLeft;
	public Sprite left;
	public Sprite center;

	public float scaleMultiplier = 1f;
	float oldScaleMultiplier = 1f;

	Bounds oldBounds;

	public Bounds bounds;

	Sprite GetSprite(Slice slice)
	{
		switch (slice)
		{
		case Slice.TopLeft:
			return topLeft;
		case Slice.Top:
			return top;
		case Slice.TopRight:
			return topRight;
		case Slice.Right:
			return right;
		case Slice.LowRight:
			return lowRight;
		case Slice.Low:
			return low;
		case Slice.LowLeft:
			return lowLeft;
		case Slice.Left:
			return left;
		case Slice.Center:
		default:
			return center;
		}
	}
	Vector3 GetLocalPosition(Slice slice)
	{
		switch (slice)
		{
		case Slice.TopLeft:
			return new Vector3(bounds.min.x / 2f, bounds.max.y / 2f);
		case Slice.Top:
			return new Vector3(0f, bounds.max.y / 2f);
		case Slice.TopRight:
			return new Vector3(bounds.max.x / 2f, bounds.max.y / 2f);
		case Slice.Right:
			return new Vector3(bounds.max.x / 2f, 0f);
		case Slice.LowRight:
			return new Vector3(bounds.max.x / 2f, bounds.min.y / 2f);
		case Slice.Low:
			return new Vector3(0f, bounds.min.y / 2f);
		case Slice.LowLeft:
			return new Vector3(bounds.min.x / 2f, bounds.min.y / 2f);
		case Slice.Left:
			return new Vector3(bounds.min.x / 2f, 0f);
		case Slice.Center:
		default:
			return Vector3.zero;
		}
	}
	Vector3 GetLocalScale(Slice slice)
	{
		switch (slice)
		{
		case Slice.TopLeft:
			return Vector3.one;
		case Slice.Top:
			return new Vector3((bounds.extents.x * scaleMultiplier - 2f) , 1f);
		case Slice.TopRight:
			return Vector3.one;
		case Slice.Right:
			return new Vector3(1f, (bounds.extents.y * scaleMultiplier - 2f));
		case Slice.LowRight:
			return Vector3.one;
		case Slice.Low:
			return new Vector3((bounds.extents.x * scaleMultiplier - 2f), 1f);
		case Slice.LowLeft:
			return Vector3.one;
		case Slice.Left:
			return new Vector3(1f, (bounds.extents.y * scaleMultiplier - 2f));
		case Slice.Center:
		default:
			return new Vector3((bounds.extents.x * scaleMultiplier - 2f), (bounds.extents.y * scaleMultiplier - 2f));
		}
	}

	void Update () 
	{
		if (bounds == oldBounds && scaleMultiplier == oldScaleMultiplier)
			return;

		transform.DestroyChildrenImmediate();

		CreateSide(Slice.TopLeft);
		CreateSide(Slice.Top);
		CreateSide(Slice.TopRight);
		CreateSide(Slice.Right);
		CreateSide(Slice.LowRight);
		CreateSide(Slice.Low);
		CreateSide(Slice.LowLeft);
		CreateSide(Slice.Left);
		CreateSide(Slice.Center);

		oldBounds = bounds;
		oldScaleMultiplier = scaleMultiplier;
	}

	void CreateSide(Slice slice)
	{
		GameObject go = new GameObject(slice.ToString());
		SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
		sr.sprite = GetSprite(slice);
		sr.sortingOrder = (slice == Slice.Center) ? sortingOrderCenter : sortingOrder;
		sr.sortingLayerName = sortingLayerName;
		sr.color = (slice == Slice.Center) ? colorCenter : color;
		go.transform.parent = this.transform;
		go.transform.localPosition = GetLocalPosition(slice);
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = GetLocalScale(slice);

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), new Vector3(bounds.max.x, bounds.max.y));
		Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.max.y), new Vector3(bounds.max.x, bounds.min.y));
		Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y), new Vector3(bounds.min.x, bounds.min.y));
		Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y), new Vector3(bounds.min.x, bounds.max.y));
	}
}
