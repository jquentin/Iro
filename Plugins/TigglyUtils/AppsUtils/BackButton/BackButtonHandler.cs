using UnityEngine;
using System.Collections;

/**
 * Children components should be used to handle the Android back button press
 * Only the selected BackButtonHandler with highest priority will be triggered
 **/
public abstract class BackButtonHandler : MonoBehaviour {

	// If several BackButtonHandler scripts are selectable when the user hits back, only the highest priorityLevel will be triggered
	// If there are several with the same priority level, only one of them (unpredictable) will be triggered
	public int priorityLevel;

	bool selected = false;
	static int maxSelectedPriorityLevel = int.MinValue;

	// This function defines if this BackButtonHandler is eligible for trigger
	abstract protected bool isSelectable { get; }

	// Select for triggering
	// The component will be triggered in LateUpdate if it has highest priority level
	void Select()
	{
		selected = true;
		maxSelectedPriorityLevel = Mathf.Max(maxSelectedPriorityLevel, priorityLevel);
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape) && isSelectable)
		{
			Select();
		}
	}

	void LateUpdate()
	{
		if (selected && priorityLevel == maxSelectedPriorityLevel)
		{
			Trigger();
			maxSelectedPriorityLevel = int.MinValue;
		}
		selected = false;
	}

	// This function will be called on the highest priority selectable BackButtonHandler component
	// Override for specific behaviours.
	abstract protected void Trigger();
}
