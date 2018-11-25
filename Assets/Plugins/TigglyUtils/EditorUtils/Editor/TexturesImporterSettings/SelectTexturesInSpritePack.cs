using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class SelectTexturesInSpritePack : EditorWindow {
	
	private bool useRegex = false;
	private string packingTag;
	private bool onlyWithFormat = false;
	private bool onlyWithReadWriteEnabledState = false;
	private bool readWriteEnabledState = true;
	private TextureImporterFormat format;
	private static SelectTexturesInSpritePack window;

	public SelectTexturesInSpritePack(string packingTag, TextureImporterFormat format)
	{
		this.packingTag = packingTag;
		this.format = format;
		this.onlyWithFormat = false;
		this.useRegex = false;
	}

	void OnGUI()
	{
		useRegex = GUILayout.Toggle(useRegex, "Use regulare expressions");
		packingTag = EditorGUILayout.TextField("Packing tag:",packingTag);
		onlyWithFormat = GUILayout.Toggle(onlyWithFormat, "Only with this format");
		if (onlyWithFormat)
		{
			format = (TextureImporterFormat)EditorGUILayout.EnumPopup(format);
		}
		
		onlyWithReadWriteEnabledState = GUILayout.Toggle(onlyWithReadWriteEnabledState, "Only with read-write enabled status");
		if (onlyWithReadWriteEnabledState)
		{
			readWriteEnabledState = GUILayout.Toggle(readWriteEnabledState, "Read-write enabled");
		}

		if(GUILayout.Button("Select")) 
		{
			Select();
			Close ();
		}
	}

	[MenuItem("Tiggly/Editor Utils/Select Textures With Packing Tag")]
	static void SelectTexturesWithPackingTag () 
	{
		if (window == null)
		{
			string tag = "";
			TextureImporterFormat format = TextureImporterFormat.AutomaticTruecolor;
			foreach(Object o in Selection.objects)
			{
				string path = AssetDatabase.GetAssetPath(o);
				AssetImporter ai = TextureImporter.GetAtPath(path);
				if (ai is TextureImporter)
				{
					TextureImporter ti = ai as TextureImporter;
					tag = ti.spritePackingTag;
					format = ti.textureFormat;
					break;
				}
			}

			window = new SelectTexturesInSpritePack(tag, format);
		}
		window.Show();
	}

	void Select() 
	{
		string pack = packingTag;
		List<Object> objectsToSelect = new List<Object>();
		foreach(string path in AssetDatabase.GetAllAssetPaths())
		{
			AssetImporter ai = TextureImporter.GetAtPath(path);
			if (!ai is TextureImporter)
				continue;
			TextureImporter ti = ai as TextureImporter;
			if (ti != null 
			    && ((useRegex && Regex.IsMatch(ti.spritePackingTag, pack)) || (!useRegex && ti.spritePackingTag == pack))
			    && (!onlyWithFormat || ti.textureFormat == format)
			    && (!onlyWithReadWriteEnabledState || ti.isReadable == readWriteEnabledState))
			{
				objectsToSelect.Add(AssetDatabase.LoadAssetAtPath<Texture>(path));
//				objectsToSelect.Add(AssetDatabase.LoadAssetAtPath(Path.GetDirectoryName(path), typeof(UnityEngine.Object)));
			}
		}
		
		Selection.objects = objectsToSelect.ToArray();
		if (Selection.selectionChanged != null)
			Selection.selectionChanged();
	}

}
