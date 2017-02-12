using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TigglyNetworkBehaviour : NetworkBehaviour {
	
	private AudioSource _audioSource;
	public AudioSource audioSource
	{
		get
		{
			if (_audioSource == null)
				_audioSource = this.GetOrAddComponent<AudioSource>();
			return _audioSource;
		}
	}
}
