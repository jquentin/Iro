using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButton : MonoBehaviour {

	public GameObject selected;

	public void Select()
	{
		selected.SetActive(true);
	}

	public void Unselect()
	{
		selected.SetActive(false);
	}
}
