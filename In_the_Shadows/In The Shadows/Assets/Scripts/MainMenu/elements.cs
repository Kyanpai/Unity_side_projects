using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class elements : MonoBehaviour {

	public GameObject prevElem;
	public GameObject nextElem;
	public Vector3 goodPosition;
	public bool unlocked;
	public bool done;
	public int level;
	public GameObject[] subObjects;
	public string title;

	void Start() {
		if (level < PlayerPrefs.GetInt ("lastLevelUnlocked")) {
			done = true;
			unlocked = true;
		}
		else if (level == PlayerPrefs.GetInt ("lastLevelUnlocked"))
			unlocked = true;
		foreach (GameObject elem in subObjects) {
			if (!unlocked || !done) {
				elem.transform.rotation = Random.rotation;
			}
		}
	}
}
