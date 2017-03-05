using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Weapon : TigglyNetworkBehaviour {

	ControllableColorBeing _owner;
	protected ControllableColorBeing owner
	{
		get
		{
			if (_owner == null)
				_owner = GetComponentInParent<ControllableColorBeing>();
			return _owner;
		}
	}

	protected Color color
	{
		get
		{
			return owner.color;
		}
	}

}
