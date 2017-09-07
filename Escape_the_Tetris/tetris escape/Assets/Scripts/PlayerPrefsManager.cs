using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

	public static PlayerPrefsManager ppManager;

	void Start() {
		if (ppManager == null)
			ppManager = this;
	}

	public void storeBestScore(int value) {
		PlayerPrefs.SetInt ("_bestScore", value);
	}

	public int getBestScore() {
		return PlayerPrefs.GetInt ("_bestScore");
	}

}
