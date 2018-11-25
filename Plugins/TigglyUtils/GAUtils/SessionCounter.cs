using UnityEngine;
using System.Collections;

namespace Tiggly
{

public class SessionCounter : MonoBehaviour 
{
	public static int numberOfTimesAppHasBeenOpened;
		
	private const string NUMBER_OF_TIMES_OPENED_KEY = "NumberOfTimesOpened";
	
	// Use this for initialization
	void Awake () 
	{
		numberOfTimesAppHasBeenOpened = PlayerPrefs.GetInt( NUMBER_OF_TIMES_OPENED_KEY );
		numberOfTimesAppHasBeenOpened += 1;
		PlayerPrefs.SetInt( NUMBER_OF_TIMES_OPENED_KEY, numberOfTimesAppHasBeenOpened );
	}
	
}

}
