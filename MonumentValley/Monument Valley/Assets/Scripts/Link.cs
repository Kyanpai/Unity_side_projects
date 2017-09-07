using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

	public List<Link> linkedNodes = new List<Link>();
	public Node attachedNode;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Boundary") {
			linkedNodes.Add(other.gameObject.GetComponent<Link>());
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Boundary") {
			linkedNodes.Remove(other.gameObject.GetComponent<Link>());
		}

		if (other.tag == "Player") {
			other.GetComponent<PlayerMovement>().StepOnLink(transform.position);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color32(33, 150, 243, 255);
		Gizmos.DrawCube(transform.position, new Vector3(.2f, .2f, .2f));
		for (int i = 0; i < linkedNodes.Count; i++) {
			Gizmos.color = new Color32(255, 160, 0, 255);
			Gizmos.DrawCube(transform.position, new Vector3(.1f, .1f, .1f));
			Gizmos.color = new Color32(76, 175, 80, 255);
			Gizmos.DrawLine(transform.position, linkedNodes[i].transform.position);
		}
	}

	// Use this for initialization
	void Start() {
		attachedNode = GetComponentInParent<Node>();
	}

	// Update is called once per frame
	void Update() {

	}

	public List<Node> GetLinkNeighbours() {
		List<Node> neighbours = new List<Node>();
		for (int i = 0; i < linkedNodes.Count; i++) {
			neighbours.Add(linkedNodes[i].attachedNode);
		}
//		Debug.Log("[" + attachedNode.transform.position + "] => Link neighbours size : " + neighbours.Count);
		return neighbours;
	}
}
