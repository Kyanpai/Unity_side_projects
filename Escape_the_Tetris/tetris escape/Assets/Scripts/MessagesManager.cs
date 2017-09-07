using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagesManager : MonoBehaviour {

	private List<string> sentences = new List<string> ();
	public static MessagesManager mmanager;

	public TextMeshProUGUI Title;
	public TextMeshProUGUI msg;

	void Start() {
		if (mmanager == null) {
			mmanager = this;
		}

	}

	public void StartDisplayMessage(Message message) {
		sentences.Clear ();

		Title.text = message.Title;

		foreach (string str in message.messages) {
			sentences.Add (str);
		}

		StopAllCoroutines ();
		StartCoroutine (DisplayMessagesWithDelay ());
	}

	IEnumerator DisplayMessagesWithDelay() {
		while (true) {
			foreach (string str in sentences) {
				DisplayNextMessage (str);
				yield return new WaitForSeconds (10);
			}
		}
	}

	public void DisplayNextMessage(string str) {
			
		StartCoroutine (DisplayMessageWithStyle (str));
	}

	IEnumerator DisplayMessageWithStyle(string message) {
		msg.text = "";
		foreach (char letter in message.ToCharArray()) {
			msg.text += letter;
			yield return null;
		}
	}

}
