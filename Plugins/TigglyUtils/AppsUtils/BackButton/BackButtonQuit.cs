using UnityEngine;
using System.Collections;

/**
 * This component can be added to the Title screen to quit the app when the 
 * Android back button is clicked and no higher priority BackButtonHandler is selectable
 **/
public class BackButtonQuit : BackButtonHandler {

	protected override bool isSelectable {
		get 
		{
			return true;
		}
	}

	protected override void Trigger ()
	{
		Application.Quit();
	}

}
