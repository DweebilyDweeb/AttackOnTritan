using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

	[SerializeField]
	private float maxMovementSpeed; //How fast do we move?
	[SerializeField]
	private float rotationSpeed; //How fast do we rotate? (Not in angles. Kind like ratio instead. Kinda.)
	[SerializeField]
	private float movementForce; //How much force do we use to propel ourselves forward? More Force = Less Slidey.

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Do nothing.
	}

	//Move towards this direction.
	public void Move(Vector3 _direction, bool rotateTowards = true) {
		if (_direction == Vector3.zero) {
			return;
		}

		if (rotateTowards == true) {
			RotateTowards(_direction);
		}
		gameObject.GetComponent<Rigidbody>().AddForce(_direction.normalized * Time.deltaTime * movementForce);
		LimitHorizontalVelocity();
	}

	//Limit our horizontal velocity so we don't fly off.
	private void LimitHorizontalVelocity() {
		Vector3 horizontalVelocity = gameObject.GetComponent<Rigidbody>().velocity;
		horizontalVelocity.y = 0.0f;
		if (horizontalVelocity.sqrMagnitude > maxMovementSpeed * maxMovementSpeed) {
			horizontalVelocity = (horizontalVelocity.normalized * maxMovementSpeed);
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(horizontalVelocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, horizontalVelocity.z);
		}
	}

	//Rotate on the Y-Axis towards a direction.
	public void RotateTowards(Vector3 _direction, bool _snap = false) {
		if (_direction == Vector3.zero) { //We can't rotate if direction is a zero vector.
			return;
		}

		if (_snap) {
			float yRotation = Mathf.Atan2 (_direction.x, _direction.z) * Mathf.Rad2Deg;
			gameObject.transform.eulerAngles = new Vector3 (0, yRotation, 0);
		} else {
			Quaternion rotationToDirection = Quaternion.LookRotation(_direction); //This is the rotation which will make us face _direction.
			Vector3 rotationEuler = Quaternion.Slerp(gameObject.transform.rotation, rotationToDirection, rotationSpeed * Time.deltaTime).eulerAngles; //Rotate base on our speed.
			rotationEuler.z = 0; //We only want to rotation on the y-axis.
			rotationEuler.x = 0; //We only want to rotation on the y-axis.
			gameObject.transform.rotation = Quaternion.Euler(rotationEuler);
		}
	}

	public void SlowDown(float _multiplier) {
		_multiplier = Mathf.Clamp(_multiplier, 0.0f, 1.0f);
		gameObject.GetComponent<Rigidbody>().velocity *= ((1.0f - Time.deltaTime) * _multiplier);
	}

	public void SetRotationSpeed(float _rotationSpeed) {
		rotationSpeed = Mathf.Max(0.0f, _rotationSpeed);
	}

	public float GetRotationSpeed() {
		return rotationSpeed;
	}

	public void SetMovementForce(float _movementForce) {
		movementForce = Mathf.Max(0.0f, _movementForce);
	}

	public float GetMovementForce() {
		return movementForce;
	}

	public void SetMaxMovementSpeed(float _maxMovementSpeed) {
		this.maxMovementSpeed = _maxMovementSpeed;
	}

	public float GetMaxMovementSpeed() {
		return maxMovementSpeed;
	}

}