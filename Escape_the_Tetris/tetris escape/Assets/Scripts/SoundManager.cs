using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioClip LineCompleteSound;
	public AudioClip PlayerDieSound;
	public AudioClip WinSound;
	private AudioSource source;
	public static SoundManager soundManager;

	// Use this for initialization
	void Start () {
		if (soundManager == null)
			soundManager = this;

		source = GetComponent<AudioSource> ();
	}

	public void PlayLineComplete() {
		source.Stop ();
		source.clip = LineCompleteSound;
		source.Play ();
	}

	public void PlayPlayerDie() {
		source.Stop ();
		source.clip = PlayerDieSound;
		source.Play ();
	}

	public void PlayWin() {
		source.Stop ();
		source.clip = WinSound;
		source.Play ();
	}
}
