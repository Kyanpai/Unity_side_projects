using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyHasBeenKilledEvent : GameEvent {
	public int pointsGiven;
}

public class EnemyBehaviour : MonoBehaviour {

	public int health;
	public int pointsWhenKilled;

	private Animator _anim;

	private void Awake() {
		_anim = GetComponent<Animator>();
	}

	private void OnEnable() {
		Events.Instance.AddListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.AddListener<OnPauseEvent>(HandleOnPause);
	}

	private void OnDisable() {
		Events.Instance.RemoveListener<OnPlayerLooseEvent>(HandleOnPlayerLoose);
		Events.Instance.RemoveListener<OnPauseEvent>(HandleOnPause);
	}

	private void HandleOnPlayerLoose(OnPlayerLooseEvent e) {
		_anim.SetTrigger("PlayerDead");
	}

	private void HandleOnPause(OnPauseEvent e) {
		_anim.SetTrigger("Pause");
	}

	void Update () {
		if (GameManager.Instance.isPlaying && GameManager.Instance.pause == false)
			transform.Translate(Vector3.forward * EnemiesManager.Instance.globalSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.transform.tag == "EndObstacle") {
			Destroy(gameObject);
		} else if (other.transform.tag == "Player") {
			Events.Instance.Raise(new OnPlayerLooseEvent { });
		}
	}

	public void Hit() {
		if (GameManager.Instance.isPlaying == false || GameManager.Instance.pause == true)
			return;

		health--;
		if (health <= 0) {
			Events.Instance.Raise(new OnEnemyHasBeenKilledEvent { pointsGiven = pointsWhenKilled });
			Destroy(gameObject);
		}
	}
}
