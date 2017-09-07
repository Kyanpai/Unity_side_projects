using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour {

	public float speed;
	private bool AlreadyTouched;

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player" && !AlreadyTouched) {
			AlreadyTouched = true;
			GameManager.gm.CreatePlatform();
		}
	}

	private void Start() {
		AlreadyTouched = false;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}
}
