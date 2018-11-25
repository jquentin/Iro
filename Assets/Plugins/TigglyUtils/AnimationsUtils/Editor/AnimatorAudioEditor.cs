using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimatorAudio))]
public class AnimatorAudioEditor : Editor {

	AudioType audioType;

	double timeMessageShow = 2;
	double timeStartMessage;
	string message;

	public override void OnInspectorGUI ()
	{
		audioType = (AudioType)EditorGUILayout.EnumPopup(audioType);
		AudioClip ac = (AudioClip)EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
		if (ac != null)
		{
			bool itemAdded = ((AnimatorAudio)serializedObject.targetObject).AddItem(ac, audioType);
			if (itemAdded)
				ShowMessage(string.Format("Added clip \'{0}\' as {1}", ac.name, audioType), 2);
			else
				ShowMessage(string.Format("Clip \'{0}\' could not be added. There may be already a clip by the same name.", ac.name, audioType), 4);
			timeStartMessage = EditorApplication.timeSinceStartup;
		}
		EditorGUILayout.LabelField(message);

		if (!string.IsNullOrEmpty(message) && EditorApplication.timeSinceStartup > timeStartMessage + timeMessageShow)
			message = string.Empty;
		base.DrawDefaultInspector();
	}

	void ShowMessage(string message, double nbSeconds)
	{
		this.message = message;
		this.timeMessageShow = nbSeconds;
		this.timeStartMessage = EditorApplication.timeSinceStartup;
	}
}
