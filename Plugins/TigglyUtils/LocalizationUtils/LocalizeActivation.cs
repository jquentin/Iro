using UnityEngine;
using System.Collections;

public abstract class LocalizeActivation : MonoBehaviour {

	/// <summary>
	/// Localization key.
	/// </summary>
	
	protected abstract string keyToUse {get;}
	
	bool mStarted = false;
	
	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>
	
	protected void OnEnable ()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying) return;
		#endif
		if (mStarted) OnLocalize();
	}
	
	/// <summary>
	/// Localize the widget on start.
	/// </summary>
	
	protected void Start ()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying) return;
		#endif
		mStarted = true;
		OnLocalize();
		Localization.OnLanguageChanged += OnLocalize;
	}
	
	protected void OnDestroy ()
	{
		Localization.OnLanguageChanged -= OnLocalize;
	}
	
	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>
	
	protected void OnLocalize ()
	{
		string nameChild;
		if (string.IsNullOrEmpty(keyToUse)) 
			return;
		nameChild = Localization.Get(keyToUse);
		if (string.IsNullOrEmpty(nameChild)) 
			return;
		for(int i = 0 ; i < transform.childCount ; i++)
		{
			Transform child = transform.GetChild(i);
			child.gameObject.SetActive(child.name == nameChild);
		}
	}
}
