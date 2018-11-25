using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class EventListener : MonoBehaviour {

	public delegate void VoidDelegate ();

	public VoidDelegate onClick;

	void OnClick ()					
	{ 
		if (onClick != null) 
			onClick(); 
	}

	void OnETMouseUpAsButton ()
	{
		OnClick();
	}
	
}
