using UnityEngine;
using System.Collections;

public class AIAttack : MonoBehaviour {

	[SerializeField]
	private float attackRate;
	private float attackCountdownTimer;
	private bool attack;

	// Use this for initialization
	void Start () {
		attack = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (attackRate < 0.0f) {
			return;
		}

		attackCountdownTimer -= Time.deltaTime;
		if (attackCountdownTimer <= 0.0f) {
			attack = true;
			attackCountdownTimer = 1.0f / attackRate;
		} else {
			attack = false;
		}
	}

	public virtual bool IsAttacking() {
		return attack;
	}

}