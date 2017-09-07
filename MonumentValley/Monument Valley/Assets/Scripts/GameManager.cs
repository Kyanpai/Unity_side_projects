using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager gm;
	public GameObject cube;
	private MeshRenderer cubeRenderer;
	public bool end;

	public void TriggerEnd() {
		end = true;
		StartCoroutine(makeCubeAppear());
	}

	// Use this for initialization
	void Start () {
		if (gm == null)
			gm = this;

		end = false;
		cubeRenderer = cube.GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if (end) {
			cube.transform.Rotate(new Vector3(0, 1, 0), Space.World);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	IEnumerator makeCubeAppear() {
		while (cubeRenderer.sharedMaterial.color.a != 1) {
			cubeRenderer.material.color = new Color(cubeRenderer.material.color.r, cubeRenderer.material.color.g, cubeRenderer.material.color.b, cubeRenderer.material.color.a + .01f);
			yield return null;
		}
	}
}
