using UnityEngine;
using System.Collections;

/**
 * This class provides a generic transition fade that is meant to be custommized.
 * There are two ways to customize its behaviour.
 * 1) Inherit TransitionFade class and override UpdateValue.
 * 2) Inherit TransitionFadeElement and override UpdateValue.
 * The TransitionFade prefab needs to be placed in Resources folder and named "TransitionFade".
 * It will be automatically instantiated when instance is called.
 **/
public class TransitionFade : ResourcePrefabMonoSingleton <TransitionFade>
{

	[SerializeField]
	private Camera camera;
	[SerializeField]
	private float defaultFadeInTime = 1f;
	[SerializeField]
	private float defaultFadeOutTime = 1f;
	[SerializeField]
	private iTween.EaseType easeTypeIn = iTween.EaseType.linear;
	[SerializeField]
	private iTween.EaseType easeTypeOut = iTween.EaseType.linear;

	float currentValue;

	public delegate void OnFadeFinishedHandler();
	OnFadeFinishedHandler OnFadeFinished;

	void Awake()
	{
		UpdateValue(0f);
		DontDestroyOnLoad(gameObject);
	}

	protected virtual void UpdateValue(float value) 
	{
		foreach(TransitionFadeElement tfe in GetComponentsInChildren<TransitionFadeElement>())
		{
			tfe.UpdateValue(value);
		}
	}

	protected virtual void FadeInStart() {}
	protected virtual void FadeOutStart() {}

	void CallUpdateValue(float value)
	{
		UpdateValue(value);
		currentValue = value;
	}

	IEnumerator FadeInDelay_CR(OnFadeFinishedHandler callback, float time, float delay)
	{
		yield return new WaitForSeconds(delay);
		FadeIn(callback, time);
	}

	public void FadeIn (OnFadeFinishedHandler callback, float time = -1f, float delay = 0f) 
	{
		if (delay > 0f)
		{
			StartCoroutine(FadeInDelay_CR(callback, time, delay));
			return;
		}
		Debug.Log("FadeIn");
		FadeInStart();
		float fadeTime = time;
		if (fadeTime < 0f)
			fadeTime = defaultFadeInTime;
		OnFadeFinished = callback;
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", currentValue,
			"to", 0f,
			"time", fadeTime,
			"ignoretimescale", true,
			"easetype", easeTypeIn,
			"onupdate", "CallUpdateValue",
			"oncomplete", "OnCompleteFade"));
	}

	public void FadeOut (OnFadeFinishedHandler callback, float time = -1f) 
	{
		Debug.Log("FadeOut");
		FadeOutStart();
		float fadeTime = time;
		if (fadeTime < 0f)
			fadeTime = defaultFadeOutTime;
		OnFadeFinished = callback;
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", currentValue,
			"to", 1f,
			"time", fadeTime,
			"ignoretimescale", true,
			"easetype", easeTypeOut,
			"onupdate", "CallUpdateValue",
			"oncomplete", "OnCompleteFade"));
	}

	void OnCompleteFade()
	{
		if (OnFadeFinished != null)
			OnFadeFinished();
	}

}
