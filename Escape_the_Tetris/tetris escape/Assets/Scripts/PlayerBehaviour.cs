using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	private TetrominoBehaviour tetroBehaviour;
	private float delay = 1f;
	private float tmpTime = 0f;
	private bool CanJump = true;
	private bool win = false;

	private Animator animator;

	// Use this for initialization
	void Start () {
		StartCoroutine (LateStart());
	}

	IEnumerator LateStart() {
		yield return new WaitForSeconds(.5f);
		transform.position = new Vector3 (GameManager.gm.GridWidth / 2, 0, 0);
		tetroBehaviour = GetComponent<TetrominoBehaviour> ();
		tetroBehaviour.matrix = new int[,] {
			{ 0, 0, 0 },
			{ 0, 1, 0 },
			{ 0, 0, 0 }
		};
		MovePlayer(new Vector3 (0, 0, 0), false);
		animator = GetComponentInChildren<Animator> ();
	}
		
	private void MovePlayer(Vector3 movement, bool left) {
		if (CheckVictory (movement, left)) {
			return;
		}
		if (CanMove (movement) == TetrominoBehaviour.MovementState.CAN_MOVE) {
			GameManager.gm.Grid [(int)transform.position.x, (int)transform.position.y] = 0;
			transform.position += movement;
			GameManager.gm.Grid [(int)transform.position.x, (int)transform.position.y] = 2;
		}
	}

	private TetrominoBehaviour.MovementState CanMove(Vector3 movement) {
		Vector3 potentialPosition = transform.position + movement;
		if ((int)potentialPosition.y > GameManager.gm.Grid.GetLength (1) - 1 ||
		    (int)potentialPosition.x < 0 ||
		    (int)potentialPosition.x > GameManager.gm.Grid.GetLength (0) - 1)
			return TetrominoBehaviour.MovementState.WALL;

		if ((int)potentialPosition.y < 0 || GameManager.gm.Grid [(int)potentialPosition.x, (int)potentialPosition.y] != 0)
			return TetrominoBehaviour.MovementState.OBSTACLE;
		return TetrominoBehaviour.MovementState.CAN_MOVE;
	}

	private bool CheckVictory(Vector3 movement, bool left) {
		Vector3 tmpPos = transform.position + movement;
		if ((left && tmpPos.x == -1 && (tmpPos.y == 20 || tmpPos.y == 21)) || (!left && tmpPos.x == 10 && (tmpPos.y == 20 || tmpPos.y == 21))) {
			if (tmpPos.y == 21)
				transform.position += new Vector3 (0, -1, 0);
			GameManager.gm.Win ();
			win = true;
			transform.position += movement;
			end (left);
			return true;
		}
		return false;
	}

	void end(bool left) {
		animator.enabled = true;
		if (left)
			animator.SetTrigger ("EndLeft");
		else
			animator.SetTrigger ("EndRight");
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.gm.pause || GameManager.gm.gameOver
			|| tetroBehaviour == null || win)
			return;

		tmpTime += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.A)) {
			MovePlayer (new Vector3 (-1, 0, 0), true);
		} else if (Input.GetKeyDown(KeyCode.D)) {
			MovePlayer (new Vector3 (1, 0, 0), false);
		} else if (Input.GetKeyDown(KeyCode.W) && CanJump) {
			CanJump = false;
			tmpTime = 0;
			MovePlayer(new Vector3 (0, 1, 0), false);
		}
		if (tmpTime > delay) {
			TetrominoBehaviour.MovementState state = CanMove (new Vector3 (0, -1, 0));
			if (state == TetrominoBehaviour.MovementState.CAN_MOVE) {
				GameManager.gm.Grid [(int)transform.position.x, (int)transform.position.y] = 0;
				transform.position += new Vector3 (0, -1, 0);
				GameManager.gm.Grid [(int)transform.position.x, (int)transform.position.y] = 2;
				tmpTime = 0;
				CheckPlayerLineComplete ();
			} else if (state == TetrominoBehaviour.MovementState.OBSTACLE) {
				CanJump = true;
			}
		}
	}

	public void CheckPlayerLineComplete() {
		bool lineFull = true;
		for (int x = 0; x < GameManager.gm.GridWidth; x++) {
			if (GameManager.gm.Grid [x, (int)transform.position.y] == 0) {
				lineFull = false;
					break;
			}
		}
		if (lineFull) {
			GameManager.gm.GameOver ();
			GameManager.gm.DeleteLineFromGrid ((int)transform.position.y);
		}
	}
}
