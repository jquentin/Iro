using UnityEngine;
using System.Collections;

public static class SendMessageUtils
{
	public static void SendMessageUpwardsExcludingSelf(this GameObject go, string message, object o, SendMessageOptions sendMessageOptions)
	{
		foreach(Transform parent in go.transform.GetParents(false))
			parent.gameObject.SendMessage(message, o, sendMessageOptions);
	}
}

public class CollisionEventDispatcher : MonoBehaviour {

	public enum Mode { SendCSEvent, BroadcastUTMessageUpwards }
	public Mode mode = Mode.SendCSEvent;

	public delegate void OnCollisionEventHandler(Collision collision);
	public OnCollisionEventHandler OnCollisionEnterEvent;
	public OnCollisionEventHandler OnCollisionStayEvent;
	public OnCollisionEventHandler OnCollisionExitEvent;

	public delegate void OnTriggerEventHandler(Collider other);
	public OnTriggerEventHandler OnTriggerEnterEvent;
	public OnTriggerEventHandler OnTriggerStayEvent;
	public OnTriggerEventHandler OnTriggerExitEvent;

	void OnCollisionEnter(Collision collision)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnCollisionEnterEvent != null)
				OnCollisionEnterEvent(collision);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
		}
	}
	void OnCollisionStay(Collision collision)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnCollisionStayEvent != null)
				OnCollisionStayEvent(collision);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnCollisionStay", collision, SendMessageOptions.DontRequireReceiver);
		}
	}
	void OnCollisionExit(Collision collision)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnCollisionExitEvent != null)
				OnCollisionExitEvent(collision);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnCollisionExit", collision, SendMessageOptions.DontRequireReceiver);
		}
	}

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
	void OnTriggerStay(Collider other)
	{
		if (mode == Mode.SendCSEvent)
		{
			if (OnTriggerStayEvent != null)
				OnTriggerStayEvent(other);
		}
		else
		{
			gameObject.SendMessageUpwardsExcludingSelf("OnTriggerStay", other, SendMessageOptions.DontRequireReceiver);
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
