using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/**
 * This component provides a way to load another game scene in a smoother way than
 * Unity's Application/SceneManager.LoadLevel/LoadLevelAsync, especially for slower devices.
 * It will use the current TransitionFade to fade out of the current screen and fade in to the next.
 * If transitionSceneEnabled is true, it will first load a very light transition scene that
 * will load the actual next scene. This allows to clear the memory and avoid lags during loading.
 * That transition scene is provided along and needs to be integrated in the build settings scenes.
 * Call TransitionScene.TransitionToScene to trigger the scene transition.
 **/
public class TransitionScene : MonoBehaviour {

	static int _nextSceneIndex;
	static int nextSceneIndex { get { return _nextSceneIndex; } set { _nextScene = string.Empty; _nextSceneIndex = value; } }
	static string _nextScene;
	static string nextScene { get { return _nextScene; } set { _nextScene = value; _nextSceneIndex = -1; } }

	static AsyncOperation LoadNextSceneAsyncAdditive()
	{
		if (nextSceneIndex >= 0)
			return SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
		else if (!string.IsNullOrEmpty(nextScene))
			return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
		else
		{
			Debug.LogWarning("No next scene to load");
			return null;
		}
	}

	static void LoadNextScene()
	{
		if (nextSceneIndex >= 0)
			SceneManager.LoadScene(nextSceneIndex);
		else if (!string.IsNullOrEmpty(nextScene))
			SceneManager.LoadScene(nextScene);
		else
			Debug.LogWarning("No next scene to load");
	}

	AsyncOperation async ;

	static Action callback;

	IEnumerator Start () {
		Resources.UnloadUnusedAssets();
		System.GC.Collect();

		yield return new WaitForSeconds(1f);

		async = LoadNextSceneAsyncAdditive();
		async.allowSceneActivation = false;

		StartCoroutine(CheckingLoadProgress());
	}

	IEnumerator CheckingLoadProgress()
	{
		while ( async.progress<0.85f) {
			Debug.Log ("%: " + async.progress);
			yield return new WaitForSeconds(0.3f);
		}
		async.allowSceneActivation = true;
		yield return new WaitForSeconds(0.1f);
		TransitionFade.instance.FadeIn(OnFadeInComplete);

	}

	void OnFadeInComplete()
	{
		SceneManager.UnloadScene("TransitionScene");
		if (callback != null)
			callback();
	}

	public static void TransitionToScene(string nextScene, Action callback = null)
	{
		TransitionScene.callback = callback;
		TransitionScene.nextScene = nextScene;
		TransitionFade.instance.FadeOut(OnFadeOutComplete);
	}

	public static void TransitionToScene(int nextSceneIndex, Action callback = null)
	{
		TransitionScene.callback = callback;
		TransitionScene.nextSceneIndex = nextSceneIndex;
		TransitionFade.instance.FadeOut(OnFadeOutComplete);
	}

	static void OnFadeOutComplete()
	{
		if (transitionSceneEnabled)
		{
			SceneManager.LoadScene("TransitionScene");
		}
		else
		{
			//TODO needs to load the scene async
			LoadNextScene();
			TransitionFade.instance.FadeIn(null, -1f, 0.1f);
		}
	}



	public static bool transitionSceneEnabled
	{
		get
		{
			return true;
		}
	}
}
