using UnityEngine;
using UnityEditor;

public class NodeConverter : EditorWindow {

	[MenuItem("Window/NodeConverter")]
	public static void ShowWindow() {
		GetWindow<NodeConverter>();
	}

	private void OnGUI() {
		GUILayout.Label("Select items and click the button", EditorStyles.boldLabel);

		if (GUILayout.Button("Node them all!")) {
			foreach( GameObject obj in Selection.gameObjects) {
				obj.layer = 8;

				if (obj.GetComponent<BoxCollider>() == null)
					obj.AddComponent<BoxCollider>();

				Rigidbody rb = obj.AddComponent<Rigidbody>();
				rb.useGravity = false;
				rb.isKinematic = true;

				GameObject node = new GameObject("Node");
				node.transform.parent = obj.transform;
				node.AddComponent<Node>();
				node.transform.localPosition = new Vector3(0, 0.5f, 0);
				node.layer = 8;

				BoxCollider nodeBoxCollider = node.AddComponent<BoxCollider>();
				nodeBoxCollider.size = new Vector3(.05f, .05f, .05f);
				nodeBoxCollider.isTrigger = true;

				CreateBoundary(0, node).transform.localPosition = new Vector3(0.5f, 0, 0);
				CreateBoundary(1, node).transform.localPosition = new Vector3(-0.5f, 0, 0);
				CreateBoundary(2, node).transform.localPosition = new Vector3(0, 0, 0.5f);
				CreateBoundary(3, node).transform.localPosition = new Vector3(0, 0, -0.5f);

			}
		}
	}

	private GameObject CreateBoundary(int i, GameObject parent) {
		GameObject Boundary = new GameObject("Boundary" + i.ToString());
		Boundary.AddComponent<Link>();
		Boundary.transform.parent = parent.transform;
		Boundary.tag = "Boundary";
		BoxCollider box = Boundary.AddComponent<BoxCollider>();
		box.size = new Vector3(.2f, .2f, .2f);
		box.isTrigger = true;
		return Boundary;
	}
}
