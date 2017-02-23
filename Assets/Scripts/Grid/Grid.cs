using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class Grid : MonoBehaviour {

	[SerializeField]
	private GridID id; // ID of each tile, ID = currentRow * numberOfColumns + currentColumn.
	[SerializeField]
	private GridID[] neighbourIDs = new GridID[4]; // Neighbouring tiles of a tile, Previous Row, Next Row, Previous Column, Next Column.

	public GameObject tower; // The tower (if any) on the grid. Public variable so that it can be changed easily
	public GameObject wall; // The wall (if any) on the grid. Public variable so that it can be changed easily
	public GameObject tritanCrystal;

	[SerializeField]
	private GridSystem gridSystem;

	public void Awake() {		
	}

	// Use this for initialization
	void Start () {		
	}

	// Update is called once per frame
	void Update () {
	}

	public void FindParentGridSystem() {
		Transform parentTransform = gameObject.transform.parent; //Get our parent's transform.
		while (parentTransform != null) {
			gridSystem = parentTransform.gameObject.GetComponent<GridSystem>(); //Get the spawner if there is, null if there isn't.
			if (gridSystem != null) {
				break;
			}
			parentTransform = parentTransform.parent;
		}

		if (gridSystem == null) {
			print(gameObject.name + " has no Grid System.");
		}
	}

	//Setters and getters for Tile ID
	public GridID GetID() {
		return id;
	}

	public void SetID(GridID _id) {
		this.id = _id;
	}

	// Setters and getters for neighbours
	public List<GridID> GetNeighbourIDs() {
		return new List<GridID>(neighbourIDs); // Returns a list of the array of neighbours' tileIDs
	}

	public void SetNeighbourIDs(GridID _previousRow, GridID _nextRow, GridID _previousColumn, GridID _nextColumn) {
		if (neighbourIDs.Length != 4) {
			neighbourIDs = new GridID[4];
		}
		neighbourIDs [0] = _previousRow;
		neighbourIDs [1] = _nextRow;
		neighbourIDs [2] = _previousColumn;
		neighbourIDs [3] = _nextColumn;
	}

	public GridSystem GetGridSystem() {
		return gridSystem;
	}
    
}