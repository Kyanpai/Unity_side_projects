using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour {

	public static TorchManager torchManager;
	public GameObject torch;
	public float offset;

	public void SpawnTorch(Vector3 position)
	{
		GameObject.Destroy(GameObject.Instantiate(torch, position + new Vector3(-offset, 2f, -offset) , Quaternion.identity), 30f);
		GameObject.Destroy(GameObject.Instantiate(torch, position + new Vector3(offset, 2f, offset), Quaternion.identity), 30f);
	}

	private void Start()
	{
		if (torchManager == null)
			torchManager = this;
	}
}
