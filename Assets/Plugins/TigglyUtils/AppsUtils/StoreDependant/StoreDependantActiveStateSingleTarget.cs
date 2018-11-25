using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StoreDependantActiveStateSingleTarget : MonoBehaviour {

	public GameObject target;
	public enum ActivationMode { ActivateIfInStoreList, UnactivateIfInStoreList }
	public ActivationMode activationMode;
	public List<StoreSelection.Store> storeList;

	void Awake () 
	{
		bool listContainsCurrentStore = storeList.Contains(StoreSelection.store);
		if (activationMode == ActivationMode.ActivateIfInStoreList)
		{
			if (!listContainsCurrentStore)
				target.SetActive(false);
		}
		else
		{
			if (listContainsCurrentStore)
				target.SetActive(false);
		}
	}

	[ContextMenu("SetTargetToSelf")]
	public void SetTargetToSelf()
	{
		target = this.gameObject;
	}
}
