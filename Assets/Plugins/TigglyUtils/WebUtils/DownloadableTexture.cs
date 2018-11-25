using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DownloadableTexture : MonoBehaviour {


	private const string DOWNLOADABLE_TEXTURES_DATA_PATH = "https://s3.amazonaws.com/apps-utils/downloadable-textures/data/";
	private const string DOWNLOADABLE_TEXTURES_IMAGES_PATH = "https://s3.amazonaws.com/apps-utils/downloadable-textures/images/";

	public const string DOWNLOADABLE_TEXTURE_CACHE_VERSION_KEY = "DownloadableTextureCacheVersion";

	private const string WIDTH_PREF_SUFFIX = "_w";
	private const string HEIGHT_PREF_SUFFIX = "_h";

	public string jsonFileUrl;
	public string entryName;

	public string jsonFileFullUrl
	{
		get
		{
			string res = DOWNLOADABLE_TEXTURES_DATA_PATH;
			if (!res.EndsWith("/"))
				res += "/";
			res += jsonFileUrl;
			if (!(res.ToLower().EndsWith(".json") || res.ToLower().EndsWith(".txt")))
				res += ".json";
			return res;
		}
	}

	public string cachePath
	{
		get
		{
			return jsonFileUrl + "_" + entryName;
		}
	}

	private string GetImageFullUrl(string relativeUrl)
	{
		string res = "";
		if (!relativeUrl.Contains("://"))
		{
			res += DOWNLOADABLE_TEXTURES_IMAGES_PATH;
			if (!res.EndsWith("/"))
				res += "/";
		}
		res += relativeUrl;
		if (!(res.ToLower().EndsWith(".png") || res.ToLower().EndsWith(".jpg") || res.ToLower().EndsWith(".jpeg")))
			res += ".png";
		return res;
	}

	public static Dictionary<string, string> cachedJsonData = new Dictionary<string, string>();
	public static Dictionary<string, WWW> cachedJsonDataDownloads = new Dictionary<string, WWW>();

	private UISprite _sprite;
	private UISprite sprite
	{
		get
		{
			if (_sprite == null)
				_sprite = GetComponent<UISprite>();
			return _sprite;
		}
	}

	private UITexture _uiTexture;
	private UITexture uiTexture
	{
		get
		{
			if (_uiTexture == null)
				_uiTexture = GetComponent<UITexture>();
			if (_uiTexture == null)
			{
				_uiTexture = gameObject.AddComponent<UITexture>();
				if (sprite != null)
					CopyWidget(sprite, _uiTexture);
			}
			return _uiTexture;
		}
	}

	private Texture texture
	{
		set
		{
			uiTexture.mainTexture = value;
			if (sprite != null)
				sprite.enabled = false;
			uiTexture.enabled = true;
		}
	}

	IEnumerator Start () 
	{
		string data = null;
		if (cachedJsonData.ContainsKey(jsonFileUrl))
			data = cachedJsonData[jsonFileUrl];
		else
		{
			WWW jsonWWW;
			if (cachedJsonDataDownloads.ContainsKey(jsonFileUrl))
				jsonWWW = cachedJsonDataDownloads[jsonFileUrl];
			else
				jsonWWW = new WWW( jsonFileFullUrl );

			yield return jsonWWW;

			if ( string.IsNullOrEmpty( jsonWWW.error ) )
			{
				data = jsonWWW.text;
				if (!cachedJsonData.ContainsKey(jsonFileUrl))
					cachedJsonData.Add(jsonFileUrl, data);
			}
			else
			{
				Debug.Log( jsonWWW.error );
			}
		}
		Texture2D t2d = null;
		if (data != null)
		{
			int cachedVersion = PlayerPrefs.GetInt(DOWNLOADABLE_TEXTURE_CACHE_VERSION_KEY, 0);
			SimpleJSON.JSONNode jsonData = SimpleJSON.JSONNode.Parse(data);
			int latestVersion = jsonData["version"].AsInt;
			if (cachedVersion >= latestVersion)
			{
				t2d = LoadTextureFromCache(cachePath);
			}
			if (t2d == null)
			{
				string imgPath = JSONUtils.GetImageFromNode(jsonData[entryName]);
				string imgUrl = GetImageFullUrl(imgPath);
				WWW www = new WWW(imgUrl);
				yield return www;

				if ( string.IsNullOrEmpty( www.error ) )
				{
					t2d = www.texture;
					SaveTextureInCache( t2d, cachePath);
					PlayerPrefs.SetInt(DOWNLOADABLE_TEXTURE_CACHE_VERSION_KEY, latestVersion);
				}
				else
				{
					Debug.Log( www.error );
				}
			}
		}
		else
		{
			t2d = LoadTextureFromCache(cachePath);
		}

		if (t2d != null)
		{
			this.texture = t2d;
			yield return new WaitForEndOfFrame();
			Resources.UnloadUnusedAssets();
		}
	}

	public Texture2D LoadTextureFromCache( string thePath )
	{
		int width = PlayerPrefs.GetInt( thePath + WIDTH_PREF_SUFFIX );
		int height = PlayerPrefs.GetInt( thePath + HEIGHT_PREF_SUFFIX );

		return LoadTextureFromCache( width, height, thePath );
	}

	public Texture2D LoadTextureFromCache( int theWidth, int theHeight, string thePath )
	{
		#if !UNITY_WEBPLAYER
		string imagePath = Application.persistentDataPath + "/" + thePath;

		byte[] bytes;

		try 
		{
			bytes = File.ReadAllBytes( imagePath );
		}
		catch
		{

			Debug.Log( "ERROR WITH READING TEXTURE : " + imagePath );
			return null;
		}

		Texture2D result = new Texture2D( theWidth, theHeight, TextureFormat.RGBA32, false );

		result.LoadImage( bytes );
		result.filterMode = FilterMode.Bilinear;
		return result;
		#else
		return null;
		#endif
	}


	public void SaveTextureInCache( Texture2D theTexture, string thePath, bool shouldSaveDimensions = true )
	{
		#if !UNITY_WEBPLAYER
		byte[] bytes = theTexture.EncodeToPNG();
		string imagePath = Application.persistentDataPath + "/" + thePath;
		File.WriteAllBytes( imagePath, bytes );

		if ( shouldSaveDimensions )
		{
			PlayerPrefs.SetInt( thePath + WIDTH_PREF_SUFFIX, theTexture.width );
			PlayerPrefs.SetInt( thePath + HEIGHT_PREF_SUFFIX, theTexture.height );
		}
		#endif
	}
	/// <summary>
	/// Copy the specified widget's parameters.
	/// </summary>

	static public void CopyWidget (UIWidget reference, UIWidget target)
	{
		target.width = reference.width;
		target.height = reference.height;
		target.depth = reference.depth;
		target.color = reference.color;
		target.pivot = reference.pivot;
	}
		
}
