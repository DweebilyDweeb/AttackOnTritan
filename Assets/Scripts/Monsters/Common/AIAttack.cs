using UnityEngine;
using System.Collections;

public class AIAttack : MonoBehaviour {
	
	protected bool attacking; //Are we currently attacking?

	[SerializeField]
	protected int attackDamage;

	[SerializeField]
	protected float attackRange;

	[SerializeField]
	protected float attackRate; //How many times per seconds can we attack?
	protected float attackCountdownTimer;

	// Use this for initialization
	void Start () {
		InitValues();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCountdownTimer();
	}

	virtual protected void InitValues() {
		attacking = false;
		attackDamage = Mathf.Max(0, attackDamage);
		attackRange = Mathf.Max(0, attackRange);
		attackRate = Mathf.Max(0.01f, attackRate);
		attackCountdownTimer = 0.0f;
	}

	virtual protected void UpdateCountdownTimer() {
		if (attackCountdownTimer > 0.0f) {
			attackCountdownTimer = Mathf.Max(0.0f, attackCountdownTimer - Time.deltaTime);
		}
	}

	public virtual bool Attack() {
		return false;
	}

	protected virtual void DealDamage(Health _health) {
		if (_health == null) {
			return;
		}
		_health.DecreaseHealth(attackDamage);
	}

	//Non-Virtual Functions
	public bool IsAttacking() {
		return attacking;
	}

	public void SetAttackDamage(int _attackDamage) {
		attackDamage = Mathf.Max(0, _attackDamage);
	}

	public int GetAttackDamage() {
		return attackDamage;
	}

	public void SetAttackRange(float _attackRange) {
		attackRange = Mathf.Max(0.0f, _attackRange);
	}

	public float GetAttackRange() {
		return attackRange;
	}

	public void SetAttackRate(float _attackRate) {
		attackRate = Mathf.Max(0.01f, _attackRate);
	}

	public float GetAttackRate() {
		return attackRate;
	}

}