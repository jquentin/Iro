using UnityEngine;
using System.Collections;

public abstract class TwoStepsButton : MonoBehaviour {

	enum State { Idle, Waiting, Validated }
	State currentState = State.Idle;

	public float waitingTime = 2f;

	void OnETMouseUpAsButton()
	{
		OnClick();
	}

	void OnClick()
	{
		if (currentState == State.Idle)
		{
			currentState = State.Waiting;
			SetToWaitingState();
			Invoke("TimeoutWaiting", waitingTime);
		}
		else if (currentState == State.Waiting)
		{
			CancelInvoke("TimeoutWaiting");
			currentState = State.Validated;
			ValidateButton();
		}
	}

	void TimeoutWaiting()
	{
		if (currentState == State.Waiting)
		{
			currentState = State.Idle;
			SetToIdleState();
		}
	}

	protected abstract void SetToWaitingState();

	protected abstract void SetToIdleState();

	protected abstract void ValidateButton();

}
