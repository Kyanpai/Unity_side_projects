using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {

	private Vector3 rotationMask = new Vector3(0, 0, 1);
	public float rotationSpeed = 5.0f; //degrees per second
	public Transform rotateAroundObject;

	void FixedUpdate() {
			transform.RotateAround(rotateAroundObject.transform.position, rotationMask, rotationSpeed * Time.deltaTime);
	}
}
