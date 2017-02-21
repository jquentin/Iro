using System.Collections;
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

	[Command]
	void CmdChangeHue(float value)
	{
		float h, s, v;
		Color.RGBToHSV(this.color, out h, out s, out v);
		h += value % 1f;
		this.color = Color.HSVToRGB(h, 1f, 1f);
		RpcUpdateSelector(h);
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

}
