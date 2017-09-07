using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour {

	public TurnPlatforms towerTurnScript;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player")
			towerTurnScript.canRotate = false;
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player")
			towerTurnScript.canRotate = true;
	}
}
