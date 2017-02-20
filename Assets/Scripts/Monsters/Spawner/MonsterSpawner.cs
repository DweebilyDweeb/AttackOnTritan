using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class MonsterSpawner : MonoBehaviour {
	
	public GridSystem gridSystem;
	public GridID _startID, _endID;
	[SerializeField]
	private List<GridID> path;

	// Use this for initialization
	void Start () {
		GeneratePath();
	}

	// Update is called once per frame
	void Update () {		
	}

	public void GeneratePath() {
		if (gridSystem != null) {
			path = gridSystem.Search (_startID, _endID);
		}
	}

	public bool SpawnMonster(GameObject _monsterPrefab) {
		if (_monsterPrefab.GetComponent<AIMovement>() == null) {
			print("Cannot spawn a monster with AIMovement Component!");
			return false;
		}

		if (path != null && path.Count > 0) { //Place the monster at the start of the path.
			GameObject monster = GameObject.Instantiate (_monsterPrefab);
			monster.GetComponent<AIMovement> ().gridSystem = gridSystem;
			monster.GetComponent<AIMovement> ().path = path;
			monster.GetComponent<Transform>().position = gridSystem.GetGrid(path[0]).GetComponent<Transform>().position + new Vector3(0, 0.2f, 0);
			return true;
		} else {
			print("Cannot spawn monster without a path!");
			return false;
		}
	}
}