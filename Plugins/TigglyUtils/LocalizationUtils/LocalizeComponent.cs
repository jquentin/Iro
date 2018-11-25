using UnityEngine;
using System.Collections;

public abstract class LocalizeComponent : MonoBehaviour 
{

	/// <summary>
	/// Localization key.
	/// </summary>
	public string key;

	/// <summary>
	/// Manually change the value of whatever the localization component is attached to.
	/// </summary>

	public abstract string value
	{
		set;
	}

	protected virtual bool fixArabic
	{
		get
		{
			return true;
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
		if (!string.IsNullOrEmpty(key)) value = Localization.Get(key, fixArabic);
	}

}
