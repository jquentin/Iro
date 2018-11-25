using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StoreDependantActiveState : MonoBehaviour {
	
	[Serializable]
	class StoreGameObjectCombination
	{
		public StoreSelection.Store store;
		public GameObject gameObject;
	}
	
	[SerializeField]
	private List<StoreGameObjectCombination> objectsToActivate;
	
	void Start () 
	{
		foreach(StoreGameObjectCombination comb in objectsToActivate)
			comb.gameObject.SetActive(comb.store == StoreSelection.store);
	}
}
