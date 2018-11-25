using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Use this tool to transfer from AudioClips to LocalizableAudioClips
/// if you have built your app without any localization and have to add it later on.
/// This window will find the AudioClip properties in the selected GameObjects' monobehaviour
/// If you have equivalent LocalizableAudioClip property aside, named the same with "Loc" appended,
/// you will be able to "Transfer" the clips.
/// This will look at the path of the audio clip, and fill the relative path 
/// from the language folder in the LocalizableAudioClip's key property.
/// Note the AudioClips have to be already placed in Resources/<lang_code>/ folder.
/// 
/// Things to do to localize your app:
/// 1) Place all your audio clips to be localized in a Resources/en folder (or any other language your app has been first built for)
/// 2) Create LocalizableAudioClip properties to replace AudioClip properties, 
///    Name them the same as the AudioClip property + "Loc".
/// 3) Use those new properties in your code instead of the old AudioClip (Use Mono's "Find References" feature to find all uses of the properties)
/// 4) Open this transfer window.
/// 5) Select the game object containing the AudioClips.
/// 6) The properties to be transfered should appear enabled.
///    Properties shown disabled don't have equivalent LocalizableAudioClips named the same + "Loc"
/// 7) Hit "Transfer"
/// 8) Save
/// 9) Test
/// 
/// </summary>
public class AudioClipPropertiesPath : List<string>
{
	public override string ToString ()
	{
		string res = "";
		foreach( string s in this)
			res += s + ".";
		res = res.Remove(res.LastIndexOf("."));
		return res;
	}

	public override bool Equals (object obj)
	{
		AudioClipPropertiesPath acpp = (AudioClipPropertiesPath) obj;
		if (acpp == null)
			return false;
		return (ToString() == acpp.ToString());
	}

	public static AudioClipPropertiesPath operator +(AudioClipPropertiesPath c1, AudioClipPropertiesPath c2) 
	{
		AudioClipPropertiesPath res = new AudioClipPropertiesPath();
		if (c1 != null)
			res.AddRange(c1);
		if (c2 != null)
			res.AddRange(c2);
		return res;
	}
	
	public static AudioClipPropertiesPath operator +(AudioClipPropertiesPath c1, string c2) 
	{
		AudioClipPropertiesPath res = new AudioClipPropertiesPath();
		if (c1 != null)
			res.AddRange(c1);
		res.Add(c2);
		return res;
	}

}

public class AudioClipProperties
{
	public string component;
	public AudioClipPropertiesPath path;
	public bool localizable = false;
	public bool toAffect = false;
	public List<GameObject> gameObjects = new List<GameObject>();
	public AudioClipProperties(string component, AudioClipPropertiesPath path, bool localizable)
	{
		this.component = component;
		this.path = path;
		this.localizable = localizable;
		this.toAffect = localizable;
	}

	public override bool Equals (object o)
	{
		AudioClipProperties acp = (AudioClipProperties)o;
		if (acp == null)
			return false;
		return (acp.path.ToString() == path.ToString() && acp.component == component);
	}

	public string GetGameObjectsList()
	{
		string res = "(in ";
		for( int i = 0 ; i < gameObjects.Count ; i++)
		{
			res += gameObjects[i].name;
			if (i < gameObjects.Count - 1)
				res += ", ";
		}
		res += ")";
		return res;
	}

}

public class AudioClipToLocalizableWindow : EditorWindow {

	private List<AudioClipProperties> clips = new List<AudioClipProperties>();
	
	private string field = "";

	private bool isRecursive = false;

	private Vector2 scrollPos = new Vector2();

	List<AudioClipProperties> GetClipList(string component, System.Type type, AudioClipPropertiesPath prefix = null)
	{
		List<AudioClipProperties> res = new List<AudioClipProperties>();
		FieldInfo[] fields = type.GetFields();
		foreach(FieldInfo fi in fields)
		{
			if (fi.IsNotSerialized || fi.IsStatic)
				continue;
			AudioClipPropertiesPath path = prefix + fi.Name;
			if (fi.FieldType == typeof(AudioClip))
				res.Add(new AudioClipProperties(component, path, type.GetField(fi.Name + "Loc") != null));
			else if (fi.FieldType.IsArray)
			{
				if (fi.FieldType.GetElementType() == typeof(AudioClip))
					res.Add(new AudioClipProperties(component, path + "Array.data[]", type.GetField(fi.Name + "Loc") != null));
				else
					res.AddRange(GetClipList(component, fi.FieldType.GetElementType(), path + "Array.data[]"));
			}
			else if (IsAssignableToGenericType(fi.FieldType, typeof(List<>)))
			{
				if (fi.FieldType.GetGenericArguments()[0] == typeof(AudioClip))
					res.Add(new AudioClipProperties(component, path + "Array.data[]", type.GetField(fi.Name + "Loc") != null));
				else
					res.AddRange(GetClipList(component, fi.FieldType.GetGenericArguments()[0], path + "Array.data[]"));
			}
			else if (!typeof(Component).IsAssignableFrom(fi.FieldType) && !typeof(GameObject).IsAssignableFrom(fi.FieldType))
				res.AddRange(GetClipList(component, fi.FieldType, path));
		}
		return res;
	}

	private bool AddToClipList(GameObject go, AudioClipProperties acp)
	{
		AudioClipProperties similar = clips.Find(delegate(AudioClipProperties obj) {
			return obj.Equals(acp);
		});
		if (similar == null)
		{
			clips.Add(acp);
			acp.gameObjects.Add(go);
			return true;
		}
		else
		{
			similar.gameObjects.Add(go);
			return false;
		}
	}
	
	private void AddRangeToClipList(GameObject go, List<AudioClipProperties> acps)
	{
		foreach(AudioClipProperties acp in acps)
			AddToClipList(go, acp);
	}

	public static bool IsAssignableToGenericType(System.Type givenType, System.Type genericType)
	{
		var interfaceTypes = givenType.GetInterfaces();
		
		foreach (var it in interfaceTypes)
		{
			if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
				return true;
		}
		
		if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			return true;
		
		System.Type baseType = givenType.BaseType;
		if (baseType == null) return false;
		
		return IsAssignableToGenericType(baseType, genericType);
	}
	
	void Update()
	{
		minSize = new Vector2(400f, 560f);
		// This is necessary to make the framerate normal for the editor window.
		Repaint();
	}

	void OnSelectionChange()
	{
		clips.Clear();
		if (Selection.gameObjects == null)
			return;
		foreach(GameObject go in Selection.gameObjects)
		{
			if (isRecursive)
			{
				foreach(Component c in go.GetComponentsInChildren<MonoBehaviour>())
					AddRangeToClipList(c.gameObject, GetClipList(c.GetType().Name,c.GetType()));
			}
			else
			{
				foreach(Component c in go.GetComponents<MonoBehaviour>())
					AddRangeToClipList(go, GetClipList(c.GetType().Name,c.GetType()));
			}
		}
	}

	void OnGUI ()
	{
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		bool newRecursive = GUILayout.Toggle(isRecursive, "Recursive");
		if (isRecursive != newRecursive)
		{
			isRecursive = newRecursive;
			OnSelectionChange();
		}

		for (int i = 0 ; i < clips.Count ; i++)
		{
			GUILayout.BeginHorizontal();
			GUI.enabled = clips[i].localizable;
			bool b = clips[i].toAffect;
			b = GUILayout.Toggle(clips[i].toAffect, "", GUILayout.Width(20f));
			clips[i].toAffect = b;
			GUILayout.Label(clips[i].component, GUILayout.Width(180f));
			GUILayout.Label(clips[i].path.ToString());
			GUILayout.EndHorizontal();
			GUILayout.Label(clips[i].GetGameObjectsList());
		}
		GUI.enabled = false;
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.gray;
		style.wordWrap = true;
		GUILayout.Label(" (*) Properties shown disabled don't have equivalent LocalizableAudioClips named the same with \"Loc\" appended.\n"
		                + "Add the properties to be able to transfer from the AudioClips to the LocalizableAudioClips", style);

		GUI.enabled = true;
		if (clips.Count > 0)
		{
			if (GUILayout.Button("Transfer"))
			{
				AffectSelection();
			}
		}
		GUILayout.EndScrollView();
	}

	void AffectSelection()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if (isRecursive)
			{
				foreach(Transform child in go.GetComponentsInChildren<Transform>())
					AffectGameObject(child.gameObject);
			}
			else
			{
				AffectGameObject(go);
			}
		}
	}

	void AffectGameObject(GameObject go)
	{
		foreach(AudioClipProperties acp in clips)
		{
			if (!acp.toAffect)
				continue;
			SerializedObject so = new SerializedObject (go.GetComponent(acp.component));
			if (so != null)
				AffectPropertyRec(so, acp.path.ToString());
		}
	}

	void AffectPropertyRec(SerializedObject so, string path)
	{
		// If all array index has been filled with a value, fill the property
		if (!path.Contains("[]"))
		{
			SerializedProperty sp = null;
			SerializedProperty spLoc = null;
			// If this is a list or array of AudioClips, look for the list or array of LocalizableAudioClips
			if (path.EndsWith("]"))
			{
				int endIndex = path.LastIndexOf(".Array.data[");
				string end = path.Substring(endIndex);

				string arrayPath = path.Remove(endIndex);
				SerializedProperty arrayProp = so.FindProperty(arrayPath);

				string arrayLocPath = arrayPath + "Loc";
				SerializedProperty arrayLocProp = so.FindProperty(arrayLocPath);

				arrayLocProp.arraySize = arrayProp.arraySize;
				string elmtPath = arrayPath + "Loc" + end + ".key";
				sp = so.FindProperty(path);
				spLoc = so.FindProperty(elmtPath);
			}
			// Else, look for the LocalizableAudioClip in the object
			else
			{
				sp = so.FindProperty(path);
				spLoc = so.FindProperty(path + "Loc.key");
			}

			if (spLoc != null && sp != null && sp.objectReferenceValue != null && !string.IsNullOrEmpty(GetKeyFromObject(sp.objectReferenceValue)))
			{
				spLoc.stringValue = GetKeyFromObject(sp.objectReferenceValue);
				sp.objectReferenceValue = null;
			}
			else
				Debug.LogWarning("Problem with: " + so.targetObject.name + " - " + path);
		}
		// Else, iterate on the first index and fill the other indices recursively
		else
		{
			string arrayPath = path.Remove(path.IndexOf(".Array.data[]"));
			SerializedProperty arrayProp = so.FindProperty(arrayPath);
			int arraySize = arrayProp.arraySize;
			for (int i = 0 ; i < arraySize ; i++)
			{
				AffectPropertyRec(so, path.ReplaceFirst("[]", "[" + i + "]"));
			}
		}
		so.ApplyModifiedProperties();
	}

	string GetKeyFromObject (Object o)
	{
		string fullPath = UnityEditor.AssetDatabase.GetAssetPath(o);
		int resourcesStartIndex = fullPath.IndexOf("/Resources");
		List<string> splitPath = new List<string>(fullPath.Split(new char[]{'/'}, System.StringSplitOptions.RemoveEmptyEntries));
		int resourcesNodeIndex = splitPath.IndexOf("Resources");
		if (resourcesNodeIndex >= 0)
		{
			splitPath.RemoveRange(0, resourcesNodeIndex + 2);
			string relativePath = "";
			foreach (string node in splitPath)
				relativePath += node + "/";
			relativePath = relativePath.Remove(relativePath.Length - 1);
			relativePath = relativePath.Remove(relativePath.LastIndexOf("."));
			return relativePath;
		}
		else
		{
			Debug.LogError("The clip " + fullPath + " is not in a Resources folder. It can't be localized.");
			return "";
		}
	}


	
	[MenuItem("Tiggly/Localization/Localize Audio Clips Window")]
	public static void TransferSortingLayers()
	{
		EditorWindow.GetWindow<AudioClipToLocalizableWindow>(false, "Localize Audio Clips", true).Show();
	}

}

public static class StringExtensions
{
	
	public static string ReplaceFirst(this string text, string search, string replace)
	{
		int pos = text.IndexOf(search);
		if (pos < 0)
		{
			return text;
		}
		return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
	}

}
