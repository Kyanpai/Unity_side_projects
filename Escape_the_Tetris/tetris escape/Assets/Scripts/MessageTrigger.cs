using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour {

	public Message message;

	public void TriggerMessage() {
		MessagesManager.mmanager.StartDisplayMessage (message);
	}
}
