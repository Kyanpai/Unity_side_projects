using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour {

	public bool alreadyMoved;
	public float heightToMove;
	private Vector3 initialPosition;
	public bool upward;

	// Use this for initialization
	void Start () {
		alreadyMoved = false;
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (alreadyMoved) {
			int up = 1;
			if (!upward)
				up = -1;
			transform.position = Vector3.MoveTowards(transform.position, initialPosition + new Vector3(0, up * heightToMove, 0), .05f);
		}
	}
}
