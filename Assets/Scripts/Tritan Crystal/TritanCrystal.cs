using UnityEngine;
using System.Collections;

public class TritanCrystal : MonoBehaviour {

	public GameObject explosionPrefab;

	public GameObject lightningPrefab;
	public GameObject floorElectricityPrefab;
	private GameObject lightning;
	private GameObject floorElectricity;

	[SerializeField]
	private float attackRadius;
	[SerializeField]
	private int attackDamage;
	[SerializeField]
	private float attackRate;
	private float attackCountdownTimer;

	// Use this for initialization
	void Start () {
		attackRate = Mathf.Max(0.001f, attackRate);
		attackCountdownTimer = 0.0f;

		lightning = GameObject.Instantiate(lightningPrefab);
		lightning.transform.position = gameObject.GetComponent<Transform>().position;

		floorElectricity = GameObject.Instantiate(floorElectricityPrefab);
		floorElectricity.GetComponent<Transform>().position = gameObject.transform.position + new Vector3(0.0f, 0.1f, 0.0f);
		ParticleSystem.ShapeModule particleShape = floorElectricity.GetComponent<ParticleSystem>().shape;
		particleShape.radius = attackRadius;
		floorElectricity.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//Check if we diededed.
		if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0) {			
			GameObject explosion = GameObject.Instantiate(explosionPrefab); //イクスポロージョン！
			explosion.transform.position = gameObject.transform.position;
			//GameObject.Destroy(gameObject);
			lightning.SetActive(false);
			floorElectricity.SetActive(false);
			gameObject.SetActive(false);
			return;
		}

		//How long till we're ready to dish out some pain?
		if (attackCountdownTimer > 0.0f) {
			attackCountdownTimer = Mathf.Max(0.0f, attackCountdownTimer - Time.deltaTime);
		}
		//Scan for enemies.
		Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, attackRadius, LayerMask.GetMask("Monster"));
		//If there's enemies nearby, attack them.
		if (hitColliders.Length > 0) {
			lightning.SetActive(true);
			floorElectricity.SetActive(true);
			if (attackCountdownTimer <= 0.0f) {
				DamageMonsters(hitColliders);
				attackCountdownTimer = 1.0f / attackRate;
			}
		} else {
			lightning.SetActive(false);
			floorElectricity.SetActive(false);
		}
	}

	public void Reset() {
		gameObject.GetComponent<Health>().SetCurrentHealth(gameObject.GetComponent<Health>().GetMaxHealth());
		gameObject.SetActive(true);
	}

	private void DamageMonsters(Collider[] _hitColliders) {
		for (int i = 0; i < _hitColliders.Length; ++i) {
			Health healthComponent = _hitColliders[i].gameObject.GetComponent<Health>();
			if (healthComponent == null) {
				print("Cannot Damage a GameObject with no Health component! Target GameObject: " + healthComponent.gameObject.name);
			} else {
				healthComponent.DecreaseHealth(attackDamage);
			}
		}
	}

	public float GetAttackRadius() {
		return attackRadius;
	}

	public float GetAttackRate() {
		return attackRate;
	}

	public int GetAttackDamage() {
		return attackDamage;
	}

}