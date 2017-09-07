using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public static CameraScript cs;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);

		if (cs == null) {
			cs = this;
		} else {
			DestroyObject (gameObject);
		}
	}


}
