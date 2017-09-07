using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endMenu : MonoBehaviour {

	public GameObject nextLevelButton;
	private Vector3 screenCenter;
	public float speed = 10;

	void Update() {
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (Screen.width / 2, Screen.height / 2, 0), speed * Time.deltaTime);
		if (transform.position != new Vector3 (Screen.width / 2, Screen.height / 2, 0))
			speed += 1;
		if (SceneManager.GetActiveScene ().buildIndex == SceneManager.sceneCountInBuildSettings)
			nextLevelButton.SetActive (false);
	}

	public void quitButton() {
		SceneManager.LoadScene (0);
	}

	public void levelSelection() {
		Debug.Log(PlayerPrefs.GetInt("lastLevelUnlocked") + "    " + SceneManager.GetActiveScene().buildIndex);
		if (PlayerPrefs.GetInt("mod") == 2 && SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1 && PlayerPrefs.GetInt("lastLevelUnlocked") == SceneManager.GetActiveScene().buildIndex + 1)
			PlayerPrefs.SetInt ("unlockedLevel", SceneManager.GetActiveScene().buildIndex + 1);
		SceneManager.LoadScene (PlayerPrefs.GetInt("mod"));
	}

}
