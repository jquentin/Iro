using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TigglyNetworkBehaviour : NetworkBehaviour {

	public static bool offlineMode = true;

	public bool isLocalPlayer
	{
		get
		{
			PlayerController cc = this.transform.GetComponent<PlayerController>();
			return (offlineMode && (cc == null || cc is IroCharacterController) ) || base.isLocalPlayer;
		}
	}
	public bool isServer
	{
		get
		{
			return offlineMode || base.isServer;
		}
	}

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

	public static void ModeDependantCall(Action ifOnline, Action ifOffline)
	{
		if (offlineMode)
		{
			if (ifOffline != null)
				ifOffline();
		}
		else
		{
			if (ifOnline != null)
				ifOnline();
		}
	}

	public static void ModeDependantCall<Type>(Action<Type> ifOnline, Action<Type> ifOffline, Type value)
	{
		ModeDependantCall(()=>ifOnline(value), ()=>ifOffline(value));
	}

	public static void ModeDependantCall<Type1, Type2>(Action<Type1, Type2> ifOnline, Action<Type1, Type2> ifOffline, Type1 value1, Type2 value2)
	{
		ModeDependantCall(()=>ifOnline(value1, value2), ()=>ifOffline(value1, value2));
	}

	public static void ModeDependantCall<Type1, Type2, Type3>(Action<Type1, Type2, Type3> ifOnline, Action<Type1, Type2, Type3> ifOffline, Type1 value1, Type2 value2, Type3 value3)
	{
		ModeDependantCall(()=>ifOnline(value1, value2, value3), ()=>ifOffline(value1, value2, value3));
	}

	public static void SpawnIfOnline(GameObject go)
	{
		if (!offlineMode)
			NetworkServer.Spawn(go);
	}
}
