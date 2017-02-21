using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class AIFollowPath : MonoBehaviour {

    public GridSystem gridSystem; //Grid System

	public List<GridID> path; //Path
	[SerializeField]
	private int pathIndex; //Where are we along the path.    
	private float reachedDistance; //How close must we be to the grid to have considered reaching it.

	private GameObject currentGrid; //The grid we are going towards.
	private GameObject nextGrid; //The next grid to head to.

	AIMovement aiMovement;

	// This is called before Start().
	void Awake() {
		pathIndex = 0;
		reachedDistance = 0.5f;
	}

	// Use this for initialization
	void Start () {
		if (gridSystem == null) { //Make sure that we have a gridSystem assigned.
			print ("Error! Grid System is null!");
			return;
		}

		currentGrid = gridSystem.GetGrid (path[pathIndex]); //Where we wanna go?
		if (currentGrid == null) {
			print ("Error! Destination Grid is null!");
			return;
		}

		aiMovement = gameObject.GetComponent<AIMovement>();

		//Start off facing where we need to go.
		Vector3 direction = currentGrid.GetComponent<Transform>().position - gameObject.transform.position;
		aiMovement.RotateTowards(direction, true);
	}
	
	// Update is called once per frame
	void Update () {
		if (path == null) {
			print("Path is null!");
			aiMovement.SlowDown(0.1f);
			return;
		} else if (path.Count == 0) {
			print("Path Empty!");
			aiMovement.SlowDown(0.1f);
			return;
		} else if (IsDone()) {
			//print("Reached End!");
			aiMovement.SlowDown(0.0f);
			return;
		}

		//Get the grid that we wanna head towards.
		currentGrid = gridSystem.GetGrid (path[pathIndex]);
		if (pathIndex + 1 < path.Count) {
			nextGrid = gridSystem.GetGrid(path[pathIndex + 1]); //At the same time, find out what the next grid is.
		} else {
			nextGrid = null;
		}

		//This is the direction we need to head in order to go towards our current grid.
		Vector3 directionToCurrentGrid = currentGrid.transform.position - gameObject.transform.position;
		directionToCurrentGrid.y = 0.0f; //Disregard the height difference. The grid is the floor while we are above the floor.

		Vector3 destination = currentGrid.transform.position; //This is where we are going.
		Vector3 moveDirection = directionToCurrentGrid; //The direction we need to move.

		if (nextGrid != null) { //If this isn't the last grid,
			Vector3 pathDirection = nextGrid.transform.position - currentGrid.transform.position;
			pathDirection.y = 0.0f;

			Vector3 directionToNextGrid = nextGrid.transform.position - gameObject.transform.position;
			directionToNextGrid.y = 0.0f;

			//Make sure that going to the current grid isn't going against the traffic flow (In case we overshot.).
			//If we have to go against traffic flow to get to our grid, then just skip it and move on to the next one if the next one is not against traffic.
			if (Vector3.Dot(directionToCurrentGrid, pathDirection) < 0.0f && Vector3.Dot(directionToNextGrid, pathDirection) > 0.0f) {
				++pathIndex;
				moveDirection = directionToNextGrid;
				destination = nextGrid.transform.position;
			}
		}

		aiMovement.Move(moveDirection, true);

		//If we've reached the grid, move on to the next one.
		float distanceToGridSquared = (destination - gameObject.transform.position).sqrMagnitude;
		if (distanceToGridSquared < reachedDistance * reachedDistance) {
			++pathIndex;
		}
	}

	public void OnCollisionStay(Collision _collisionInfo) {
		//Check if we're on the last grid and colliding with the crystal.
		if (pathIndex == path.Count - 1 && _collisionInfo.gameObject.CompareTag("Tritan Crystal")) {
			++pathIndex;
		}
	}

	public bool IsDone() {
		return (path == null) || (pathIndex >= path.Count);
	}

	public void Reset() {
		pathIndex = 0;
	}

}