using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LocalizableAudioClip {

	public string key;
	protected bool alreadyTriedLoading = false;
	protected AudioClip _clip;
	public virtual AudioClip clip
	{
		get
		{
			if (_clip == null && !alreadyTriedLoading)
			{
				alreadyTriedLoading = true;
				if (string.IsNullOrEmpty(key))
				{
					Debug.LogWarning("LocalizableAudioClip empty key");
					return null;
				}
				string name;
				// if there is no value for this key, try with adding the language's suffix at the end of the key
				if (Localization.Exists(key))
					name = Localization.Get(key);
				else
					name = Localization.Get("Language_Prefix") + key;
				if (string.IsNullOrEmpty(name))
				{
					Debug.LogWarning("LocalizableAudioClip key not found in dictionary");
					return null;
				}
				
				_clip = Resources.Load(name) as AudioClip;				
				if (_clip == null)
				{
					Debug.LogWarning("LocalizableAudioClip clip not found in Resources:" + name);
				}
				else
				{
//					LanguageManager.OnLanguageChanged += Reset;
				}
			}
			
			return _clip;
		}
	}

	public float length
	{
		get
		{
			if (clip != null)
				return clip.length;
			else
				return 1f;
		}
	}
	
	public float GetLength(float defaultValue)
	{
		if (clip != null)
			return clip.length;
		else
			return defaultValue;
	}
	
	public LocalizableAudioClip()
	{
	}
	
	
	public LocalizableAudioClip(string key)
	{
		this.key = key;
	}
	
	public LocalizableAudioClip(AudioClip clip)
	{
		this._clip = clip;
	}
	
	
	protected virtual void Reset()
	{
//		LanguageManager.OnLanguageChanged -= Reset;
		_clip = null;
		alreadyTriedLoading = false;
	}
	
	protected virtual void OnDestroy()
	{
//		LanguageManager.OnLanguageChanged -= Reset;
	}
}

public static class LocalizableAudioClipExtensions
{
	
	public static void PlayOneShotControlled(this AudioSource source, List<LocalizableAudioClip> clips)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		source.PlayOneShotControlled(clips, source.GetAudioType());
	}
	
	public static void PlayOneShotControlled(this AudioSource source, List<LocalizableAudioClip> clips, AudioType type)
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
		if (clips.Count > 0)
			source.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)].clip);
		else
			Debug.LogWarning("Empty sound list to play");
	}
	
	public static void PlayOneShotControlled(this AudioSource source, LocalizableAudioClip clip)
	{
		if (source == null)
		{
			Debug.LogWarning("PlayOneShotControlled null audio source");
			return;
		}
		source.PlayOneShotControlled(clip, source.GetAudioType());
	}
	
	
	public static void PlayOneShotControlled(this AudioSource source, LocalizableAudioClip clip, AudioType type)
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
		source.PlayOneShot(clip.clip);
	}

}
