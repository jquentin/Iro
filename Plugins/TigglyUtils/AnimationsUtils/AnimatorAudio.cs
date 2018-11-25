using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AnimatorAudioClip
{

	public string name;

	public AudioType audioType;

	public AudioClip clip;
	public List<AudioClip> alternativeClips;

	public List<AudioClip> clips
	{
		get
		{
			List<AudioClip> res = new List<AudioClip>(alternativeClips);
			res.Add(clip);
			return res;
		}
	}

	public AnimatorAudioClip(){}

	public AnimatorAudioClip(AudioClip clip, AudioType type)
	{
		this.clip = clip;
		this.audioType = type;
		this.name = clip.name;
	}

	public void Validate()
	{
		if (string.IsNullOrEmpty(name) && clip != null)
		{
			name = clip.name;
		}
	}

}

public class AnimatorAudio : TigglyMonoBehaviour {

	public List<AnimatorAudioClip> sounds = new List<AnimatorAudioClip>();

	void Awake()
	{
		AddExternalAudioPacks();
	}

	void AddExternalAudioPacks()
	{
		foreach(AnimatorAudioPack pack in GetComponents<AnimatorAudioPack>())
		{
			sounds.AddRange(pack.sounds);
		}
	}

	public void PlayAudioClip(AudioClip clip)
	{
		if (clip == null)
		{
			Debug.LogWarning("null clip");
			return;
		}
		audioSource.PlayOneShotControlled(clip);
	}

	public void PlaySound(int index)
	{
		if (sounds == null || index >= sounds.Count)
		{
			Debug.LogWarning("index out of bound");
			return;
		}
		audioSource.PlayOneShotControlled(sounds[index].clips, sounds[index].audioType);
	}

	public void PlaySoundByName(string name)
	{
		if (sounds == null)
		{
			Debug.LogWarning("sounds list empty");
			return;
		}
		AnimatorAudioClip clip = sounds.Find(snd => snd.name.Trim() == name.Trim());
		if (clip == null)
		{
			Debug.LogWarning("clip not found: " + name);
			return;
		}
		audioSource.PlayOneShotControlled(clip.clips, clip.audioType);
	}

	public bool AddItem(AudioClip clip, AudioType type)
	{
		if (sounds.Any(aac => aac.name == clip.name))
		{
			return false;
		}
		sounds.Add(new AnimatorAudioClip(clip, type));
		return true;
	}

}
