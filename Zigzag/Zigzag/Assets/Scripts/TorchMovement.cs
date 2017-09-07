using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchMovement : MonoBehaviour {

	public float speed = 200;
	public float distance;
	private Vector3 direction;
	private Vector3 origin;

	// Use this for initialization
	void Start () {
		direction = Vector3.up;
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y - origin.y < 0 || origin.y + distance < transform.position.y)
			direction *= -1;
		transform.Translate(direction * Time.deltaTime * speed);
		
	}
}
