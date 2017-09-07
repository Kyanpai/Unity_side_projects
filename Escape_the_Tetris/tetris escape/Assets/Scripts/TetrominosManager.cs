using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosManager : MonoBehaviour {

	private List<int[,]> TetrominosList = new List<int[,]>();
	public List<TetrominoBehaviour> TetrominosGO = new List<TetrominoBehaviour>();
	public static TetrominosManager TetroManager;
	public Transform nextPieceLocation;
	public TetrominoBehaviour nextTetromino;

	private TetrominoBehaviour SpawnTetrominos() {
		int random = Random.Range (0, TetrominosList.Count);
		TetrominoBehaviour newTetromino = GameObject.Instantiate (TetrominosGO [random], nextPieceLocation.position, Quaternion.identity);
		newTetromino.matrix = TetrominosList [random];
		//Special case for the line and the box
		if (newTetromino.matrix.GetLength (0) == 5)
			newTetromino.transform.position += new Vector3 (0.5f, 0, 0);
		if (newTetromino.matrix.GetLength (0) == 2)
			newTetromino.transform.position += new Vector3 (0.5f, -0.5f, 0);
		return newTetromino;
	}

	public void GetNextTetromino() {
		if (nextTetromino == null)
			nextTetromino = SpawnTetrominos ();
		nextTetromino.transform.position = new Vector3 (GameManager.gm.GridWidth / 2, GameManager.gm.GridHeight - 2, 0);
		nextTetromino.isCurrentTetromino = true;
		// Check if new tetrimino isn't already stuck
		int[] matrixOrigins = nextTetromino.GetMatrixOrigins (nextTetromino.transform.position);
		TetrominoBehaviour.MovementState state = nextTetromino.CanMove (GameManager.gm.Grid, nextTetromino.matrix, matrixOrigins [0], matrixOrigins [1]);
		if (state != TetrominoBehaviour.MovementState.CAN_MOVE)
			GameManager.gm.GameOverMaxTetrominos ();

		nextTetromino = SpawnTetrominos ();


	}

	// In start() we're going to create all tetrominos' matrices
	void Start () {
		
		if (TetroManager == null)
			TetroManager = this;

		TetrominosList.Add (
			new int[,] {
				{0, 0, 0},
				{0, 1, 0},
				{1, 1, 1}
			}
		);

		TetrominosList.Add (
			new int[,] {
				{0, 0, 0},
				{1, 0, 0},
				{1, 1, 1}
			}
		);

		TetrominosList.Add (
			new int[,] {
				{0, 0, 0},
				{0, 0, 1},
				{1, 1, 1}
			}
		);

		TetrominosList.Add (
			new int[,] {
				{1, 1, 0},
				{0, 1, 1},
				{0, 0, 0}
			}
		);

		TetrominosList.Add (
			new int[,] {
				{0, 1, 1},
				{1, 1, 0},
				{0, 0, 0}
			}
		);

		TetrominosList.Add (
			new int[,] {
				{1, 1},
				{1, 1},
			}
		);

		TetrominosList.Add (
			new int[,] {
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{1, 1, 1, 1, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
			}
		);

		StartCoroutine (LateStart ());
	}

	// We need coroutine so the gameManager has time to initialize
	IEnumerator LateStart() {
		yield return new WaitForSeconds(.5f);
	//	GetNextTetromino ();
	}


	void Update () {
		
	}
}
