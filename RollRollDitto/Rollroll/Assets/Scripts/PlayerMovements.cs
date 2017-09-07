using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {

	private GameObject currentPlatform;
	private Rigidbody2D rb;
	private Vector3 JumpDirection;
	public float TimeSinceLastTrigger;
	public float MaxTimeBetweenTwoTrigger;

	public float force;
	public float FallSpeed;

	private bool Jump;


	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		Jump = false;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (TimeSinceLastTrigger > MaxTimeBetweenTwoTrigger && (collision.transform.tag == "Platform" || collision.transform.tag == "LeftWall" || collision.transform.tag == "RightWall")) {
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			transform.parent = collision.transform;
			currentPlatform = collision.transform.gameObject;
			rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
		}
	}

	// Update is called once per frame
	void Update () {
		TimeSinceLastTrigger += Time.deltaTime;

		if (currentPlatform != null) {
			if (currentPlatform.tag == "Platform")
				JumpDirection = transform.position - currentPlatform.transform.position;
			else if (currentPlatform.tag == "LeftWall") {
				JumpDirection = new Vector3(transform.position.x + .2f, transform.position.y + .2f) - transform.position;
				transform.Translate(Vector3.down * Time.deltaTime * FallSpeed, Space.World);
			} else if (currentPlatform.tag == "RightWall") {
				JumpDirection = new Vector3(transform.position.x - .2f, transform.position.y + .2f) - transform.position;
				transform.Translate(Vector3.down * Time.deltaTime * FallSpeed, Space.World);
			}
		}

		if (currentPlatform != null && Input.GetKeyDown(KeyCode.Space)) {
			TimeSinceLastTrigger = 0;
			currentPlatform = null;
			JumpDirection.Normalize();
			transform.parent = null;
			Jump = true;
			rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		}
	}

	private void FixedUpdate() {
		if (Jump) {
			Jump = false;
			rb.AddForce(JumpDirection * force, ForceMode2D.Impulse);
			rb.constraints = RigidbodyConstraints2D.None;
		}
	}

	private void OnDrawGizmos() {
		if (currentPlatform != null)
			Gizmos.DrawLine(transform.position, currentPlatform.transform.position);
	}
}
