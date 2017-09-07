using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using TMPro.Examples;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour {

	public static GameManager gm;
	public int GridHeight;
	public int GridWidth;

	public int[,] Grid;

	private float ingameTime;
	private float timeScale;
	public bool pause = false;
	public bool gameOver = false;
	private bool GameBegan = false;
	private bool win = false;

	public TextMeshProUGUI timer;

	public PlayerBehaviour Player;

	public MessageTrigger PauseTrigger;
	public MessageTrigger GameoverTrigger;
	public MessageTrigger GlobalMessagesTrigger;
	public MessageTrigger MainMenuTrigger;
	public MessageTrigger WinMessageTrigger;

	private LateralArrowsBehaviour[] arrows;

	public bool resetPlayerPrefs = false;

	public void DisplayGrid() {
		String GridStr = "";

		for (int y = GridHeight - 1; y >= 0; y--) {
			for (int x = 0; x < GridWidth ; x++)
				GridStr += Grid[x, y] + " ";
			GridStr += "\n";
		}

		Debug.Log (GridStr);
	}

	public void ModifyGrid(int [,] matrice, int x, int y, int value) {
		for (int matriceX = 0; matriceX < matrice.GetLength (0); matriceX++) {
			for (int matriceY = 0; matriceY < matrice.GetLength (0); matriceY++) {
				int gridPosX = x + matriceX;
				int gridPosY = y - matriceY;
				if (matrice [matriceY, matriceX] != 0) {
					Grid [gridPosX, gridPosY] = value;
				}
			}
		}
	}

	public void DeleteLineFromGrid(int y) {
		SoundManager.soundManager.PlayLineComplete ();
		TetrominoPieces[] pieces =  FindObjectsOfType (typeof(TetrominoPieces)) as TetrominoPieces[];
		foreach (TetrominoPieces piece in pieces) {
			piece.CheckPieceDestroy (y);
		}

		for (int gridY = y; gridY < GridHeight; gridY++) {
			for (int gridX = 0; gridX < GridWidth; gridX++) {
				if (gridY == y)
					Grid [gridX, gridY] = 0;
				else {
					Grid [gridX, gridY - 1] = Grid [gridX, gridY];
					Grid [gridX, gridY] = 0;
				}
			}
		}
	}

	public void CheckLineComplete() {
		bool lineFull;
		int y = 0;
		bool playerDead = false;

		while (y < GridHeight) {
			lineFull = true;
			playerDead = false;
			for (int x = 0; x < GridWidth; x++) {
				if (Grid [x, y] == 0) {
					lineFull = false;
					break;
				} else if (Grid [x, y] == 2)
					playerDead = true;
			}
			if (lineFull) {
				if (playerDead)
					GameOver ();
				DeleteLineFromGrid (y);
				y = 0;
			} else
				y++;
		}
	}

	public void PauseFunction() {
		if (pause) {
			Time.timeScale = timeScale;
			pause = false;
			GlobalMessagesTrigger.TriggerMessage ();
		} else {
			pause = true;
			timeScale = Time.timeScale;
			Time.timeScale = 0;
			PauseTrigger.TriggerMessage ();
		}
	}

	public void GameOver() {
		if (!gameOver) {
			GameoverTrigger.TriggerMessage ();
			Player.GetComponentInChildren<ParticleSystem> ().Play ();
			SoundManager.soundManager.PlayPlayerDie ();
			gameOver = true;
		}
	}

	public void GameOverMaxTetrominos() {
		if (!gameOver) {
			gameOver = true;
		}
	}

	public void Win() {
		if (!win) {
			if (PlayerPrefsManager.ppManager.getBestScore () > ingameTime || PlayerPrefsManager.ppManager.getBestScore () == 0) {
				PlayerPrefsManager.ppManager.storeBestScore (Mathf.RoundToInt (ingameTime));
				WinMessageTrigger.message.messages.Add ("\nNew Best Score: " + Mathf.RoundToInt(ingameTime) + "s!");
			}
			WinMessageTrigger.TriggerMessage ();
			SoundManager.soundManager.PlayWin ();
			foreach (LateralArrowsBehaviour arrow in arrows) {
				arrow.gameObject.SetActive (false);
			}
			win = true;
		}
	}

	// Use this for initialization
	void Start () {
		if (gm == null)
			gm = this;

		Grid = new int[GridWidth, GridHeight];

		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth ; x++) {
				Grid [x, y] = 0;
			}
		}
		StartCoroutine (LateStart ());

		arrows = (LateralArrowsBehaviour[])FindObjectsOfType (typeof(LateralArrowsBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
		if (GameBegan && !win && !gameOver)
			ingameTime += Time.deltaTime;

		if (!GameBegan && Input.GetKeyDown(KeyCode.Return)) {
			GameBegan = true;
			GlobalMessagesTrigger.TriggerMessage ();
			TetrominosManager.TetroManager.GetNextTetromino ();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		if (GameBegan && !gameOver && Input.GetKeyDown(KeyCode.Space)) {
			PauseFunction ();
		}
		if (!gameOver)
			timer.text = "Timer\n\n" + Mathf.RoundToInt(ingameTime).ToString();

		if (pause && Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	IEnumerator LateStart() {
		yield return new WaitForSeconds (.5f);
		MainMenuTrigger.TriggerMessage ();
		if (resetPlayerPrefs)
			PlayerPrefsManager.ppManager.storeBestScore (0);

		if (PlayerPrefsManager.ppManager.getBestScore () != 0)
			GlobalMessagesTrigger.message.messages.Add ("\nCurrent best score: " + Mathf.RoundToInt (PlayerPrefsManager.ppManager.getBestScore ()) + "s");
	}
}
