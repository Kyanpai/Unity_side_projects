using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject[] items;
	public GameObject endMenu;
	// Check if all objects are right
	private bool globalGoodPosition = false;
	public bool levelEnded = false;
	public static GameManager gm;
	public GameObject pauseMenu;

	void Start() {
		gm = this;
		items = GameObject.FindGameObjectsWithTag ("object");
	}

	void Update () {
		foreach (GameObject item in items) {
			if (item.GetComponent<objects> ().goodPosition)
				globalGoodPosition = true;
			else {
				globalGoodPosition = false;
				break;
			}
		}
		if (globalGoodPosition) {
			if (items.Length == 2) {
				if (Mathf.Abs (items [0].transform.position.y - items [1].transform.position.y) < 1) {
					endMenu.SetActive (true);
					levelEnded = true;
				}
			} else {
				endMenu.SetActive (true);
				levelEnded = true;
			}
		}

		if (levelEnded && PlayerPrefs.GetInt("lastLevelUnlocked") < SceneManager.GetActiveScene().buildIndex  + 1)
			PlayerPrefs.SetInt ("lastLevelUnlocked", SceneManager.GetActiveScene ().buildIndex + 1);
		if (Input.GetKeyDown (KeyCode.R) && !levelEnded)
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		if (Input.GetKeyDown (KeyCode.Escape) && !levelEnded)
			pauseMenu.SetActive (!pauseMenu.activeSelf);
	}
}
