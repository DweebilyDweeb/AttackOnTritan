using UnityEngine;
using System.Collections;

public class AIAttackCrystal : AIAttack {

	public GameObject tritanCrystal;
	private bool touchingCrystal;
	private AIMovement aiMovement;

	// Use this for initialization
	void Start () {
		InitValues();
		touchingCrystal = false;
		aiMovement = gameObject.GetComponent<AIMovement>();
	}
	
	// Update is called once per frame
	void Update () {		
		UpdateCountdownTimer();

		//Don't bother attacking if there's nothing to attack.
		if (tritanCrystal == null || tritanCrystal.activeSelf == false) {
			attacking = false;
			return;
		}

		//Look at the crystal.
		Vector3 directionToCrystal = tritanCrystal.transform.position - gameObject.transform.position;
		directionToCrystal.y = 0.0f; //Disregard the height difference.
		aiMovement.RotateTowards(directionToCrystal); //Face the crystal.
		if (!touchingCrystal) { //Don't move if we are already touching the crystal.
			aiMovement.Move(directionToCrystal);
		}

		if (attackCountdownTimer <= 0.0f) {
			if (Attack()) {
				//print("Attacked Crystal");
				attacking = true;
				attackCountdownTimer = 1.0f / attackRate;
			}
		} else {
			attacking = false;
		}
	}

	void OnCollisionEnter(Collision _collisionInfo) {
		if (_collisionInfo.gameObject == tritanCrystal) {
			touchingCrystal = true; //We're already touching the crystal. No need to move towards to anymore.
		}
	}

	void OnCollisionExit(Collision _collisionInfo) {
		if (_collisionInfo.gameObject == tritanCrystal) {
			touchingCrystal = false; //We're not touching the crystal anymore. Move towards it.
		}
	}

	//Returns if we've manage to attack anything.
	public override bool Attack() {
		bool result = false;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
		for (int i = 0; i < hitColliders.Length; ++i) {
			//If it's not a Tritan Crystal, don't hurt it.
			if (hitColliders[i].gameObject.CompareTag("Tritan Crystal") == false) {
				continue;
			}
			//If it's not facing us, also don't hurt it.
			if (Vector3.Dot(gameObject.GetComponent<Transform>().forward, hitColliders[i].gameObject.GetComponent<Transform>().position) < 0.0f) {
				continue;
			}
			DealDamage(hitColliders[i].gameObject.GetComponent<Health>());
			result = true;
		}
		return result;
	}

	public bool IsDone() {
		if (tritanCrystal == null || tritanCrystal.GetComponent<Health>() == null || tritanCrystal.GetComponent<Health>().IsDead()) {
			return true;
		}
		return false;
	}

}