using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	private Animator _anim;

	//Where the player should be
	private GameManager.Lane targetLane;

	private void Awake() {
		_anim = GetComponent<Animator>();
		targetLane = GameManager.Lane.MIDDLE;
	}

	private void OnEnable() {
		Events.Instance.AddListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.AddListener<OnPauseEvent>(HandleOnPause);
		Events.Instance.AddListener<OnGameStartEvent>(HandleGameStart);
	}

	private void OnDisable() {
		Events.Instance.RemoveListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.RemoveListener<OnPauseEvent>(HandleOnPause);
		Events.Instance.RemoveListener<OnGameStartEvent>(HandleGameStart);
	}

	void HandleOnPlayerLoose(OnPlayerLooseEvent e) {
		_anim.SetTrigger("Die");
	}

	void HandleOnPause(OnPauseEvent e) {
		_anim.SetTrigger("Pause");
	}

	void HandleGameStart(OnGameStartEvent e) {
		_anim.SetTrigger("Pause");
	}

	private void Update() {
		if (GameManager.Instance.isPlaying == false || GameManager.Instance.pause == true)
			return;

		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			MoveLeft();
		} else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			MoveRight();
		}

		Vector3 targetPos = new Vector3(GameManager.Instance.LanesPositions.Find(e => e.lane == targetLane).position, transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, targetPos, .1f);
	}

	private void MoveLeft() {
		switch (targetLane) {
			case GameManager.Lane.LEFT: //Player is already on the left lane
				break;
			case GameManager.Lane.MIDDLE:
				targetLane = GameManager.Lane.LEFT;
				break;
			case GameManager.Lane.RIGHT:
				targetLane = GameManager.Lane.MIDDLE;
				break;
		}
	}

	private void MoveRight() {
		switch (targetLane) {
			case GameManager.Lane.LEFT:
				targetLane = GameManager.Lane.MIDDLE;
				break;
			case GameManager.Lane.MIDDLE:
				targetLane = GameManager.Lane.RIGHT;
				break;
			case GameManager.Lane.RIGHT: //Player is already on the right lane
				break;
		}
	}


	//Needed by the animator
	public void RestartLevel() {}
}
