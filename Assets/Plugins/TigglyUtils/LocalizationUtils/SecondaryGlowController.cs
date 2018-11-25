using UnityEngine;
using System.Collections;

// if the game has support for a secondary language (like Safari) this Script attached to all the language selector buttons in the UI Settings panel
// This script is based on the GlowController script
public class SecondaryGlowController : MonoBehaviour {

	private GameObject _glow;
	public GameObject glow
	{
		get
		{
			if (_glow == null)
			{
				_glow = transform.parent.Find("glow").gameObject;
			}
			return _glow;
		}
	}
	
	void OnClick()
	{
		foreach (SecondaryGlowController gc in transform.parent.parent.GetComponentsInChildren<SecondaryGlowController>())
		{
			gc.glow.SetActive(false);
		}
		glow.SetActive(true);
	}
	
	void Start()
	{
		bool active = (transform.parent.GetComponent<UISprite>().spriteName == SecondaryLocalization.Get("Language_icon"));
		glow.SetActive(active);
	}
}
