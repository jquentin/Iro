using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BilingualLocalizableAudioClip : LocalizableAudioClip {

	// Temporary fix For using secondary language functionality --------------------------------------------------------------
	
	private bool alreadyTriedLoading2 = false;
	private AudioClip _clip2;
	public AudioClip clip2
	{
		get
		{
			if (_clip2 == null && !alreadyTriedLoading2)
			{
				alreadyTriedLoading2 = true;
				if (string.IsNullOrEmpty(key))
				{
					Debug.LogWarning("LocalizableAudioClip empty key");
					return null;
				}
				string name;
				// if there is no value for this key, try with adding the language's suffix at the end of the key
				if (Localization.Exists(key))
					name = SecondaryLocalization.Get(key);
				else
					name = SecondaryLocalization.Get("Language_Prefix") + key;
				if (string.IsNullOrEmpty(name))
				{
					Debug.LogWarning("LocalizableAudioClip key not found in dictionary");
					return null;
				}
				
				_clip2 = Resources.Load(name) as AudioClip;				
				if (_clip2 == null)
				{
					Debug.LogWarning("LocalizableAudioClip clip not found in Resources:" + name);
				}
				else
				{
					SecondaryLocalization.OnLanguageChanged += Reset;
				}
			}
			
			return _clip2;
		}
	}

	protected override void Reset ()
	{
		base.Reset ();
		_clip2 = null;
		alreadyTriedLoading2 = false;
	}

	public BilingualLocalizableAudioClip(string key)
	{
		this.key = key;
	}
	
	public BilingualLocalizableAudioClip(AudioClip clip)
	{
		this._clip = clip;
	}

}

public static class BilingualLocalizableAudioClipExtensions
{
	
	public static void PlayOneShotControlledSecondary(this AudioSource source, BilingualLocalizableAudioClip clip)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		source.PlayOneShotControlledSecondary(clip, source.GetAudioType());
	}
	
	// Temporary solution For secondary language functionality
	public static void PlayOneShotControlledSecondary(this AudioSource source, BilingualLocalizableAudioClip clip, AudioType type)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		if (!type.IsPlayable())
			return;
		//		#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
		//		source.spatialBlend = 0f;
		//		#endif
		source.PlayOneShot(clip.clip2);
	}
}
