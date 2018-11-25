using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class TigglyEmailRegistration : MonoBehaviour {

	#region Singleton
	private static TigglyEmailRegistration _instance;
	public static TigglyEmailRegistration instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<TigglyEmailRegistration>();
			}
			if (_instance == null)
			{
				GameObject go = new GameObject("TigglyEmailRegistration");
				_instance = go.AddComponent<TigglyEmailRegistration>();
			}
			return _instance;
		}
	}
	#endregion
	
	public delegate void EmailRegistrationServerCallbackDelegate(bool wasSuccessful, string message);

	
	//https://development.tiggly.com/version2.0/manageapi?f=tig.isUnlocked&u=4a6cb8d9ef751088a25bb26a11446910&p={"user_deviceid":"d22499b7b38fd3ad4af85a4f3161a389","app_name":"chef"}
	//RequestURL : https://development.tiggly.com/version2.0/manageapi?f=tig.sendRequest&u=4a6cb8d9ef751088a25bb26a11446910&p={"user_deviceid":"FEC34344-06C5-4F53-AEB8-41EF6C7EB1F6","email":"sagar.kudale@cuelogic.co.in","email_category":"unlock","app_type":"","device_type":"ios","app_name":"chef"}
	
	// This link does NOT send a welcome email back.
	//private string SERVICE_URL = "https://api.tiggly.com/production/version2.0/manageapi?f=tig.sendRequest&u=4a6cb8d9ef751088a25bb26a11446910&p=";
	private string SERVICE_URL = "https://api.tiggly.com/manageapi?f=tig.sendRequest&u=4a6cb8d9ef751088a25bb26a11446910&p=";

	protected string GetTigglySubscriptionURL(string email)
	{
		string urlMail = "";

		string deviceTypeString = "ios";
#if UNITY_ANDROID
		deviceTypeString = "android";
#endif

#if UNITY_ANDROID
		string deviceID = "Android";
#else
		string deviceID = SystemInfo.deviceUniqueIdentifier;//"EC34344-06C5-4F53-AEB8-41EF6C7EB1F6"; // need to find way to get apple device unique id.
#endif
		urlMail =  urlMail+  SERVICE_URL;
		urlMail = urlMail+"{"+"\"user_deviceid\":"+"\""+deviceID+"\""+",";
		urlMail = urlMail+"\"app_name\":"+"\""+TigglyConstants.instance.appName.ToLower()+"\""+",";
		urlMail = urlMail+"\"email\":"+"\""+email+"\""+",";
		urlMail = urlMail+"\"email_category\":"+"\""+"register"+"\""+",";
		urlMail = urlMail+"\"app_type\":"+"\""+TigglyConstants.instance.mainToys.ToString()+"\""+",";
		urlMail = urlMail+"\"is_register\":"+"\"yes\""+",";
		urlMail = urlMail+"\"device_type\":"+"\""+deviceTypeString+"\""+"}";
#if UNITY_EDITOR
		Debug.Log("CALLING: urlMail: "+urlMail);
#endif
		return urlMail;
	}

	public void RegisterEmail(string email, EmailRegistrationServerCallbackDelegate callback){
		
		if (!InternetReachabilityChecker.instance.isInternetReachable)
		{
			Debug.Log("Network not available");
			if (callback != null)
				callback(false,"Network Not Available");
			return;
		}

		StartCoroutine(registerMail(email, callback));
	}
	
	/*
	 * GET request
	 */ 
	private IEnumerator registerMail(string email, EmailRegistrationServerCallbackDelegate callback) {

		HTTP.Request getRequest = new HTTP.Request( "get", GetTigglySubscriptionURL(email));
		getRequest.Send();
		
		while( !getRequest.isDone )
		{
			yield return null;
		}
		
		if(getRequest!=null) {

			Debug.Log("get url" + getRequest.uri);
			Debug.Log( "get Response "+getRequest.response.Text);
			callback(true,getRequest.response.Text);
		}
	}
}
