using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour {

	public static menuManager mmanager;
	private AudioSource source;

	void Start() {
		source = GetComponent<AudioSource> ();
	}

	void Awake() {
		if (!mmanager) {
			mmanager = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (gameObject);
	}

	public bool changeMusicPlayingMod() {
		if (source.isPlaying) {
			GetComponent<AudioSource> ().Stop ();
			return false;
		} else {
			GetComponent<AudioSource> ().Play ();
			return true;
		}
	}
}
