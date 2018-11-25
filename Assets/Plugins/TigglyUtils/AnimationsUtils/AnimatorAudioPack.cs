using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AnimatorAudioPack : TigglyMonoBehaviour {

	public string packName;
	public List<AnimatorAudioClip> sounds = new List<AnimatorAudioClip>();

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
