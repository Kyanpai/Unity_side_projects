using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour {

	public List<MoveUp> moveUpScript = new List<MoveUp>();
	bool isActive;
	Vector3 originPosition;

	private void OnCollisionEnter(Collision collision) {
		if (!isActive)
			return;

		if (collision.transform.tag == "Player") {
			//Lancer l'animation
			foreach(MoveUp script in moveUpScript)
				script.alreadyMoved = true;
			isActive = false;
			StartCoroutine(DisableButton());
		}
	}

	// Use this for initialization
	void Start () {
		isActive = true;
	}
	
	IEnumerator DisableButton() {
		originPosition = transform.position;
		Vector3 targetPosition = originPosition - new Vector3(0, 0.1f, 0f);
		while (transform.position != targetPosition) {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.005f);
			yield return null;
		}
//		transform.GetComponent<Renderer>().sharedMaterial.color = Color.grey;
	}
}
