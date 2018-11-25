using UnityEngine;
using System.Collections;

/**
 * This component should be added to any button (NGUI or using Unity events OnMouse..., or using other custom events) 
 * that is to be considered clicked when the Android back button is pressed
 **/
public class BackButtonClickButton : BackButtonHandler {

	public enum MessageType { Custom, OnClick, OnMouseUpAsButton, OnMouseDown, OnMouseUp }
	public MessageType messageType;

	public string customMessage;

	override protected bool isSelectable {
		get {

			Collider c = GetComponent<Collider>();
			Collider2D c2D = GetComponent<Collider2D>();
			return (c != null && c.enabled || c2D != null && c2D.enabled);
		}
	}

	override protected void Trigger()
	{
		switch(messageType)
		{
		case MessageType.Custom:
			if (!string.IsNullOrEmpty(customMessage))
				SendMessage(customMessage, SendMessageOptions.DontRequireReceiver);	
			break;
		case MessageType.OnClick:
			SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			break;
		case MessageType.OnMouseDown:
			SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
			break;
		case MessageType.OnMouseUp:
			SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
			break;
		case MessageType.OnMouseUpAsButton:
		default:
			SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
			break;
		}
	}
}
