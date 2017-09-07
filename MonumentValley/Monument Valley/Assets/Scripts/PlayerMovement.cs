using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Pathfinding pathFinding;
	private List<Vector3> path = new List<Vector3>();
	private Vector3 nextNode;
	private Node[] allNodes;

	public LayerMask mask;
	public Node currentNode;

	public float speed;

	private Animator animator;

	// Use this for initialization
	void Start () {
		pathFinding = GetComponent<Pathfinding>();
		allNodes = FindObjectsOfType<Node>();
		animator = GetComponentInChildren<Animator>();
	}
	
	private void LookAtNextNode() {
		float tmpx = Mathf.Clamp(transform.position.x - nextNode.x, -1, 1);
		float tmpz = Mathf.Clamp(transform.position.z - nextNode.z, -1, 1);

//		Vector3 nextTransform = new Vector3(tmpx, transform.position.y, tmpz);

		Debug.Log(Mathf.Round(tmpx / .1f) * .1f + " / " + Mathf.Round(tmpz / .1f) * .1f);

		Vector3 nextTransform = new Vector3(nextNode.x, transform.position.y, nextNode.z);

		transform.LookAt(nextTransform);
	}

	private void ChangeNearestNode() {
		// Get nearest node
		Collider[] nearestNode = Physics.OverlapSphere(transform.position, 1f);
		float distance;
		float nearestDistance;
		if (currentNode != null)
			nearestDistance = Vector3.Distance(transform.position, currentNode.transform.position);
		nearestDistance = 100;
		for (int i = 0; i < nearestNode.Length; i++) {
			if (nearestNode[i].GetComponent<Node>() == null)
				continue;
			distance = Vector3.Distance(transform.position, nearestNode[i].transform.gameObject.GetComponent<Node>().transform.position);
			if (distance < nearestDistance)
				currentNode = nearestNode[i].GetComponent<Node>();
		}
	}

	// Update is called once per frame
	void Update () {

		if (GameManager.gm.end) {
			animator.SetBool("isWalking", false);
			return;
		}

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f, mask)) {
				// clean all nodes
				foreach (Node _node in allNodes) {
					_node.ParentNode = null;
				}

				ChangeNearestNode();

				List<Node> globalPath = pathFinding.FindPath(currentNode, hit.transform.gameObject.GetComponentInChildren<Node>());
				path = SimplifyPath(globalPath);
				if (path != null && path.Count > 0) {
					nextNode = path[0];
					//		LookAtNextNode();
				}
			}
		}

		if (nextNode != Vector3.zero) {
			animator.SetBool("isWalking", true);
			//			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			//	transform.Translate(Vector3.forward * Time.deltaTime * speed);
			//			transform.position = Vector3.MoveTowards(transform.position, path[path.Count - 1].transform.position + new Vector3(0, .50f, .75f), .05f);
			if (Mathf.Abs((transform.position.y - 0.5f) - nextNode.y) > 1) {
				if (transform.position.y > nextNode.y)
					transform.position = nextNode + new Vector3(0, .55f, -0.15f);
				else
					transform.position = nextNode;
				ChangeNextNode();
			} else {
				transform.position = Vector3.MoveTowards(transform.position, nextNode + new Vector3(0, .5f, 0), .05f);
				LookAtNextNode();

			}
		} else
			animator.SetBool("isWalking", false);

		//		if (path.Count > 0 && nextNode == null) {
		//			nextNode = path[0];
		//	Debug.Log(nextNode.transform.position.y + " => " + transform.position.y);
		//			if (nextNode.transform.position.y != transform.position.y - 0.5) {
		//			transform.position = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y + 0.5f, nextNode.transform.position.z + 1f);
		//				transform.position += new Vector3(nextNode.transform.position.x - transform.position.x, nextNode.transform.position.y - transform.position.y + 0.5f, Mathf.Abs(transform.position.z - nextNode.transform.position.z));
		//			}
		//		}

		if (nextNode != Vector3.zero && transform.position - new Vector3(0, 0.5f, 0) == nextNode) {
			ChangeNextNode();
		}
	}

	private void ChangeNextNode() {
		if (path.Count > 0) {
			path.RemoveAt(0);
			if (path.Count > 0) {
				nextNode = path[0];
			} else {
				nextNode = Vector3.zero;
			}
		}
	}

	private void OnDrawGizmos() {
		if (path.Count > 0) {
			foreach (Vector3 pos in path) {
				//	Gizmos.color = new Color32(76, 175, 80, 255);
				Gizmos.DrawSphere(pos, .25f);
			}
		}
		if (currentNode != null)
			Gizmos.DrawCube(currentNode.transform.position, new Vector3(.5f, .5f, .5f));
	}

	private Vector3 GetLinkBetweenNodes(Node prev, Node next) {
		foreach (Link link in prev.Links) {
			foreach (Link subLink in link.linkedNodes) {
				if (subLink.attachedNode == next) {
					if (prev.transform.position.y < next.transform.position.y)
						return new Vector3(1.142f, 9.687f, 13.051f);
					//						return subLink.transform.position + new Vector3(0, .46f, -.35f);
					else {
						return link.transform.position + new Vector3(0, 0, .25f);
					}
				}
			}
		}
		return Vector3.zero;
	}

	private List<Vector3> SimplifyPath(List<Node> globalPath) {
		List<Vector3> simplePath = new List<Vector3>();

		if ( globalPath == null || globalPath.Count == 0)
			return simplePath;

		simplePath.Add(globalPath[0].transform.position);
//		Debug.Log(globalPath[0].transform.position);

		foreach (Node node in globalPath) {
			if (node.ParentNode != null) {
				if (Mathf.Abs(node.transform.position.y - node.ParentNode.transform.position.y) > 1) {
					simplePath.Add(node.ParentNode.transform.position);
		//			Debug.Log(node.ParentNode.transform.position);
					simplePath.Add(GetLinkBetweenNodes(node.ParentNode, node));
			//		Debug.Log(GetLinkBetweenNodes(node.ParentNode, node));
					simplePath.Add(node.transform.position);
				//	Debug.Log(node.transform.position);
				} else {
					simplePath.Add(node.transform.position);
				}
			}
		}

		simplePath.Add(globalPath[globalPath.Count - 1].transform.position);

		return simplePath;
	}

	public void StepOnNode(Node node) {
	//	currentNode = node;
		/*
		if (node == nextNode) {
			nextNode = null;
			if (path.Count > 0)
				path.RemoveAt(0);
		} */
	}

	public void StepOnLink(Vector3 pos) {
	//	transform.position = pos + new Vector3(0, .5f, 0);
	}
}
