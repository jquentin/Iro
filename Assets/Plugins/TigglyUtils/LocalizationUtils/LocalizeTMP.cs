//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

#if TMProFadeEnabled

using UnityEngine;
using TMPro;

/// <summary>
/// Simple script that lets you localize a UIWidget.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshPro))]
public class LocalizeTMP : MonoBehaviour
{
	/// <summary>
	/// Localization key.
	/// </summary>
	
	public string key;
	
	/// <summary>
	/// Manually change the value of whatever the localization component is attached to.
	/// </summary>
	
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				TextMeshPro lbl = GetComponent<TextMeshPro>();
				
				if (lbl != null)
				{
					lbl.text = value;
					#if UNITY_EDITOR
					if (!Application.isPlaying) NGUITools.SetDirty(lbl);
					#endif
				}
			}
		}
	}
	
	bool mStarted = false;
	
	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>
	
	void OnEnable ()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying) return;
		#endif
		if (mStarted) OnLocalize();
	}
	
	/// <summary>
	/// Localize the widget on start.
	/// </summary>
	
	void Start ()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying) return;
		#endif
		mStarted = true;
		OnLocalize();
		Localization.OnLanguageChanged += OnLocalize;
	}
	
	void OnDestroy ()
	{
		Localization.OnLanguageChanged -= OnLocalize;
	}
	
	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>
	
	void OnLocalize ()
	{
		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(key))
		{
			TextMeshPro lbl = GetComponent<TextMeshPro>();
			if (lbl != null) key = lbl.text;
		}
		
		// If we still don't have a key, leave the value as blank
		if (!string.IsNullOrEmpty(key)) value = Localization.Get(key);
	}
}

#endif
