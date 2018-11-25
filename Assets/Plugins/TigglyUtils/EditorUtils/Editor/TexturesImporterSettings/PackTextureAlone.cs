using UnityEngine;
using System.Collections;
using UnityEditor;

public class PackTextureAlone : MonoBehaviour {
	/// <summary>
	/// Packs the selected textures each in their own packing tag, named with their name.
	/// As of Unity5.2.0p1, the changes won't get properly saved and get lost at some point.
	/// To avoid that, you'll have to 
	/// </summary>
	[MenuItem("Tiggly/Editor Utils/Pack selected textures by themselves")]
	public static void PackAlone () 
	{
		foreach(Object o in Selection.objects)
		{
			string path = AssetDatabase.GetAssetPath(o);
			AssetImporter ai = TextureImporter.GetAtPath(path);
			if (ai is TextureImporter)
			{
				TextureImporter ti = ai as TextureImporter;
				ti.spritePackingTag = o.name;
				ti.SaveAndReimport();
			}
		}
	}

}
