using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			TileManager.tileManager.SpawnTile();
			StartCoroutine(Tilefall());
			GameManager.gm.AddToScore(1);
		}
	}

	IEnumerator Tilefall()
	{
		float currentY = transform.parent.position.y;
		while (currentY - transform.parent.position.y < 20)
		{
			transform.parent.position -= new Vector3(0, 0.1f, 0);
			yield return null;
		}
		Destroy(transform.parent.gameObject);
	}
}
