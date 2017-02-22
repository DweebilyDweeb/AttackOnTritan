using UnityEngine;
using System.Collections;

public class MonsterSpawnerWave : MonoBehaviour {

	private MonsterSpawner monsterSpawner; //The spawner that is going to spawn our monster and handle the other stuff for us.

	public MonsterSpawnerWave triggerWave; //This wave starts after triggerWave is done.
	public float triggerTime; //Either number of seconds after triggeWave is done or after game starts.
	private float triggerCountdownTimer;

	public MONSTER_TYPE monsterType; //What is the monster we are spawning?
	public uint numMonsters; //How many monster to spawn?
	private uint count; //How many monsters have we spawned so far?

	public float spawnInterval; //The number of seconds between the spawning of each monster.
	private float spawnCountdownTimer; //The numer of seconds left before the next monster spawns.

	private bool stop;

	void Awake() {
		//Get our parent spawner.
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

		//Initialise our default values.
		stop = true;
		Reset();
	}

	void Start() {
	}

	void Update() {
		if (stop || IsDone()) {
			return;
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

	private void SpawnMonster() { //What have we done? We've made a monster!
		if (monsterSpawner.SpawnMonster(monsterType)) {
			//print("Spawned Monster");
		} else {
			print("Failed to spawn monster.");
		}
	}

	public bool IsDone() { //任务完成！ \(^.^)/
		return (count >= numMonsters);
	}

	public void Reset() {
		count = 0;
		spawnCountdownTimer = 0.0f;
		triggerTime = Mathf.Max(0.0f, triggerTime);
		triggerCountdownTimer = triggerTime;
	}

	//Whenever we start, it is reseted.
	public void StartWave() {
		Reset();
		stop = false;
	}

	public void StopWave() {
		stop = true;
	}

	public bool IsStopped() {
		return stop;
	}

}