using UnityEngine;
using System.Collections;

public class DistantLabel : MonoBehaviour {

	public string textURL;
	public string cacheKey;
	public string defaultResourcesPath;

	private UILabel _label;
	protected UILabel label
	{
		get
		{
			if (_label == null)
				_label = GetComponent<UILabel>();
			return _label;
		}
	}

	protected string text
	{
		get
		{
			return label.text;
		}
		set
		{
			label.text = value;
		}
	}

	protected virtual string targetURL
	{
		get
		{
			return textURL;
		}
	}

	protected virtual string resourcesPath
	{
		get
		{
			return defaultResourcesPath;
		}
	}
	
	protected virtual string actualCacheKey
	{
		get
		{
			return cacheKey;
		}
	}

	void OnEnable()
	{
		UpdateWithCacheInPlayerPrefs(targetURL, actualCacheKey, resourcesPath);
	}

	public void UpdateWithCacheInPlayerPrefs(string targetURL, string cacheKey, string resourcesPath)
	{
		string cachedHtml = PlayerPrefs.GetString(cacheKey, "");
		if (string.IsNullOrEmpty(cachedHtml))
		{
			TextAsset ta = Resources.Load(resourcesPath) as TextAsset;
			if (ta == null)
				Debug.LogWarning("Missing default text in resources at: " + resourcesPath);
			else
				cachedHtml = ta.text;
		}
		
		Show(targetURL, cachedHtml, cacheKey);
	}
	
	public void Show(string targetURL, string cachedHtml = "", string cacheKey = "")
	{
		text = cachedHtml;
		if (InternetReachabilityChecker.instance.isInternetReachable)
		{
			if (!string.IsNullOrEmpty(cacheKey))
				StartCoroutine(LoadOnlineText(targetURL, cacheKey));
		}
	}
	
	IEnumerator LoadOnlineText(string targetURL, string cacheKey)
	{
		WWW www = new WWW(targetURL);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			text = www.text;
			PlayerPrefs.SetString(cacheKey, www.text);
		}
	}

	[ContextMenu("Clear cached text")]
	public virtual void ClearCachedText()
	{
		PlayerPrefs.DeleteKey(cacheKey);
	}
}
