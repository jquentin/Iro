using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BindingSearchCriteria
{
	public enum BindingParameter { Property, Path }
	public BindingParameter bindingParameter;

	public enum Type { Contains, Is, EndsWith, BeginsWith, DoesNotContain, IsNot }
	public Type type;

	public string text;
}

// Editor window for editing float curves in an animation clip
public class AnimationKeyframeValueEditor : EditorWindow
{
	private AnimationClip clip;
	private RuntimeAnimatorController animatorController;

	private List<BindingSearchCriteria> criterias = new List<BindingSearchCriteria>();

	public enum Operation { Multiply, Add, Set, SetTangents }
	private Operation operation;

	private float operationValue;

	public enum TangentMode { Editable, Smooth, Linear, Stepped }
	private TangentMode tangentMode;

	private int selectedClipIndex = 0;

	private Vector2 scrollPosition;

	[MenuItem ("Tiggly/Animation Utils/Animation Keyframe Value Editor")]
	static void Init ()
	{
		GetWindow (typeof (AnimationKeyframeValueEditor));
	}

	void Apply()
	{
		if (clip != null)
		{
			foreach (EditorCurveBinding binding in chosenBindings)
			{
				AffectBinding(binding);
			}
		}
	}
	List<EditorCurveBinding> _chosenBindings;
	List<EditorCurveBinding> chosenBindings
	{
		get
		{
			return _chosenBindings;
		}
	}

	List<string> _chosenBindingsDescriptions;
	List<string> chosenBindingsDescriptions
	{
		get
		{
			return _chosenBindingsDescriptions;
		}
	}

	bool IsBindingAccepted(EditorCurveBinding binding)
	{
		if (binding == null)
			return false;
		foreach(BindingSearchCriteria criteria in criterias)
		{
			if (criteria == null || string.IsNullOrEmpty(criteria.text))
				continue;
			string textToCompare = (criteria.bindingParameter == BindingSearchCriteria.BindingParameter.Path) ? binding.path : binding.propertyName;
			if (criteria.type == BindingSearchCriteria.Type.BeginsWith)
			{
				if (!textToCompare.StartsWith(criteria.text))
					return false;
			}
			else if (criteria.type == BindingSearchCriteria.Type.EndsWith)
			{
				if (!textToCompare.EndsWith(criteria.text))
					return false;
			}
			else if (criteria.type == BindingSearchCriteria.Type.Contains)
			{
				if (!textToCompare.Contains(criteria.text))
					return false;
			}
			else if (criteria.type == BindingSearchCriteria.Type.DoesNotContain)
			{
				if (textToCompare.Contains(criteria.text))
					return false;
			}
			else if (criteria.type == BindingSearchCriteria.Type.Is)
			{
				if (!textToCompare.Equals(criteria.text))
					return false;
			}
			else if (criteria.type == BindingSearchCriteria.Type.IsNot)
			{
				if (textToCompare.Equals(criteria.text))
					return false;
			}
		}
		return true;
	}

	void UpdateBindings()
	{
		Debug.Log("UpdateBindings");

		_chosenBindings = new List<EditorCurveBinding>();
		_chosenBindingsDescriptions = new List<string>();
		if (clip == null)
			return;
		foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings (clip))
		{
			if (IsBindingAccepted(binding))
			{
				_chosenBindings.Add(binding);
			}
		}
		foreach (EditorCurveBinding binding in _chosenBindings)
		{
			_chosenBindingsDescriptions.Add(GetBindingDescription(binding));
		}
	}

	public void OnGUI()
	{
		bool hasChanged = false;
		AnimationClip newClip = EditorGUILayout.ObjectField ("Clip", clip, typeof (AnimationClip), false) as AnimationClip;
		if (newClip != clip)
		{
			animatorController = null;
			clip = newClip;
			hasChanged = true;
		}

		RuntimeAnimatorController newAnimatorController = EditorGUILayout.ObjectField ("AnimatorController", animatorController, typeof (RuntimeAnimatorController), false) as RuntimeAnimatorController;
		if (newAnimatorController != animatorController)
		{
			animatorController = newAnimatorController;
			selectedClipIndex = 0;
			clip = animatorController.animationClips[selectedClipIndex];
			hasChanged = true;
		}

		if (animatorController != null)
		{
			string[] clipNames = animatorController.animationClips.Select((c)=>c.name).ToArray();
			int newSelectedClipIndex = EditorGUILayout.Popup(selectedClipIndex, clipNames);
			if (newSelectedClipIndex != selectedClipIndex)
			{
				clip = animatorController.animationClips[newSelectedClipIndex];
				selectedClipIndex = newSelectedClipIndex;
				hasChanged = true;
			}
		}

		EditorGUILayout.Space();

		if (criterias.Count < 1)
			criterias.Add(new BindingSearchCriteria());

		for (int i = 0 ; i < criterias.Count ; i++)
		{
			BindingSearchCriteria criteria = criterias[i];

			EditorGUILayout.BeginHorizontal();

			BindingSearchCriteria.BindingParameter newParameter = 
				(BindingSearchCriteria.BindingParameter) EditorGUILayout.EnumPopup(criteria.bindingParameter, GUILayout.Width(100f));

			BindingSearchCriteria.Type newType = 
				(BindingSearchCriteria.Type) EditorGUILayout.EnumPopup(criteria.type, GUILayout.Width(100f));
			
			string newText = EditorGUILayout.TextField (criteria.text);

			if (newText != criteria.text || newParameter != criteria.bindingParameter || newType != criteria.type)
				hasChanged = true;
			
			criteria.text = newText;
			criteria.bindingParameter = newParameter;
			criteria.type = newType;

			if (i == criterias.Count - 1)
			{
				if (GUILayout.Button("+", GUILayout.Width(30f)))
					criterias.Add(new BindingSearchCriteria());
			}

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField ("Operation:", GUILayout.Width(100f));
		operation = (Operation) EditorGUILayout.EnumPopup(operation, GUILayout.Width(100f));
		EditorGUILayout.LabelField ("Value:", GUILayout.Width(40f));
		if (operation == Operation.SetTangents)
			tangentMode = (TangentMode) EditorGUILayout.EnumPopup(tangentMode);
		else
			operationValue = EditorGUILayout.FloatField (operationValue);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		if (GUILayout.Button("Apply"))
		{
			Apply();
			hasChanged = true;
		}

		if (hasChanged)
			UpdateBindings();

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
		if (clip != null && chosenBindings != null)
		{
			foreach (string binding in chosenBindingsDescriptions)
			{
				EditorGUILayout.SelectableLabel (binding);
			}
		}
		EditorGUILayout.EndScrollView();
	}

	string GetBindingDescription(EditorCurveBinding binding)
	{
		AnimationCurve curve = AnimationUtility.GetEditorCurve (clip, binding);
		string keys = "{ ";
		foreach(Keyframe k in curve.keys)
			keys += k.value + " ; ";
		keys = keys.Remove(keys.Length - 3) + " }";
		return (binding.path + " -> " + binding.propertyName + "\n   Keys: " + keys);
	}

	void AffectBinding(EditorCurveBinding binding)
	{

		AnimationCurve curve = AnimationUtility.GetEditorCurve (clip, binding);
		Keyframe[] keys = curve.keys;
		for(int i = 0 ; i < keys.Length ; i++)
		{
			Keyframe k = keys[i];
			switch (operation)
			{
			case Operation.Multiply:
				keys[i].value = keys[i].value * operationValue;
				break;
			case Operation.Add:
				keys[i].value = keys[i].value + operationValue;
				break;
			case Operation.Set:
				keys[i].value = operationValue;
				break;
			case Operation.SetTangents:
				//Only stepped is supported for now
				if (tangentMode == TangentMode.Stepped)
				{
					keys[i].inTangent = float.PositiveInfinity;
					keys[i].outTangent = float.PositiveInfinity;
				}
				break;
			}
		}
		clip.SetCurve(binding.path, binding.type, binding.propertyName, new AnimationCurve(keys));
	}
}