using UnityEngine;
using UnityEditor;

public class PlayerPrefsEditorUtils {

	[MenuItem("Tiggly/Player Prefs/Delete All")]
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
