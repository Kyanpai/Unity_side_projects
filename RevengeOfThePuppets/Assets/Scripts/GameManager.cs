using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OnPlayerLooseEvent : GameEvent {}
public class OnIncreaseDifficultyEvent : GameEvent {
	public GameManager.Difficulty newDifficulty;
}
public class OnPauseEvent : GameEvent {
	public bool pause;
}

public class OnGameStartEvent : GameEvent { }

public class GameManager : MonoSingleton<GameManager> {

	[HideInInspector]
	public bool isPlaying;

	[HideInInspector]
	public bool pause;

	public int score;
	public enum Difficulty {
		VERYEASY,
		EASY,
		MEDIUM,
		HARD,
		VERYHARD,
		IMPOSSIBLE
	}
	private Difficulty currentDifficulty = Difficulty.VERYEASY;
	private float time;
	private float increaseScoreTime = .1f;

	public enum Lane {
		LEFT,
		MIDDLE,
		RIGHT
	}

	[System.Serializable]
	public struct LanePosition {
		public Lane lane;
		public float position;
	}

	public List<LanePosition> LanesPositions = new List<LanePosition>();

	[Header("UI")]
	[SerializeField]
	TextMeshProUGUI ScoreText;
	[SerializeField]
	TextMeshProUGUI SpecialPointsText;
	[SerializeField]
	Animator SpecialScoreAnim;
	[SerializeField]
	TextMeshProUGUI WelcomePanelBestScore;

	private void Start() {
		isPlaying = false;
		WelcomePanelBestScore.text = "Beat your best score: " + PlayerPrefs.GetInt("PlayerBestScore", 0);
	}

	private void OnEnable() {
		Events.Instance.AddListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.AddListener<OnEnemyHasBeenKilledEvent>(HandleEnemyKilledEvent);
	}

	private void OnDisable() {
		Events.Instance.RemoveListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.RemoveListener<OnEnemyHasBeenKilledEvent>(HandleEnemyKilledEvent);
	}

	private void HandleOnPlayerLoose(OnPlayerLooseEvent e) {
		isPlaying = false;
		if (score > PlayerPrefs.GetInt("PlayerBestScore", 0)) {
			PlayerPrefs.SetInt("PlayerBestScore", score);
			UIManager.Instance.DisplayEndGame(score, true);
		} else {
			UIManager.Instance.DisplayEndGame(PlayerPrefs.GetInt("PlayerBestScore"));
		}
	}

	private void HandleEnemyKilledEvent(OnEnemyHasBeenKilledEvent e) {
		IncreaseScore(e.pointsGiven, true);
	}

	private void TogglePause() {
		if (pause == false) {
			UIManager.Instance.DisplayPauseMenu();
		} else {
			UIManager.Instance.HideMenu();
		}
		pause = !pause;
		Events.Instance.Raise(new OnPauseEvent { pause = pause });
	}

	private void Update() {
		if (isPlaying == false)
			return;

		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePause();
		}

		if (pause)
			return;

		time += Time.deltaTime;

		if (time > increaseScoreTime) {
			IncreaseScore(1);
			time = 0;
		}
	}

	public void IncreaseScore(int value, bool special = false) {
		score += value;
		ScoreText.text = score.ToString();

		if (special) {
			SpecialPointsText.text = "+" + value.ToString();
			SpecialScoreAnim.SetTrigger("GainPoints");
		}

		if (score / 400 > (int)currentDifficulty) {
			if ((int)currentDifficulty + 1 < Enum.GetValues(typeof(Difficulty)).Length)
				currentDifficulty++;
			Events.Instance.Raise(new OnIncreaseDifficultyEvent { newDifficulty = currentDifficulty });
		}
	}

	public void PauseGame() {

	}

	public void ResumeGame() {
		TogglePause();
	}

	public void ReloadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void Play() {
		isPlaying = true;
		Events.Instance.Raise(new OnGameStartEvent { });
		UIManager.Instance.HideMenu();
	}
}
