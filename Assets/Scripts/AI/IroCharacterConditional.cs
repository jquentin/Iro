using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public abstract class IroCharacterConditional : Conditional {

	PlayerController _controller;
	protected PlayerController controller
	{
		get
		{
			if (_controller == null)
				_controller = GetComponent<PlayerController>();
			return _controller;
		}
	}

	ColorBeing _being;
	protected ColorBeing being
	{
		get
		{
			if (_being == null)
				_being = GetComponent<ColorBeing>();
			return _being;
		}
	}

}
