using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

	public static GameManager gm;

	#region Public Variables
	[Header("Prefabs")]
	public List<GameObject> PlatformsList;
	public int InitialPlatformNb;
	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject LastPlatform;

	public GameObject LastBackground;
	public GameObject BackgroundPrefab;

	[Header("Player")]
	public GameObject Player;

	[Header("GUI")]
	public TextMeshProUGUI ScoreText;
	public GameObject EndPanel;
	public TextMeshProUGUI BestScoreText;
	public GameObject RetryArrow;
	public GameObject QuitArrow;

	#endregion

	#region Private Variables
	private int BackgroundCount = 0;

	private float PlayerRadius;

	private float Score;
	private float PlayerMaxY;
	private float PlayerInitialY;

	private bool end;

	private EndGameOptions CurrentOption;

	private enum EndGameOptions { Retry, Quit };

	private enum PlatformType { Small, Medium, Big };

	#endregion

	#region Endgame

	public void EndGame() {
		StartCoroutine(ArrowCoroutine());
		end = true;
		CurrentOption = EndGameOptions.Retry;
		Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		if (Score > PlayerPrefs.GetInt("BestScore")) {
			PlayerPrefs.SetInt("BestScore", Mathf.RoundToInt(Score));
		}
		BestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString() + "pts";
		EndPanel.SetActive(true);
	}

	private void ChangeEndGameOption() {
		if (CurrentOption == EndGameOptions.Quit) {
			CurrentOption = EndGameOptions.Retry;
			QuitArrow.SetActive(false);
			RetryArrow.SetActive(true);
		} else {
			CurrentOption = EndGameOptions.Quit;
			QuitArrow.SetActive(true);
			RetryArrow.SetActive(false);
		}
	}

	private void ValidateEndGameOption() {
		if (CurrentOption == EndGameOptions.Quit) {
			Application.Quit();
		} else {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	IEnumerator ArrowCoroutine() {
		while (true) {
			if (CurrentOption == EndGameOptions.Quit)
				QuitArrow.SetActive(!QuitArrow.activeSelf);
			else
				RetryArrow.SetActive(!RetryArrow.activeSelf);
			yield return new WaitForSeconds(.5f);
		}
	}

	#endregion

	#region Procedural generation

	private void CreateBackground() {
		LastBackground = GameObject.Instantiate(BackgroundPrefab, new Vector3(LastBackground.transform.position.x, LastBackground.transform.position.y  + 12f, 0), Quaternion.identity);
		BackgroundCount = 0;
	}

	public void CreatePlatform() {
		PlatformType nextPlatform = (PlatformType)Random.Range(0, PlatformsList.Count);
		float CurrentPlatformRadius = LastPlatform.GetComponent<CircleCollider2D>().radius;
		float PlatformRadius = PlatformsList[(int)nextPlatform].GetComponent<CircleCollider2D>().radius;

		LastPlatform = GameObject.Instantiate(PlatformsList[(int)nextPlatform], new Vector3(Random.Range(LeftWall.transform.position.x + 1 + PlatformRadius + PlayerRadius, RightWall.transform.position.x - 1 - PlatformRadius - PlayerRadius),
	Random.Range(LastPlatform.transform.position.y + CurrentPlatformRadius + PlatformRadius + PlayerRadius + 1f, LastPlatform.transform.position.y + CurrentPlatformRadius + PlatformRadius + PlayerRadius + 2),
	0), Quaternion.identity);

		LastPlatform.GetComponent<RotatePlatform>().speed = Random.Range(150, 500);
		if (Random.Range(0, 2) == 1) {
			LastPlatform.GetComponent<RotatePlatform>().speed *= -1;
		}

		BackgroundCount++;
		if (BackgroundCount > 2) {
			CreateBackground();
		}
	}

	private void UpdateScore() {
		PlayerMaxY = Player.transform.position.y;
		Score = (PlayerMaxY - PlayerInitialY) * 10;
		ScoreText.text = Mathf.RoundToInt(Score) + "pts";
	}

	#endregion

	// Use this for initialization
	void Start () {
		if (gm == null)
			gm = this;

		PlayerRadius = Player.GetComponent<CircleCollider2D>().radius;
		PlayerInitialY = PlayerMaxY = Player.transform.position.y;
		UpdateScore();
		Score = 0;

		for (int i = 0; i < InitialPlatformNb; i++) {
			CreatePlatform();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (Player.transform.position.y > PlayerMaxY) {
			UpdateScore();
		}

		if (end && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))) {
			ChangeEndGameOption();
		}

		if (end && Input.GetKeyDown(KeyCode.Return)) {
			ValidateEndGameOption();
		}
	}
}
