using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {


	public static TileManager tileManager;

	public GameObject tile;
	public GameObject currentTile;

	private int tileCount = 0;

	public void SpawnTile()
	{
		currentTile = GameObject.Instantiate(tile, currentTile.transform.GetChild(Random.Range(0,2)).transform.position, Quaternion.identity);
		tileCount++;
		if (tileCount == 15)
		{
			TorchManager.torchManager.SpawnTorch(currentTile.transform.position);
			tileCount = 0;
		}
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 40; i++)
			SpawnTile();
		if (tileManager == null)
			tileManager = this;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator tileCreationCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(.2f);
			SpawnTile();
		}
	}
}
