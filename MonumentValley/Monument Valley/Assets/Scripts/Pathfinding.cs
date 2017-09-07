using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {


//	public List<Node> path = new List<Node>();

	public List<Node> FindPath(Node startNode, Node targetNode)
	{
		List<Node> finalPath = new List<Node>();
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet[0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			if (currentNode == targetNode) {
				finalPath = RetracePath(startNode, targetNode);
				return finalPath;
			}

			foreach (Node neighbour in currentNode.GetNeighbours()) {
				if (closedSet.Contains(neighbour))
					continue;

				neighbour.gCost = currentNode.gCost + 1;
				neighbour.hCost = 0;

				neighbour.ParentNode = currentNode;

				openSet.Add(neighbour);
			}

		}
		return null;
	}

	private List<Node> RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		path.Add(currentNode);

		while (currentNode != startNode) {
			currentNode = currentNode.ParentNode;
			path.Add(currentNode);
		}

		path.Reverse();
		return path;

	}

}
