using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawnerStage : MonoBehaviour {

	private MonsterSpawner monsterSpawner; //The spawner that is going to spawn our monster and handle the other stuff for us.

	void Awake() {
		//Find our parent that is a Monster Spawner.
		Transform parentTransform = gameObject.transform.parent; //Get our parent's transform.
		while (parentTransform != null) {
			monsterSpawner = parentTransform.gameObject.GetComponent<MonsterSpawner>(); //Get the spawner if there is, null if there isn't.
			if (monsterSpawner != null) {
				break;
			}
			parentTransform = parentTransform.parent;
		}

		if (monsterSpawner == null) {
			print(gameObject.name + " has no Monster Spawner.");
		}
	}

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update () {	
	}

	//Any children with the wave thing will be set as active.
	public void StartStage() {
		EnableWaves(gameObject.transform);
	}

	private void EnableWaves(Transform _transform) {
		//Enable the MonsterSpawnerWave for this object.
		MonsterSpawnerWave wave = _transform.gameObject.GetComponent<MonsterSpawnerWave>();
		if (wave != null) {
			wave.StartWave(); //Start the wave.
		}

		//Recursively do this.
		foreach (Transform child in _transform) {
			EnableWaves(child);
		}
	}

	public void StopStage() {
		DisableWaves(gameObject.transform);
	}

	private void DisableWaves(Transform _transform) {
		//Enable the MonsterSpawnerWave for this object.
		MonsterSpawnerWave wave = _transform.gameObject.GetComponent<MonsterSpawnerWave>();
		if (wave != null) {
			wave.StopWave();
		}

		//Recursively do this.
		foreach (Transform child in _transform) {
			EnableWaves(child);
		}
	}

}