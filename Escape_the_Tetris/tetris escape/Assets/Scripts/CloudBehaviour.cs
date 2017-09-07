using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour {

	public float speed;
	public bool MoveLeft;

	// Update is called once per frame
	void Update () {
		if (MoveLeft) {
			transform.position -= new Vector3 (Time.deltaTime * speed, 0, 0);
			if (transform.position.x < -90)
				transform.position = new Vector3 (80, transform.position.y, transform.position.z);
		} else {
			transform.position += new Vector3 (Time.deltaTime * speed, 0, 0);
			if (transform.position.x > 80)
				transform.position = new Vector3 (-90, transform.position.y, transform.position.z);
		}
	}
}
