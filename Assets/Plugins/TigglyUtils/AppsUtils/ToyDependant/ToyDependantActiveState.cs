using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ToyDependantActiveState : MonoBehaviour {

	[Serializable]
	class ToyGameObjectCombination
	{
		public TigglyConstants.Toys toy;
		public GameObject gameObject;
	}

	[SerializeField]
	private List<ToyGameObjectCombination> objectsToActivate;

	void Start () 
	{
		foreach(ToyGameObjectCombination comb in objectsToActivate)
			comb.gameObject.SetActive(TigglyConstants.instance != null && comb.toy == TigglyConstants.instance.mainToys);
	}

}
