using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoPieces : MonoBehaviour {

	public void CheckPieceDestroy(int y) {
		if (transform.position.x > 11)
			return;
		if (Mathf.RoundToInt(transform.position.y) == y) {
			Destroy (gameObject, .1f);
		} else if (y < Mathf.RoundToInt(transform.position.y)) {
			transform.position = new Vector3 (Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 1, 0);
		}
	}
}
