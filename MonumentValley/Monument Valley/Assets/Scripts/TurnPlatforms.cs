using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlatforms : MonoBehaviour {

	public bool canRotate;
	private int nextRotationY;
	public float speed;

	// Use this for initialization
	void Start () {
		canRotate = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gm.end)
			return;

		if (Input.GetKey(KeyCode.Mouse0)) {
			if (canRotate) {
				transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
			}
		} else {
			float currentAngle = transform.localRotation.eulerAngles.y;
			if (currentAngle >= 315 || currentAngle < 45)
				nextRotationY = 0;
			else if (currentAngle >= 45 && currentAngle < 135)
				nextRotationY = 90;
			else if (currentAngle >= 135 && currentAngle < 225)
				nextRotationY = 180;
			else
				nextRotationY = 270;

//			Debug.Log(currentAngle + " / " + nextRotationY);
			if (Mathf.Abs(transform.localRotation.eulerAngles.y - nextRotationY) > 10) {
				transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
			} else if (transform.rotation.y != nextRotationY) {
				transform.localEulerAngles = new Vector3(0, nextRotationY, 0);
			}
		}
	}

	private float AngleDegrees(float A, float B) {
		if (A < 0)
			return 360 - (Mathf.Atan2(A, B) * Mathf.Rad2Deg * -1);
		else
			return Mathf.Atan2(A, B) * Mathf.Rad2Deg;
	}
}
