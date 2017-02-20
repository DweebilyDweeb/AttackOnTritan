using UnityEngine;
using System.Collections;

public class GoblinAnimation : MonoBehaviour {

	private Animator animator;
	int velocityRatioHash;
	int attackHash;
	int healthHash;
	int deadHash;
	bool dead;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
		velocityRatioHash = Animator.StringToHash("Velocity Ratio");
		attackHash = Animator.StringToHash("Attack");
		healthHash = Animator.StringToHash("Health");
		deadHash = Animator.StringToHash("Dead");
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Velocity Ratio
		Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
		velocity.y = 0.0f;
		float velocityRatio = velocity.magnitude / gameObject.GetComponent<AIMovement>().GetMaxMovementSpeed();
		animator.SetFloat(velocityRatioHash, velocityRatio);

		//Health
		int currentHealth = gameObject.GetComponent<Health>().GetCurrentHealth();
		animator.SetInteger(healthHash, currentHealth);

		if (currentHealth > 0) {
			dead = false;
		} else if (currentHealth <= 0 && !dead) {
			animator.SetTrigger(deadHash);
			dead = true;
		}

		//Attack
		if (gameObject.GetComponent<AIAttack>().IsAttacking()) {
			animator.SetTrigger(attackHash);
		}
	}
}