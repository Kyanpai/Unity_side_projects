using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objects : MonoBehaviour {

	[Header("Reference")]
	public GameObject reference;
	[HideInInspector] public bool goodPosition = false;
	[HideInInspector] public float originY;

	[Space]
	public float precision = 10;

	void Start() {
		originY = transform.position.y;
	}

	void Update() {
		if (Quaternion.Angle(transform.rotation, reference.transform.rotation) < precision) {
			goodPosition = true;
		} else
			goodPosition = false;
	}
}
