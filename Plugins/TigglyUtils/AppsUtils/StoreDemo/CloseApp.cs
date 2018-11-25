using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//using DG.Tweening;
namespace StoreDemo{
	using UnityEngine.UI;
	public enum TimeOutBehavior{
		relaunchTitle, quitAPP
	}
	public class CloseApp : MonoBehaviour {
		
		private Vector3 originalScale;
		private bool startCloseAppTimer = false;
		private bool startLearnMoreTimer = false;
//		public GameObject EventSystem;
		public int levelIndex = -1;
		public int timeout_learnMore = 60;
		public int timeout_triggerTimeOutBehavior = 30;
//		public bool timeout = true;
		public TimeOutBehavior timeout = TimeOutBehavior.quitAPP;
		public GameObject learnMorePanel;
	
		void Start(){

			learnMorePanel.SetActive(false);
			originalScale = transform.localScale;
			DontDestroyOnLoad(transform.parent.gameObject);
//			DontDestroyOnLoad(EventSystem);
		}
		void OnLevelWasLoaded(int level) {

			if (levelIndex == -1) {
				CloseApp[] others = GameObject.FindObjectsOfType<CloseApp>();
				Debug.Log("others.Length="+others.Length);
				if (others.Length>1) {
					Destroy(transform.parent.gameObject);
				}
				levelIndex = Application.loadedLevel;
			}else{
				if (levelIndex != level) {
					CancelInvoke("CloseAppOrJumpScene");
					CancelInvoke("LearnMorePanel");
					learnMorePanel.SetActive(false);
					startLearnMoreTimer = false;
					startCloseAppTimer = false;
				}
			}
//			Debug.Log("OnLevelWasLoaded="+level);
//			Debug.Log("ltgameObject.GetComponent<Button>().enabled="+	gameObject.GetComponent<Button>().enabled);
			if (levelIndex != level) {
				gameObject.GetComponent<Button>().enabled = false;
				gameObject.GetComponent<Image>().enabled = false;
			}else{

				gameObject.GetComponent<Button>().enabled = true;
				gameObject.GetComponent<Image>().enabled = true;
			}
			EventSystem es  = EventSystem.current;
			es.SetSelectedGameObject(transform.parent.gameObject, null);
		}
		void LearnMorePanel(){
			startLearnMoreTimer = false;
			learnMorePanel.SetActive(true);
			Invoke("CloseAppOrJumpScene",timeout_triggerTimeOutBehavior);
			startCloseAppTimer = true;
		}
		void CloseAppOrJumpScene(){
			Debug.Log("CloseAppOrJumpScene");
			if (timeout == TimeOutBehavior.quitAPP) {
				CloseAppHandler();
			}else if (timeout == TimeOutBehavior.relaunchTitle) {
				if (levelIndex!= -1) {
					Application.LoadLevel(levelIndex);
				}else {
					Application.LoadLevel(1);
				}
			}
		
		}
		public void CloseAppHandler(){
			
			#if UNITY_EDITOR
			Debug.Log("CloseAppHandler");
			#else
			#if !UNITY_WEBPLAYER
			System.Diagnostics.Process.GetCurrentProcess().Kill();
			#endif
			Application.Quit();
			#endif

		}
		public void KeepPlayingBTN(){
			CancelInvoke("CloseAppOrJumpScene");
			CancelInvoke("LearnMorePanel");
//			CancelInvoke("CloseAppHandler");
			learnMorePanel.SetActive(false);
			startLearnMoreTimer = false;
			startCloseAppTimer = false;
		}
		public void LearnMoreBTN(){
			CloseAppHandler();
		}
		#if InStoreDemo && !UNITY_EDITOR

		void Update(){
				if (Input.touchCount == 0 ) {
					if (startLearnMoreTimer == false && learnMorePanel.activeSelf == false) {
						Invoke("LearnMorePanel",timeout_learnMore);
						startLearnMoreTimer = true;
					}
				}
				if (Input.touchCount != 0 ) {
					if (startLearnMoreTimer == true) {
						CancelInvoke("LearnMorePanel");
						startLearnMoreTimer = false;
					}
				}
		}
		#endif
	}
}