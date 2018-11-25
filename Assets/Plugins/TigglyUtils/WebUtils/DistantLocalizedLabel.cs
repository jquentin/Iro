using UnityEngine;
using System.Collections;

public class DistantLocalizedLabel : DistantLabel {

	public string targetURLPrefix;

	protected override string targetURL {
		get 
		{
			return targetURLPrefix + Localization.Get("Language_Prefix") + textURL;
		}
	}

	protected override string resourcesPath {
		get 
		{
			return Localization.Get("Language_Prefix") + defaultResourcesPath;
		}
	}
	
	
	protected override string actualCacheKey {
		get 
		{
			return Localization.Get("Language_Prefix") + cacheKey;
		}
	}
	
	[ContextMenu("Clear cached text")]
	public override void ClearCachedText ()
	{
		PlayerPrefs.DeleteKey("en/" + cacheKey);
		PlayerPrefs.DeleteKey("fr/" + cacheKey);
		PlayerPrefs.DeleteKey("de/" + cacheKey);
		PlayerPrefs.DeleteKey("nl/" + cacheKey);
		PlayerPrefs.DeleteKey("es/" + cacheKey);
		PlayerPrefs.DeleteKey("it/" + cacheKey);
		PlayerPrefs.DeleteKey("po/" + cacheKey);
		PlayerPrefs.DeleteKey("ru/" + cacheKey);
		PlayerPrefs.DeleteKey("ar/" + cacheKey);
		PlayerPrefs.DeleteKey("ko/" + cacheKey);
		PlayerPrefs.DeleteKey("cn/" + cacheKey);
		PlayerPrefs.DeleteKey("jp/" + cacheKey);
	}

}
