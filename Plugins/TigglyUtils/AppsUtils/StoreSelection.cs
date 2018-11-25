using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class StoreSelection 
{
	 
	public enum Store 
	{
		AppleStore, 
		GooglePlay, 
		Amazon, 
		Samsung, 
		InStoreDemo, 
		QVCDemo, 
		ChineseStore, 
		ChineseGooglePlay
	}
	public static Store store
	{
		get
		{
			#if AppleStore
			return Store.AppleStore;
			#elif GooglePlay
			return Store.GooglePlay;
			#elif Amazon
			return Store.Amazon;
			#elif Samsung
			return Store.Samsung;
			#elif InStoreDemo
			return Store.InStoreDemo;
			#elif QVCDemo
			return Store.QVCDemo;
			#elif ChineseStore
			return Store.ChineseStore;
			#elif ChineseGooglePlay
			return Store.ChineseGooglePlay;
			#else
			return Store.AppleStore;
			#endif
		}
	}

	#if UNITY_EDITOR

	static StoreSelection()
	{
		EditorApplication.delayCall += () => {
			CheckCurrentBuildStore();
		};
		EditorApplication.playmodeStateChanged += () => {
			CheckCurrentBuildStore();
		};
	}

	static bool IsStoreSymbol(string symbol)
	{
		try
		{
			if (Enum.Parse(typeof(Store), symbol) != null)
				return true;
			else
				return false;
		}
		catch
		{
			return false;
		}
	}

	static string GetStoreSymbol(Store store)
	{
		return store.ToString();
	}

	private const string MENU_NAME_PREFIX = "Tiggly/Set Build Store/";

	[MenuItem(MENU_NAME_PREFIX + "AppleStore")]
	public static void SetBuildStoreToAppleStore ()
	{
		SetBuildStore(Store.AppleStore);      
	}

	[MenuItem(MENU_NAME_PREFIX + "GooglePlay")]
	public static void SetBuildStoreToGooglePlay ()
	{
		SetBuildStore(Store.GooglePlay);
	}

	[MenuItem(MENU_NAME_PREFIX + "Amazon")]
	public static void SetBuildStoreToAmazon ()
	{
		SetBuildStore(Store.Amazon);
	}

	[MenuItem(MENU_NAME_PREFIX + "Samsung")]
	public static void SetBuildStoreToSamsung ()
	{
		SetBuildStore(Store.Samsung);
	}

	[MenuItem(MENU_NAME_PREFIX + "InStoreDemo")]
	public static void SetBuildStoreToInStoreDemo ()
	{
		SetBuildStore(Store.InStoreDemo);
	}

	[MenuItem(MENU_NAME_PREFIX + "QVCDemo")]
	public static void SetBuildStoreToQVCDemo ()
	{
		SetBuildStore(Store.QVCDemo);
	}

	[MenuItem(MENU_NAME_PREFIX + "ChineseStore")]
	public static void SetBuildStoreToChineseStore ()
	{
		SetBuildStore(Store.ChineseStore);
	}

	[MenuItem(MENU_NAME_PREFIX + "ChineseGooglePlay")]
	public static void SetBuildStoreToChineseGooglePlay ()
	{
		SetBuildStore(Store.ChineseGooglePlay);
	}

	static void SetBuildStore (Store store)
	{
		SetBuildStore(BuildTargetGroup.Android, BuildTarget.Android, store);
		SetBuildStore(BuildTargetGroup.iOS, BuildTarget.iOS, store);
		ReplaceStoreDependentAssets(store);
		SetBundleId(store);
		CheckBuildStore(store);
	}

	static void SetBundleId(Store store)
	{
		if (store == Store.ChineseStore || store == Store.ChineseGooglePlay)
		{
			if (!PlayerSettings.applicationIdentifier.Contains("Chinese"))
				PlayerSettings.applicationIdentifier = PlayerSettings.applicationIdentifier + "Chinese";
		}
		else
		{
			if (PlayerSettings.applicationIdentifier.Contains("Chinese"))
				PlayerSettings.applicationIdentifier = PlayerSettings.applicationIdentifier.Replace("Chinese", "");
		}
	}

	static void CheckCurrentBuildStore()
	{
		CheckBuildStore(store);
	}

	static void CheckBuildStore(Store store)
	{
		foreach(Store s in (Store[])Enum.GetValues(typeof(Store)))
		{
			Menu.SetChecked(MENU_NAME_PREFIX + s.ToString(), s == store);
		}
	}

	static void SetBuildStore (BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, Store store)
	{
		string[] symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
		string newSymbolList = "";
		foreach(string symbol in symbols)
			if (!IsStoreSymbol(symbol))
				newSymbolList += symbol + ";";
		newSymbolList += GetStoreSymbol(store);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newSymbolList);
	}

	static List<string> replacedAssets;

	static void ReplaceStoreDependentAssets(Store store)
	{
		string projectPath = Directory.GetCurrentDirectory();
		replacedAssets = new List<string>();
		ReplaceStoreDependentAsset(projectPath + "/StoreDependentAssets/" + GetStoreSymbol(store));
		ReplaceStoreDependentAsset(projectPath + "/StoreDependentAssets/Default");
	}

	static void ReplaceStoreDependentAsset(string folder)
	{
		if (Directory.Exists(folder))
			ReplaceStoreDependentAsset(Directory.GetFiles(folder));
	}

	static void ReplaceStoreDependentAsset(string[] files)
	{
		foreach(string file in files)
		{
			try
			{
				string[] split = file.Split(new char[]{'/'});
				string filename = split[split.Length - 1];
				string[] split2 = filename.Split(new char[]{'.'});
				string guid = split2[0];
				string targetFile = AssetDatabase.GUIDToAssetPath(guid);
				if (string.IsNullOrEmpty(targetFile))
					throw new Exception();
				if (!replacedAssets.Contains(guid))
				{
					ReplaceStoreDependentAsset(file, targetFile);
					replacedAssets.Add(guid);
				}
			}
			catch
			{
				Debug.LogWarning("Not a guid: " + file);
			}
		}
	}

	static void ReplaceStoreDependentAsset(string file, string target)
	{
		File.Copy(file, target, true);
	}
	
	#endif

}
