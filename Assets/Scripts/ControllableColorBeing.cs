﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControllableColorBeing : ColorBeing {

	public float colorWheelSpeed = 1f;

	ColorWheel _colorWheel;
	public ColorWheel colorWheel
	{
		get
		{
			if (_colorWheel == null)
				_colorWheel = GetComponentInChildren<ColorWheel>();
			return _colorWheel;
		}
	}

	protected override void Start ()
	{
		base.Start ();
		CloseColorWheel();
	}

	[ClientRpc]
	void RpcUpdateSelector(float hue)
	{
		colorWheel.UpdateSelector(hue);
	}

	protected override void Update()
	{
		base.Update();
		if (!isLocalPlayer)
			return;
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (mouseWheel != 0f)
		{
			if (!colorWheel.isShowing)
				colorWheel.Show();
			CmdChangeHue(colorWheelSpeed * mouseWheel);
			this.AddTimeout(0.4f, HueIsStill, CloseColorWheel, true);
		}
		hueIsChanging = (mouseWheel != 0f);
	}

	bool hueIsChanging = false;
	bool HueIsStill()
	{
		return !hueIsChanging;
	}

	void CloseColorWheel()
	{
		colorWheel.Hide();
	}

	protected override void OnColorChangedVirtual (Color color)
	{
		base.OnColorChangedVirtual (color);
		RpcUpdateSelector(color.GetHue());
	}

}
