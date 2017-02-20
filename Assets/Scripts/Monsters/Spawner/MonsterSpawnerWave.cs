using UnityEngine;
using System.Collections;

public class MonsterSpawnerWave : MonoBehaviour {

	private MonsterSpawner monsterSpawner; //The spawner that is going to spawn our monster and handle the other stuff for us.

	public MonsterSpawnerWave triggerWave; //This wave starts after triggerWave is done.
	public float triggerTime; //Either number of seconds after triggeWave is done or after game starts.
	private float triggerCountdownTimer;

	public GameObject monsterPrefab; //What is the monster we are spawning?
	public uint numMonsters; //How many monster to spawn?
	private uint count; //How many monsters have we spawned so far?

	public float spawnInterval; //The number of seconds between the spawning of each monster.
	private float spawnCountdownTimer; //The numer of seconds left before the next monster spawns.

	void Start() {
		count = 0;
		spawnCountdownTimer = 0.0f;
		triggerCountdownTimer = triggerTime;

		if (gameObject.transform.parent != null) {
			GameObject parentGameObject = gameObject.transform.parent.gameObject;
			while (parentGameObject != null) {
				monsterSpawner = parentGameObject.GetComponent<MonsterSpawner>(); //Get the spawner if there is, null if there isn't.
				if (monsterSpawner != null) {
					break;
				}
				parentGameObject = gameObject.transform.parent.gameObject;
			}
		}

		if (monsterSpawner == null) {
			print("MonsterSpawnerWave has no Monster Spawner.");
		}
	}

	void Update() {
		if (IsDone()) {
			return;
			//GameObject.Destroy(gameObject);
		}

		//We will only start the spawning when triggerTime is <= 0.0f. If triggerWave isn't null, then it must be done as well.
		if (triggerWave == null || triggerWave.IsDone()) {
			if (triggerCountdownTimer > 0.0f) { //Countdown to the action begins.
				triggerCountdownTimer -= Time.deltaTime;
			} else { //スタ-ト！
				if (spawnCountdownTimer > 0.0f) {
					spawnCountdownTimer -= Time.deltaTime;
				} else {
					SpawnMonster();
					++count;
					spawnCountdownTimer = spawnInterval;
				}
			}
		}
	}

	public bool IsDone() { //任务完成！ \(^.^)/
		return (count >= numMonsters);
	}

	private void SpawnMonster() { //What have we done? We've made a monster!
		if (monsterSpawner == null) {
			print("Monster Spawner is null! Cannot spawn monster");
		} else {			
			if (monsterSpawner.SpawnMonster(monsterPrefab)) {
				//print("Spawned Monster");
			} else {
				print("Failed to spawn monster.");
			}
		}
	}

}