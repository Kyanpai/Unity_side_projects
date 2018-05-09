using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	public float bulletSpeed;
	Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(Vector3.forward * bulletSpeed);
	}

	private void Start() {
		Destroy(gameObject, 3f);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			other.GetComponent<EnemyBehaviour>().Hit();
			Destroy(gameObject);
		}
	}
}
