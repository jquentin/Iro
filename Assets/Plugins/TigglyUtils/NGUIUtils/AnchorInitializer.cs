using UnityEngine;
using System.Collections;

public static class UIWidgetAdditions
{
	public static void SetAnchorRelativeValues(this UIWidget widget, float leftAnchor, float rightAnchor, float bottomAnchor, float topAnchor)
	{
		widget.leftAnchor.relative = leftAnchor;
		widget.rightAnchor.relative = rightAnchor;
		widget.bottomAnchor.relative = bottomAnchor;
		widget.topAnchor.relative = topAnchor;
	}
}

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
/// <summary>
/// Use this class to automatically initialize the anchors of a UIWidget.
/// For use on a prefab meant to be added to a UIRoot, so it can be properly linked with the root automatically.
/// For now it always anchors to fill the screen, but options could be added.
/// </summary>
public class AnchorInitializer : MonoBehaviour {

	public enum Type { WholeScreen, TopLeft, TopRight, BottomLeft, BottomRight }
	public Type type = Type.WholeScreen;

	UIWidget _widget;
	UIWidget widget
	{
		get
		{
			if (_widget == null)
				_widget = GetComponent<UIWidget>();
			return _widget;
		}
	}

	void Update () 
	{
		if (widget == null || widget.isAnchored)
			return;
		Initialize();
	}

	[ContextMenu("Update Anchor")]
	void Initialize()
	{
		UICamera uiCam = NGUITools.GetRoot(gameObject).GetComponentInChildren<UICamera>();
		if (uiCam == null)
			uiCam = NGUITools.FindCameraForLayer(gameObject.layer).GetComponent<UICamera>();
		if (uiCam == null)
		{
			Debug.LogError("Could not find UICamera component to initialize the Anchor");
			return;
		}
		widget.SetAnchor(uiCam.gameObject);
		switch(type)
		{
		case Type.TopLeft:
			widget.SetAnchorRelativeValues(0f, 0f, 1f, 1f);
			break;
		case Type.TopRight:
			widget.SetAnchorRelativeValues(1f, 1f, 1f, 1f);
			break;
		case Type.BottomLeft:
			widget.SetAnchorRelativeValues(0f, 0f, 0f, 0f);
			break;
		case Type.BottomRight:
			widget.SetAnchorRelativeValues(1f, 1f, 0f, 0f);
			break;
		case Type.WholeScreen:
		default:
			widget.SetAnchorRelativeValues(0f, 1f, 0f, 1f);
			break;
		}
		widget.leftAnchor.absolute = widget.rightAnchor.absolute = widget.bottomAnchor.absolute = widget.topAnchor.absolute = 0;
		transform.localScale = Vector3.one;
	}

}
