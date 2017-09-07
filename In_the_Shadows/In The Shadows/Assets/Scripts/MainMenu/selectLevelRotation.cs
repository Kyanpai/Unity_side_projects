using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class selectLevelRotation : MonoBehaviour {

	[Header("Objects")]
	public GameObject currentObject;
	public ParticleSystem particles;
	public Text titleObject;


	[Space]

	[Header("Other elements")]

	public GameObject lightSpot;
	public GameObject waitingRoom;
	public Vector3 displayObjectPosition;

	void Start() {
		if (!currentObject.GetComponent<elements> ().unlocked)
			lightSpot.GetComponent<Light> ().color = new Color (238/255f, 37/255f, 37/255f, 1);
		else if (!currentObject.GetComponent<elements> ().nextElem.GetComponent<elements>().unlocked)
			lightSpot.GetComponent<Light> ().color = new Color (205/255f, 174/255f, 30/255f, 1);
		else
			lightSpot.GetComponent<Light> ().color = new Color (255/255f, 244/255f, 214/255f, 1);
		titleObject.text = currentObject.GetComponent<elements> ().title;
		if (PlayerPrefs.GetInt("mod") == 2 && PlayerPrefs.HasKey("unlockedLevel"))
			StartCoroutine (animateUnlockedItem ());
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.RightArrow))
			StartCoroutine (moveObject (currentObject.GetComponent<elements> ().nextElem));
		else if (Input.GetKeyDown (KeyCode.LeftArrow))
			StartCoroutine (moveObject (currentObject.GetComponent<elements> ().prevElem));
		else if (Input.GetKeyDown (KeyCode.Return)) {
			if (currentObject.GetComponent<elements> ().unlocked)
				SceneManager.LoadSceneAsync (currentObject.GetComponent<elements> ().level);
		} else if (Input.GetKeyDown (KeyCode.Escape))
			SceneManager.LoadScene (0);
	}

	IEnumerator moveObject(GameObject elem) {
		GetComponent<AudioSource> ().Play ();
		lightSpot.SetActive (false);
		currentObject.transform.position = waitingRoom.transform.position;
		elem.transform.position = elem.GetComponent<elements>().goodPosition;
		yield return new WaitForSeconds (0.6f);
		currentObject = elem;
		if (!currentObject.GetComponent<elements> ().unlocked)
			lightSpot.GetComponent<Light> ().color = new Color (238/255f, 37/255f, 37/255f, 1);
		else if (!currentObject.GetComponent<elements> ().done && currentObject.GetComponent<elements> ().unlocked)
			lightSpot.GetComponent<Light> ().color = new Color (205/255f, 174/255f, 30/255f, 1);
		else
			lightSpot.GetComponent<Light> ().color = new Color (255/255f, 244/255f, 214/255f, 1);
		lightSpot.SetActive (true);
		titleObject.text = currentObject.GetComponent<elements> ().title;
	}

	IEnumerator animateUnlockedItem() {
		while (currentObject.GetComponent<elements> ().level != PlayerPrefs.GetInt ("unlockedLevel")) {
			StartCoroutine(moveObject(currentObject.GetComponent<elements>().nextElem));
				yield return new WaitForSeconds(0.7f);
		}
			
		particles.Play ();
		particles.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (3f);
		particles.Stop ();
		PlayerPrefs.DeleteKey ("unlockedLevel");
	}
}
