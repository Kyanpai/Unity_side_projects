using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour {

	[SerializeField]
	List<GameObject> ObstaclesList = new List<GameObject>();

	float spawnTime = 0.6f;
	float timeOut = -1;

	Dictionary<GameManager.Difficulty, float> spawnTimeByDifficulty = new Dictionary<GameManager.Difficulty, float> {
		{ GameManager.Difficulty.VERYEASY, .8f },
		{ GameManager.Difficulty.EASY, .7f },
		{ GameManager.Difficulty.MEDIUM, .6f },
		{ GameManager.Difficulty.HARD, .5f },
		{ GameManager.Difficulty.VERYHARD, .4f },
		{ GameManager.Difficulty.IMPOSSIBLE, .3f }

	};

	private void Awake() {
		spawnTime = spawnTimeByDifficulty[GameManager.Difficulty.EASY];
	}

	private void OnEnable() {
		Events.Instance.AddListener<OnIncreaseDifficultyEvent>(IncreaseDifficulty);
	}

	private void OnDisable() {
		Events.Instance.RemoveListener<OnIncreaseDifficultyEvent>(IncreaseDifficulty);
	}

	void IncreaseDifficulty(OnIncreaseDifficultyEvent e) {
		spawnTime = spawnTimeByDifficulty[e.newDifficulty];
	}

	void Update() {
		if (GameManager.Instance.isPlaying == false || GameManager.Instance.pause == true)
			return;

		if (Time.time > timeOut) {
			Spawn();
			timeOut = Time.time + spawnTime;
		}

	}

	void Spawn() {
		float spawnPointX = GameManager.Instance.LanesPositions[Random.Range(0, GameManager.Instance.LanesPositions.Count)].position;
		Vector3 spawnPosition = new Vector3(spawnPointX, transform.position.y, transform.position.z);
		Instantiate(ObstaclesList[Random.Range(0, ObstaclesList.Count)], spawnPosition, transform.rotation);
	}
}
