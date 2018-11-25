using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * This component should be added to a UGUI button
 * that is to be considered clicked when the Android back button is pressed
 **/
public class BackButtonUGUIButtonClick : BackButtonHandler {

	UnityEngine.UI.Button _button;
	UnityEngine.UI.Button button
	{
		get
		{
			if (_button == null)
				_button = GetComponent<UnityEngine.UI.Button>();
			return _button;
		}
	}

	protected override bool isSelectable 
	{
		get 
		{
			return (button != null && button.enabled && button.interactable);
		}
	}

	protected override void Trigger ()
	{
		if (button != null)
			button.onClick.Invoke();
	}

}
