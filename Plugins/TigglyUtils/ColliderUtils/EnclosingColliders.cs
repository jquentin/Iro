using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnclosingColliders : MonoBehaviour
{

	public float thickness = 1f;
	public float zThickness = 1f;

	public bool useColliders2D = false;

	public const string PARENT_NAME = "EnclosingColliders_auto";
	public const string MESH_NAME = "MeshCollider_auto";

	[ContextMenu("CreateEnclosingColliders")]
	public void CreateEnclosingColliders()
	{
//		PolygonCollider2D polygonCollider = Selection.activeGameObject.GetComponent<PolygonCollider2D>();

		RemoveEnclosingColliders();

		PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
		if (polygonCollider == null)
		{
			Debug.LogError("You need a polygon collider on this game object");
			return;
		}
		GameObject parentGO = new GameObject(PARENT_NAME);
		parentGO.layer = gameObject.layer;
		Transform parent = parentGO.transform;
		parent.parent = transform;
		parent.localPosition = Vector3.zero;
		parent.localScale = Vector3.one;
		for(int i = 0 ; i < polygonCollider.points.Length ; i++)
		{
			Vector2 a = polygonCollider.points[i];
			Vector2 b = polygonCollider.points[(i + 1) % polygonCollider.points.Length];
			GameObject go = new GameObject("Collider " + i);
			go.layer = gameObject.layer;
			go.transform.parent = parent;
			if (useColliders2D)
			{
				BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
				bc.offset = new Vector2(0f, -0.5f);
			}
			else
			{
				BoxCollider bc = go.AddComponent<BoxCollider>();
				bc.center = new Vector3(0f, -0.5f, 0f);
			}
			go.transform.localPosition = (a + b) * 0.5f;
			go.transform.localScale = new Vector3((a - b).magnitude, thickness, zThickness);
			go.transform.rotation = Quaternion.Euler(Mathf.Rad2Deg * Vector3.forward * Mathf.Atan2((b-a).y, (b-a).x));
		}
	}
	[ContextMenu("RemoveEnclosingColliders")]
	public void RemoveEnclosingColliders()
	{
		Transform parent = transform.Find(PARENT_NAME);
		while ( parent != null)
		{
			DestroyImmediate(parent.gameObject);
			parent = transform.Find(PARENT_NAME);
		}
	}


	[ContextMenu("CreateMeshCollider")]
	public void CreateMeshCollider()
	{
		PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
		if (polygonCollider == null)
		{
			Debug.LogError("You need a polygon collider on this game object");
			return;
		}
		GameObject parentGO = new GameObject(MESH_NAME);
		parentGO.layer = gameObject.layer;
		Transform parent = parentGO.transform;
		parent.parent = transform;
		parent.localPosition = Vector3.zero;
		parent.localScale = Vector3.one;
		MeshFilter meshFilter = parentGO.AddComponent<MeshFilter>();
		int polyLength = polygonCollider.points.Length;
		Vector3[] vertices = new Vector3[polyLength * 2];
		int[] triangles = new int[polyLength * 6 + (polyLength - 2) * 3 * 2];
		for(int i = 0 ; i < polyLength ; i++)
		{
			vertices[i * 2] = new Vector3(polygonCollider.points[i].x, polygonCollider.points[i].y, zThickness * 0.5f);
			vertices[i * 2 + 1] = new Vector3(polygonCollider.points[i].x, polygonCollider.points[i].y, - zThickness * 0.5f);
			triangles[i * 6] = i * 2;
			triangles[i * 6 + 1] = i * 2 + 1;
			triangles[i * 6 + 2] = (i * 2 + 2) % (polyLength * 2);
			triangles[i * 6 + 3] = (i * 2 + 2) % (polyLength * 2);
			triangles[i * 6 + 4] = i * 2 + 1;
			triangles[i * 6 + 5] = (i * 2 + 3) % (polyLength * 2);
		}
		for(int i = 0 ; i < polyLength - 2 ; i++)
		{
			triangles[polyLength * 6 + i * 6] = 0;
			triangles[polyLength * 6 + i * 6 + 1] = (i + 1) * 2;
			triangles[polyLength * 6 + i * 6 + 2] = (i + 2) * 2;
			triangles[polyLength * 6 + i * 6 + 3] = 1;
			triangles[polyLength * 6 + i * 6 + 4] = (i + 2) * 2 + 1;
			triangles[polyLength * 6 + i * 6 + 5] = (i + 1) * 2 + 1;
		}
		Debug.Log(string.Format("points : {0} ; triangles : {1}", vertices.ToList().ListToString(), triangles.ToList().ListToString()));
		meshFilter.mesh = new Mesh();
		meshFilter.mesh.vertices = vertices;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.RecalculateBounds(); 
		meshFilter.mesh.RecalculateNormals(); 
		MeshCollider mc = parentGO.AddComponent<MeshCollider>();
		mc.convex = true;
		mc.isTrigger = polygonCollider.isTrigger;
	}

}
