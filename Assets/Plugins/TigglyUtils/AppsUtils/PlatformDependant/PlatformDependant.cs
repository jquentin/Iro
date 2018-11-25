using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformDependant : MonoBehaviour {

	public enum Platform { Unknown, iOS, Android }
	public List<Platform> enabledOnPlatforms = new List<Platform>(){Platform.iOS};

	public static Platform currentPlatform
	{
		get
		{
			
#if UNITY_ANDROID
			return Platform.Android;
#elif UNITY_IOS
			return Platform.iOS;
#else
			return Platform.Unknown;
#endif

		}
	}

	void Awake () 
	{
		if (!enabledOnPlatforms.Contains(currentPlatform))
			gameObject.SetActive(false);
	}


}
