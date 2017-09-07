using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSides : MonoBehaviour {

	[Header("Position")]
	public bool verticalRotation;
	public bool movement;

	[Space]

	[Header("Properties")]

	public float speed = 300;
	public LayerMask mask;
	private GameObject selectedObject;


	void Update () {
		if (GameManager.gm.levelEnded)
			return;
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100, mask))
				selectedObject = hit.transform.gameObject;
			else
				selectedObject = null;
		} else if (Input.GetMouseButtonUp (0))
			selectedObject = null;

		if (Input.GetMouseButton (0) && selectedObject != null) {
			if (movement && Input.GetKey(KeyCode.LeftShift))
				selectedObject.transform.Translate (new Vector3 (0, Input.GetAxis ("Mouse Y"), 0) * Time.deltaTime * speed / 3, Space.World);
			else if (verticalRotation && Input.GetMouseButton(1))
				selectedObject.transform.Rotate (new Vector3 (Input.GetAxis ("Mouse X"), 0, 0) * Time.deltaTime * speed, Space.World);
			else if (verticalRotation && Input.GetKey (KeyCode.LeftControl))
				selectedObject.transform.Rotate (new Vector3 (0, 0, Input.GetAxis ("Mouse Y")) * Time.deltaTime * speed, Space.World);
			else
				selectedObject.transform.Rotate (new Vector3 (0, Input.GetAxis ("Mouse X"), 0) * Time.deltaTime * speed, Space.World);
			Vector3 tmp = selectedObject.transform.position;
			if (movement)
				tmp.y = Mathf.Clamp (selectedObject.transform.position.y, selectedObject.GetComponent<objects>().originY - 10, selectedObject.GetComponent<objects>().originY + 10);
			selectedObject.transform.position = tmp;
		}
	}
}