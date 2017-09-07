using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTriggerScript : MonoBehaviour {

	public MoveUp moveUpScript;
	public Link A;
	public Link B;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Debug.Log("Player triggered");
			moveUpScript.alreadyMoved = true;
			moveUpScript.upward = !moveUpScript.upward;
		}

		if (other.tag == "Node") {
			Debug.Log("Node triggered");
			A.linkedNodes.Add(B);
			B.linkedNodes.Add(A);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Node") {
			A.linkedNodes.Remove(B);
			B.linkedNodes.Remove(A);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
