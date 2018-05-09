using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	[SerializeField]
	Transform WeaponBarrelEnd;
	[SerializeField]
	float timeOut = -1;
	[SerializeField]
	float spawnTime = 0.1f;
	[SerializeField]
	GameObject BulletPrefab;

	[SerializeField]
	ParticleSystem ps;

	private void Update() {
		if (GameManager.Instance.isPlaying == false || GameManager.Instance.pause == true)
			return;

		if (Time.time > timeOut) {
			Spawn();
			timeOut = Time.time + spawnTime;
		}
	}

	private void Spawn() {
		Instantiate(BulletPrefab, WeaponBarrelEnd.position, WeaponBarrelEnd.rotation);
		ps.Stop();
		ps.Play();
	}
}
