using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class MonsterSpawner : MonoBehaviour {

	//These will be removed in the final product.
	public bool temporaryTestStartStageButton;
	private int tempStageIndex;

	//These 2 have priority.
	[SerializeField]
	private Grid startGrid;
	[SerializeField]
	private Grid endGrid;

	//These 2 will be used if startGrid or endGrid is null. If these 2 are used, gridSystem must not be null.
	[SerializeField]
	private GridID startID, endID; //There should not be an underscore. That was a mistake.

	//Only needed if startGrid or endGrid is null.
	[SerializeField]
	private GridSystem gridSystem;

	[SerializeField]
	private List<GridID> path;

	[SerializeField]
	private GameObject[] monsterPrefabs = new GameObject[(uint)MONSTER_TYPE.NUM_MONSTER_TYPE]; //Do not change the size!
	private List<GameObject>[] monsterPool;
	private int[] monsterPoolIndex; //Our last searched index;

	// Use this for like, initialization even before initialization. Initializationception!
	void Awake() {		
		GeneratePath();
		monsterPool = new List<GameObject>[(uint)MONSTER_TYPE.NUM_MONSTER_TYPE];
		for (int i = 0; i < monsterPool.Length; ++i) {
			monsterPool[i] = new List<GameObject>();
		}
		monsterPoolIndex = new int[(uint)MONSTER_TYPE.NUM_MONSTER_TYPE];
		for (int i = 0; i < monsterPoolIndex.Length; ++i) {
			monsterPoolIndex[i] = 0;
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (temporaryTestStartStageButton) {
			StartStage(tempStageIndex);
			temporaryTestStartStageButton = false;
			++tempStageIndex;
		}

		//Don't change this. If you do I'll just keep changing it back.
		//We migh also crash. Probably crash.
		if (monsterPrefabs.Length != (uint)MONSTER_TYPE.NUM_MONSTER_TYPE) {
			monsterPrefabs = new GameObject[(uint)MONSTER_TYPE.NUM_MONSTER_TYPE];
		}
	}

	public void GeneratePath() {
		if (startGrid == null || endGrid == null) {
			if (gridSystem.GetGrid(startID) != null) {
				startGrid = gridSystem.GetGrid(startID).GetComponent<Grid>();
			}
			if (gridSystem.GetGrid(endID) != null) {
				endGrid = gridSystem.GetGrid(endID).GetComponent<Grid>();
			}
		} else {
			//Alrighty, we have start and end grids. Let's find our path!
			gridSystem = null; //Set this to null first.
			//Get the Grid System.
			GridSystem startGridSystem = startGrid.GetGridSystem();
			GridSystem endGridSystem = endGrid.GetGridSystem();
			if (startGridSystem == null || endGridSystem == null) {
				print(gameObject.name + "'s start grid or end grid has no grid system.");
			} else if (startGridSystem != endGridSystem) {
				print(gameObject.name + "'s start grid and end grid have different grid system. Seriously? What is wrong with you?");
			} else {
				gridSystem = startGridSystem; //Gotcha!
				startID = startGrid.GetID(); //Assign our startID.
				endID = endGrid.GetID(); //Assign our endID.
			}
		}

		if (gridSystem == null) {
			print(gameObject.name + " has no Grid System."); //失败！/(T.T)\
		} else {
			path = gridSystem.Search(startID, endID); //成功！\(^.^)/
		}
	}

	public bool SpawnMonster(MONSTER_TYPE _monsterType) {
		if (_monsterType == MONSTER_TYPE.NUM_MONSTER_TYPE) {
			print(gameObject.name + " cannot spawn monster as monster type is invalid.");
			return false;
		} else if (monsterPrefabs[(uint)_monsterType] == null) {
			print(gameObject.name + " cannot spawn monster as monster type prefab is null.");
			return false;
		} else if (monsterPrefabs[(uint)_monsterType].GetComponent<AIFollowPath>() == null) {
			print(gameObject.name + " cannot spawn a monster with AIMovement Component!");
			return false;
		}

		if (path != null && path.Count > 0) { //Place the monster at the start of the path.			
			GameObject monster = GetMonster(_monsterType);
			monster.SetActive(true);
			monster.GetComponent<AIFollowPath>().gridSystem = gridSystem;
			monster.GetComponent<AIFollowPath>().path = path;
			monster.GetComponent<AIAttackCrystal>().tritanCrystal = endGrid.tritanCrystal;
			monster.GetComponent<Transform>().position = gridSystem.GetGrid(path[0]).GetComponent<Transform>().position + new Vector3(0, 0.2f, 0);
			monster.GetComponent<AIBehaviour>().Reset();
			return true;
		} else {
			print("Cannot spawn monster without a path!");
			return false;
		}
	}

	private GameObject GetMonster(MONSTER_TYPE _monsterType) {
		GameObject monster = null; //The monster.
		if (monsterPool[(uint)_monsterType].Count > 0) {			
			//Which index to start searching from.
			int currentIndex = (monsterPoolIndex[(uint)_monsterType] + 1) % monsterPool[(uint)_monsterType].Count;

			//Stop looping once we've checked everything.
			for (int i = 0; i < monsterPool[(uint)_monsterType].Count; ++i) {
				int index = (currentIndex + i) % monsterPool[(uint)_monsterType].Count;
				monster = monsterPool[(uint)_monsterType][index];
				if (monster == null || monster.activeSelf == true) {
					continue; //This one is not suitable.
				}
				monsterPoolIndex[(uint)_monsterType] = index;
				return monster;
			}
		}

		monster = GameObject.Instantiate(monsterPrefabs[(uint)_monsterType]);
		monsterPool[(uint)_monsterType].Add(monster);
		monsterPoolIndex[(uint)_monsterType] = 0;

		return monster;
	}

	public void ClearMonsters() {
		for (int monsterType = 0; monsterType < (int)MONSTER_TYPE.NUM_MONSTER_TYPE; ++monsterType) {
			foreach (GameObject monster in monsterPool[monsterType]) {
				if (monster != null) {
					GameObject.Destroy(monster);
				}
			}
			monsterPool[monsterType].Clear();
		}
	}

	public int GetStartID() {
		return startID;
	}

	public int GetEndID() {
		return endID;
	}

	public Grid GetStartGrid() {
		return startGrid;
	}

	public Grid GetEndGrid() {
		return endGrid;
	}

	//These 2 functions are just a temporary fix for one of my typos. Gotta fix it eventually.
	public int _startID {
		get {
			return this.startID;
		}
	}

	public int _endID {
		get {
			return this.endID;
		}
	}

	//Start the action.
	public bool StartStage(int _stageIndex) {
		if (_stageIndex < 0) {
			return false;
		}

		int index = 0;
		foreach (Transform child in transform) {
			//Check if it is a stage.
			MonsterSpawnerStage stage = child.gameObject.GetComponent<MonsterSpawnerStage>();
			if (stage == null) {
				continue;
			}

			//Yes, this is the right one. We can stop now.
			if (_stageIndex == index++) {
				stage.StartStage();
				return true;
			}
		}

		return false;
	}

	//For whatever reason we need to stop it.
	public bool StopStage(int _stageIndex) {
		if (_stageIndex < 0) {
			return false;
		}

		int index = 0;
		foreach (Transform child in transform) {
			//Check if it is a stage.
			MonsterSpawnerStage stage = child.gameObject.GetComponent<MonsterSpawnerStage>();
			if (stage == null) {
				continue;
			}

			//Yes, this is the right one. We can stop now.
			if (_stageIndex == index++) {
				stage.StopStage(); //停！
				return true;
			}
		}

		return false;
	}

}