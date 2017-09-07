using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private float currentY;
	public GameObject player;

	// Use this for initialization
	void Start () {
		currentY = transform.position.y;
	}
	
	private void LateUpdate() {
		if (player.transform.position.y > currentY) {
			transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
			currentY = transform.position.y;
		}

	}
}
