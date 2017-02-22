using UnityEngine;
using System.Collections;

public class AIAnimation : MonoBehaviour {

	private Animator animator;
	int velocityRatioHash;
	int attackHash;
	int healthHash;
	bool isAlive;

	// Use this for initialization
	void Awake() {
		animator = gameObject.GetComponent<Animator>();
		velocityRatioHash = Animator.StringToHash("Velocity Ratio");
		attackHash = Animator.StringToHash("Attack");
		healthHash = Animator.StringToHash("Health");
		isAlive = true;
	}

	void Start() {
	}

	// Update is called once per frame
	void Update () {
		//Health
		int currentHealth = gameObject.GetComponent<Health>().GetCurrentHealth();
		animator.SetInteger(healthHash, currentHealth);
		if (!gameObject.GetComponent<Health>().IsAlive() && isAlive) {
			animator.SetTrigger("Die");
		}
		isAlive = gameObject.GetComponent<Health>().IsAlive();
		if (!isAlive) {
			return;
		}

		//Velocity Ratio
		Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
		velocity.y = 0.0f;
		float velocityRatio = velocity.magnitude / gameObject.GetComponent<AIMovement>().GetMaxMovementSpeed();
		animator.SetFloat(velocityRatioHash, velocityRatio);

		//Attack
		if (gameObject.GetComponent<AIAttackCrystal>().IsAttacking()) {
			animator.SetTrigger(attackHash);
		}
	}

	public void Reset() {
		animator.Play("Idle");
	}

}