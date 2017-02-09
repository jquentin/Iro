using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : TigglyMonoBehaviour {

	ControllableColorBeing _owner;
	ControllableColorBeing owner
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

	protected abstract void Shoot();

}
