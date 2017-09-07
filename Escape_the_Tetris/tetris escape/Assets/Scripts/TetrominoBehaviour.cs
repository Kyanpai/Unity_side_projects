using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class TetrominoBehaviour : MonoBehaviour {

	private float delay = .5f;
	private float tmpTime = 0f;

	[HideInInspector]
	public bool isCurrentTetromino = false;

	public int[,] matrix;

	public enum MovementState {
		CAN_MOVE,
		WALL,
		OBSTACLE,
		PLAYER
	}

	private void DisplayMatrix() {
		String str = "";
		for (int matriceX = 0; matriceX < matrix.GetLength(0); matriceX++) {
			for (int matriceY = 0; matriceY < matrix.GetLength(0); matriceY++) {
				str += matrix[matriceX, matriceY] + " ";
			}
			str += "\n";
		}
		Debug.Log (str);
	}

	public int[] GetMatrixOrigins(Vector3 pos) {
		int[] ret;
		ret = new int[2] {
			(int)pos.x - (matrix.GetLength(0) / 2),
			(int)pos.y + (matrix.GetLength(0) / 2)
		};
		return ret;
	} 

	private int[,] RotateMatrix(int [,] matrix) {
		int[,] newMatrix = new int[matrix.GetLength (0), matrix.GetLength (1)];
		int newColumn, newRow = 0;
		for (int oldColumn = matrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
		{
			newColumn = 0;
			for (int oldRow = 0; oldRow < matrix.GetLength(0); oldRow++)
			{
				newMatrix[newRow, newColumn] = matrix[oldRow, oldColumn];
				newColumn++;
			}
			newRow++;
		}
		return newMatrix;
	}

	private void MovePiece(Vector3 movement, bool MoveDown) {
		int[] PrevMatrixOrigins = GetMatrixOrigins (transform.position);
		GameManager.gm.ModifyGrid (matrix, PrevMatrixOrigins [0], PrevMatrixOrigins [1], 0);

		int[] matrixOrigins = GetMatrixOrigins (transform.position + movement);

		MovementState state = CanMove (GameManager.gm.Grid, matrix, matrixOrigins [0], matrixOrigins [1]);

		if (state == MovementState.CAN_MOVE) {
			GameManager.gm.ModifyGrid (matrix, matrixOrigins [0], matrixOrigins [1], 1);
			transform.position += movement;
			if (MoveDown)
				tmpTime = 0;
		}

		//Specials states if we move down
		if (MoveDown) {
			if (state == MovementState.OBSTACLE) {
				GameManager.gm.ModifyGrid (matrix, PrevMatrixOrigins [0], PrevMatrixOrigins [1], 1);
				GameManager.gm.CheckLineComplete ();
				TetrominosManager.TetroManager.GetNextTetromino ();
				isCurrentTetromino = false;
			} else if (state == MovementState.PLAYER) {
				GameManager.gm.GameOver ();
			}
		}
	}
		
	private void RotatePiece() {
		int[] PrevMatrixOrigins = GetMatrixOrigins (transform.position);
		GameManager.gm.ModifyGrid (matrix, PrevMatrixOrigins [0], PrevMatrixOrigins [1], 0);

		int[] matrixOrigins = GetMatrixOrigins (transform.position);
		if (CanMove (GameManager.gm.Grid, RotateMatrix (matrix), matrixOrigins [0], matrixOrigins [1]) == MovementState.CAN_MOVE) {
			GameManager.gm.ModifyGrid (RotateMatrix(matrix), matrixOrigins [0], matrixOrigins [1], 1);
			if (matrix.GetLength (0) != 2) {
				transform.localRotation = transform.localRotation * Quaternion.Euler (0, 0, 90.0f);
				matrix = RotateMatrix (matrix);
			}
		}
	}


	void Update () {

		if (!isCurrentTetromino || GameManager.gm.pause || GameManager.gm.gameOver)
			return;


		tmpTime += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			MovePiece (new Vector3(-1, 0, 0), false);
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			MovePiece (new Vector3(1, 0, 0), false);
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			RotatePiece ();
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			tmpTime = delay + 1;
		} 
		if (tmpTime > delay) {
			MovePiece (new Vector3(0, -1, 0), true);
		}
	}

	public MovementState CanMove(int[,] grid, int[,] matrice, int matriceX0, int matriceY0) {
		bool meetPlayer = false;
		for (int matriceX = 0; matriceX < matrix.GetLength(0); matriceX++) {
			for (int matriceY = 0; matriceY < matrix.GetLength(0); matriceY++) {
				int gridPosX = matriceX0 + matriceX;
				int gridPosY = matriceY0 - matriceY;

				if (matrice[matriceY, matriceX] == 1 && (gridPosX < 0 || gridPosX > grid.GetLength(0) - 1)) {
					return MovementState.WALL;
				}
				if (matrice[matriceY, matriceX] == 1 && (gridPosY < 0 || grid[gridPosX, gridPosY] == 1)) {
					return MovementState.OBSTACLE;
				}

				if (matrice [matriceY, matriceX] == 1 && grid [gridPosX, gridPosY] == 2)
					meetPlayer = true;
/*				if (matrice [matriceY, matriceX] == 1 && gridPosX == GameManager.gm.Player.transform.position.x && gridPosY == GameManager.gm.Player.transform.position.y) {
					meetPlayer = true;
				}*/
			}
		}
		if (meetPlayer)
			return MovementState.PLAYER;
		return MovementState.CAN_MOVE;
	}
}
