using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public int gCost;
	public int hCost;

	public Link[] Links;

	public Node ParentNode;
	public Link entryPoint;
	public Link leavePoint;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<PlayerMovement>().StepOnNode(this);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color32(33, 150, 243, 255);
		Gizmos.DrawSphere(transform.position, .2f);
	}

	private void Start() {
		Links = GetComponentsInChildren<Link>();
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public List<Node> GetNeighbours() {
		List<Node> list = new List<Node>();

		if (Links == null)
			return list;
		for (int i = 0; i < Links.Length; i++) {
			list.AddRange(Links[i].GetLinkNeighbours());
		}

//		Debug.Log("[" + transform.position + "] => List of currentNode neighbours size : " + list.Count);
		return list;
	}
}
