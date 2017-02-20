using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GridID = System.Int32;

public class AIMovement : MonoBehaviour {

    public GridSystem gridSystem; //Grid System

	public List<GridID> path; //Path
	private int pathIndex; //Where are we along the path.    
	private float reachedDistance; //How close must we be to the grid to have considered reaching it.

	private GameObject currentGrid; //The grid we are going towards.
	private GameObject nextGrid; //The next grid to head to.

	//[HideInInspector]
	public float maxMovementSpeed; //How fast do we move?
	//[HideInInspector]
	public float rotationSpeed; //How fast do we rotate? (Not in angles. Kind like ratio instead. Kinda.)
	//[HideInInspector]
	public float movementForce; //How much force do we use to propel ourselves forward? More Force = Less Slidey.

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

		//Start off facing where we need to go.
		Vector3 direction = currentGrid.GetComponent<Transform>().position - gameObject.transform.position;
		float yRotation = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
		gameObject.transform.eulerAngles = new Vector3 (0, yRotation, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (path == null) {
			print("Path is null!");
			SlowDown(0.1f);
			//GameObject.Destroy(gameObject);
			return;
		} else if (path.Count == 0) {
			print("Path Empty!");
			SlowDown(0.1f);
			//GameObject.Destroy(gameObject);
			return;
		} else if (pathIndex >= path.Count) {
			//print("Reached End!");
			SlowDown(0.0f);
			//GameObject.Destroy(gameObject);
			return;
		}

		if (IsDone()) {
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

		moveDirection = moveDirection.normalized;

		//Rotate and move using physics.
		if (Vector3.Angle(moveDirection, gameObject.transform.forward) > 40.0f) {
			//print("Slowing Down");
			//SlowDown(0.4f);
		}
		float turnAmount = Vector3.Dot(moveDirection, gameObject.transform.right);
		if (turnAmount > 0.7f) {
			//print("Turning Right");
		} else if (turnAmount < -0.7f) {
			//print("Turning Left");
		}

		RotateTowards(moveDirection, rotationSpeed);
		gameObject.GetComponent<Rigidbody>().AddForce(moveDirection * Time.deltaTime * movementForce);
		LimitHorizontalVelocity();

		//Rotate and move without physics.
		//RotateTowards(moveDirection, rotationSpeed);
		//gameObject.transform.Translate(moveDirection * Time.deltaTime * maxMovementSpeed, Space.World);

		//If we've reached the grid, move on to the next one.
		float distanceToGridSquared = (destination - gameObject.transform.position).sqrMagnitude;
		if (distanceToGridSquared < reachedDistance * reachedDistance) {
			++pathIndex;
		}

		//Debugging Information
		//print("Framerate: " + 1.0f / Time.deltaTime);
		//print("Move Direction: " + moveDirection);
		//print("Direction To Current Grid: " + directionToCurrentGrid);
		//print("Destination: " + destination);
	}

	public float GetMaxMovementSpeed() {
		return maxMovementSpeed;
	}

	protected virtual void RotateTowards(Vector3 _direction, float _rotationSpeed) {
		if (_direction == Vector3.zero) { //We can't rotate if direction is a zero vector.
			return;
		}

		Quaternion rotationToDirection = Quaternion.LookRotation(_direction); //This is the rotation which will make us face _direction.
		Vector3 rotationEuler = Quaternion.Slerp(gameObject.transform.rotation, rotationToDirection, _rotationSpeed * Time.deltaTime).eulerAngles; //Rotate base on our speed.
		rotationEuler.z = 0; //We only want to rotation on the y-axis.
		rotationEuler.x = 0; //We only want to rotation on the y-axis.

		gameObject.transform.rotation = Quaternion.Euler(rotationEuler);
	}

	protected virtual void LimitHorizontalVelocity() {
		Vector3 horizontalVelocity = gameObject.GetComponent<Rigidbody>().velocity;
		horizontalVelocity.y = 0.0f;
		if (horizontalVelocity.sqrMagnitude > maxMovementSpeed * maxMovementSpeed) {
			horizontalVelocity = (horizontalVelocity.normalized * maxMovementSpeed);
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(horizontalVelocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, horizontalVelocity.z);
		}
	}

	protected virtual void SlowDown(float _multiplier) {
		_multiplier = Mathf.Clamp(_multiplier, 0.0f, 1.0f);
		gameObject.GetComponent<Rigidbody>().velocity *= ((1.0f - Time.deltaTime) * _multiplier);
	}

	public bool IsDone() {
		return (path == null) || (pathIndex >= path.Count);
	}

}