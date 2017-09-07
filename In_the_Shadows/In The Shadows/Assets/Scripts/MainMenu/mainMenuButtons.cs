using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuButtons : MonoBehaviour {

	public Button soundButton;
	public Sprite soundStop;
	public Sprite soundPlay;

	void Start() {
		if (PlayerPrefs.GetInt ("sound") == 0)
			soundButton.image.sprite = soundPlay;
		else
			soundButton.image.sprite = soundStop;
	}

	void Update() {
		if (PlayerPrefs.GetInt ("sound") == 0)
			soundButton.image.sprite = soundPlay;
		else
			soundButton.image.sprite = soundStop;
	}

	public void trainingMod() {
		PlayerPrefs.SetInt ("mod", 1);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void normalMod() {
		PlayerPrefs.SetInt ("mod", 2);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 2);
	}

	public void resetPrefs() {
		PlayerPrefs.DeleteAll ();
		if (!menuManager.mmanager.GetComponent<AudioSource> ().isPlaying)
			menuManager.mmanager.GetComponent<AudioSource> ().Play ();
	}

	public void manageSound() {
		if (menuManager.mmanager.changeMusicPlayingMod ()) {
			soundButton.image.sprite = soundPlay;
			PlayerPrefs.SetInt ("sound", 0);
		} else {
			soundButton.image.sprite = soundStop;
			PlayerPrefs.SetInt ("sound", 1);
		}
	}
}
