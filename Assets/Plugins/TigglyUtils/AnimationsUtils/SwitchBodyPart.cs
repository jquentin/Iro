using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(SwitchBodyPart))]
public class SwitchBodyPartInspector : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		SwitchBodyPart myTarget = (SwitchBodyPart)target;

		List<string> optionsList = new List<string>();
		optionsList.Add("None");
		foreach(BodyPartChoice choice in myTarget.choices)
			optionsList.Add(choice.name);
		string[] options = optionsList.ToArray();
		int index = GetPopupIndex(myTarget);
		index = EditorGUILayout.Popup(index, options);
		serializedObject.FindProperty("choice").floatValue = GetChoice(index);
		serializedObject.ApplyModifiedProperties();
	}

	int GetPopupIndex(SwitchBodyPart target)
	{
		return target.choiceIndex + 1;
	}

	float GetChoice(int index)
	{
		return (float)index - 1;
	}
}

#endif

[Serializable]
public class BodyPartChoice
{
	
	public string name;

	[Tooltip("List of objects to activate when this is chosen")]
	public GameObject[] relatedGameObjects;

	public void Enable()
	{
		foreach(GameObject go in relatedGameObjects)
			go.SetActive(true);
	}

	public void Disable()
	{
		foreach(GameObject go in relatedGameObjects)
			go.SetActive(false);
	}
}

[ExecuteInEditMode]
public class SwitchBodyPart : MonoBehaviour {

	[Tooltip("Convenient way to populate the component automatically for classic cases. Just drag the parent of the different choices here.")]
	public GameObject parent;

	[Tooltip("Example: Mouth, Eyes,etc")]
	public string bodyPartName;

	//	[Tooltip("List of choices for this body part")]
	public BodyPartChoice[] choices = new BodyPartChoice[0];

	public float choice;
	public int choiceIndex
	{
		get
		{
			return (int) choice;
		}
		set
		{
			EnableChoice(value);
		}
	}
	private float lastChoice;

	public void EnableChoice(int index)
	{
		for (int i = 0 ; i < choices.Length ; i++)
		{
			if (i == index)
			{
				choices[i].Enable();
			}
			else
			{
				choices[i].Disable();
			}
		}
	}

	public void EnableChoice(string name)
	{
		for (int i = 0 ; i < choices.Length ; i++)
		{
			if (choices[i].name == name)
			{
				choices[i].Enable();
			}
			else
			{
				choices[i].Disable();
			}
		}
	}

	void Update()
	{
		if (parent != null)
		{
			choices = new BodyPartChoice[parent.transform.childCount];
			for(int i = 0 ; i < parent.transform.childCount ; i++)
			{
				choices[i] = new BodyPartChoice();
				choices[i].relatedGameObjects = new GameObject[1];
				choices[i].relatedGameObjects[0] = parent.transform.GetChild(i).gameObject;
				choices[i].name = parent.transform.GetChild(i).gameObject.name;
			}
			parent = null;
		}

		if ((int)choice != (int)lastChoice)
		{
			EnableChoice((int)choice);
			lastChoice = choice;
		}

	}
}
