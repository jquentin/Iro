using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NGUIUtils
{

	//used in fullscreen button in title scene to check if the user is pressing the settings panel or not..
	public static bool isCursorOverOverlayUI(){
		
		foreach (UICamera uiCam in GameObject.FindObjectsOfType<UICamera>())
			if (isCursorOverOverlayUI (uiCam.GetComponent<Camera> ()))
				return true;
		return false;
		
	}
	
	public static bool isCursorOverOverlayUI(Camera cam){
		//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)){
			return true;
		}else{
			return false;
		}
		
	}

	public static bool isCursorOverOverlayUI(out List<GameObject> pickedObjects)
	{
		pickedObjects = new List<GameObject>();
		foreach (UICamera uiCam in GameObject.FindObjectsOfType<UICamera>())
		{
			List<GameObject> camPickedObjects;
			if (isCursorOverOverlayUI (uiCam.GetComponent<Camera> (), out camPickedObjects))
				pickedObjects.AddRange(camPickedObjects);
		}
		return pickedObjects.Count > 0;
	}

	public static bool isCursorOverOverlayUI(Camera cam, out List<GameObject> pickedObjects)
	{
		pickedObjects = new List<GameObject>();
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			pickedObjects.Add(hit.collider.gameObject);
		return pickedObjects.Count > 0;
	}

}
