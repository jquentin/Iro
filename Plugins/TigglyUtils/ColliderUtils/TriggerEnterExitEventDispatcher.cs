using UnityEngine;
using System.Collections;

public class TriggerEnterExitEventDispatcher : MonoBehaviour {

	public enum Mode { SendCSEvent, BroadcastUTMessageUpwards }
	public Mode mode = Mode.SendCSEvent;

	public delegate void OnTriggerEventHandler(Collider other);
	public OnTriggerEventHandler OnTriggerEnterEvent;
	public OnTriggerEventHandler OnTriggerExitEvent;

	void OnTriggerEnter(Collider other)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnTriggerEnterEvent != null)
				OnTriggerEnterEvent(other);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnTriggerExitEvent != null)
				OnTriggerExitEvent(other);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnTriggerExit", other, SendMessageOptions.DontRequireReceiver);
		}
	}
}
