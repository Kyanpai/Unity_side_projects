using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFallTrigger : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if (!GameManager.gm.GameOver)
			GameManager.gm.SetGameOver();
	}
}
