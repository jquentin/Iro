using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : TigglyNetworkBehaviour {

	ColorBeing _owner;
	protected ColorBeing owner
	{
		get
		{
			if (_owner == null)
				_owner = GetComponentInParent<ColorBeing>();
			return _owner;
		}
	}

	PlayerController _controller;
	protected PlayerController controller
	{
		get
		{
			if (_controller == null)
				_controller = GetComponentInParent<PlayerController>();
			return _controller;
		}
	}

	protected Color color
	{
		get
		{
			return owner.color;
		}
	}

	public virtual bool isUpgraded { get { return false; } }

}
