//#define SA_DEBUG_MODE
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using System;
using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class EmailComposer : MonoBehaviour {

	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE

	[DllImport ("__Internal")]
	private static extern void _SendMail(string subject, string body,  string recipients, string encodedMedia);


	#endif

	private static EmailComposer _instance = null;


	//Actions
	public Action<bool> OnMailResult;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void SendMail(string subject, string body, string recipients) {
		SendMail(subject, body, recipients, null);
	}
	
	public void SendMail(string subject, string body, string recipients, Texture2D texture) {
		if(texture == null) {
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_SendMail(subject, FormatForHtml(body), recipients, "");
			#else
			Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", recipients, EscapeUrl(subject), EscapeUrl(body)));
			#endif
		} else {
			
			
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			byte[] val = texture.EncodeToPNG();
			string bytesString = System.Convert.ToBase64String (val);
			_SendMail(subject, FormatForHtml(body), recipients, bytesString);
			#else
			//TODO Add attachment handling
			Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", recipients, EscapeUrl(subject), EscapeUrl(body)));
			#endif
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public static EmailComposer instance  {
		get {
			if(_instance == null) {
				GameObject go =  new GameObject("IOSSocialManager");
				_instance = go.AddComponent<EmailComposer>();
			}

			return _instance;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	

	private void OnMailFailed() {

		if(OnMailResult != null) {
			OnMailResult(false);
		}
	}

	private void OnMailSuccess() {

		if(OnMailResult != null) {
			OnMailResult(true);
		}
	}


	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------

	
	private string EscapeUrl(string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}
	
	private string FormatForHtml(string text)
	{
		return text.Replace("\n", "<BR>");
	}

	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
