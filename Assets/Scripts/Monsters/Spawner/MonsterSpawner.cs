using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class MonsterSpawner : MonoBehaviour {

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

	// Use this for like, initialization even before initialization. Initializationception!
	void Awake() {
		GeneratePath();
	}

	// Use this for initialization
	void Start () {		
	}

	// Update is called once per frame
	void Update () {		
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

	public bool SpawnMonster(GameObject _monsterPrefab) {
		if (_monsterPrefab.GetComponent<AIFollowPath>() == null) {
			print("Cannot spawn a monster with AIMovement Component!");
			return false;
		}

		if (path != null && path.Count > 0) { //Place the monster at the start of the path.
			GameObject monster = GameObject.Instantiate (_monsterPrefab);
			monster.GetComponent<AIFollowPath> ().gridSystem = gridSystem;
			monster.GetComponent<AIFollowPath> ().path = path;
			monster.GetComponent<AIAttackCrystal>().tritanCrystal = endGrid.tritanCrystal;
			monster.GetComponent<Transform>().position = gridSystem.GetGrid(path[0]).GetComponent<Transform>().position + new Vector3(0, 0.2f, 0);
			return true;
		} else {
			print("Cannot spawn monster without a path!");
			return false;
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

}