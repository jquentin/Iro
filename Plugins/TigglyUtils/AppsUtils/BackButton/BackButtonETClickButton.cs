using UnityEngine;
using System.Collections;

/**
 * This component should be added to any Easy Touch button (using the EasyTouchFacade messages) 
 * that should be considered clicked when the Android back button is pressed
 **/
public class BackButtonETClickButton : BackButtonHandler {

	public enum MessageType { OnETMouseDown, OnETMouseUp, OnETMouseUpAsButton }
	public MessageType messageType;

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
		case MessageType.OnETMouseDown:
			SendMessage("OnETMouseDown", new Gesture(), SendMessageOptions.DontRequireReceiver);
			break;
		case MessageType.OnETMouseUp:
			SendMessage("OnETMouseUp", new Gesture(), SendMessageOptions.DontRequireReceiver);
			break;
		case MessageType.OnETMouseUpAsButton:
		default:
			SendMessage("OnETMouseUpAsButton", new Gesture(), SendMessageOptions.DontRequireReceiver);
			break;
		}
	}
}
