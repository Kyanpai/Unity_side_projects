using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {

	public float speed;
	private Vector3 direction;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		direction = Vector3.zero;
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetMouseButtonDown(0) && !GameManager.gm.GameOver)
		{
			if (direction == Vector3.forward)
				direction = Vector3.left;
			else
				direction = Vector3.forward;
		}

	#else
		if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
		{
			if (direction == Vector3.forward)
				direction = Vector3.left;
			else
				direction = Vector3.forward;
		}
	#endif
	}

	private void FixedUpdate()
	{
		if (!GameManager.gm.GameOver)
//			rb.AddForce(direction * speed);
			rb.velocity = direction * speed;
	}
}
