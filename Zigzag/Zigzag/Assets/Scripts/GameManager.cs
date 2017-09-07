using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

	public static GameManager gm;
	public bool GameOver;
	private int Score;
	private int BestScore;

	[Header("GUI")]
	public TextMeshProUGUI ScoreText;

	[Header("EndPanel")]
	public GameObject GameOverPlanel;
	public TextMeshProUGUI FinalScoreText;
	public TextMeshProUGUI FinalBestScoreText;

	// Use this for initialization
	void Start () {
		if (gm == null)
			gm = this;

		Score = 0;
		GameOver = false;
		BestScore = PlayerPrefs.GetInt("BestScore");
		GameOverPlanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
			ReloadScene();
		ScoreText.text = Score.ToString();
	}

	public int GetScore()
	{
		return Score;
	}

	public void AddToScore(int value)
	{
		if (!GameOver)
			Score += value;
	}

	public void SetGameOver()
	{
		GameOver = true;
		if (Score > BestScore)
		{
			PlayerPrefs.SetInt("BestScore", Score);
			BestScore = Score;
		}
		GameOverPlanel.SetActive(true);
		FinalScoreText.text = Score.ToString();
		FinalBestScoreText.text = BestScore.ToString();
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ResetPlayerPrefsScore()
	{
		PlayerPrefs.SetInt("BestScore", 0);
		BestScore = 0;
	}
}
