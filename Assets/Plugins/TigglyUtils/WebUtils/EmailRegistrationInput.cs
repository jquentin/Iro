using UnityEngine;
using System.Collections;

public class EmailRegistrationInput : MonoBehaviour {
	
	public GameObject beforeSubmission;
	public GameObject afterSubmission;
	public UIButton joinButton;
	public Texture joinButtonTexture;
	
	private UITexture joinButtonUITexture;

	protected virtual void Start()
	{
		beforeSubmission.SetActive(true);
		afterSubmission.SetActive(false);
		joinButton.isEnabled = false;
		joinButtonUITexture = joinButton.GetComponent<UITexture>();
		if (joinButtonUITexture != null && joinButtonTexture != null)
			joinButtonUITexture.mainTexture = joinButtonTexture;
	}

	public virtual void SubmitEmail () 
	{
		if (UIInput.current != null && IsEmailFormat(UIInput.current.value))
		{
			TigglyEmailRegistration.instance.RegisterEmail(UIInput.current.value, OnSubmissionComplete);
			UIInput.current.value = null;
		}
	}

	public void ChangeEmail () 
	{
		if (UIInput.current != null)
			joinButton.isEnabled = IsEmailFormat (UIInput.current.value);
	}

	protected bool IsEmailFormat (string email)
	{
		int nIndexAt = email.LastIndexOf("@");
		int nIndexPeriod = email.LastIndexOf(".");
		int nCountAt = 0;
		foreach(char c in email)
			if(c == '@')
				nCountAt++;
				
		bool bValid = (	(nCountAt == 1)							// Should only have one '@' in string
						&& email.Contains("@")					// Must contain '@'
						&& email.Contains(".")					// Must contain '.'
						&& (nIndexAt < nIndexPeriod)			// Index of '@' must be before last index of '.'
						&& (nIndexPeriod < email.Length-1));	// Last index of '.' should not be final character
		return bValid;
		//return email.Contains("@") && email.Contains(".") && email.IndexOf("@") < email.IndexOf(".") && email.IndexOf(".") < email.Length - 1;
	}
	
	protected void OnSubmissionComplete (bool wasSuccessful, string message) 
	{
		beforeSubmission.SetActive(false);
		afterSubmission.SetActive(true);
	}
}
