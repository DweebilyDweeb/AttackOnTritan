using UnityEngine;
using System.Collections;

public class AIDeath : MonoBehaviour {

	[SerializeField]
	private float despawnTime;
	private float despawnCountdownTimer;

	void Awake() {
		Reset();
	}

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
		if (despawnCountdownTimer > 0.0f) {
			despawnCountdownTimer = Mathf.Max(0.0f, despawnCountdownTimer - Time.deltaTime);
		}
	}

	public bool IsDone() {
		return despawnCountdownTimer <= 0.0f;
	}

	public void Reset() {
		despawnCountdownTimer = despawnTime;
	}

	public void SetDespawnTime(float _despawnTime) {
		despawnTime = Mathf.Max(0.0f, _despawnTime);
		despawnCountdownTimer = Mathf.Min(despawnTime, despawnCountdownTimer);
	}

	public float GetDespawnTime() {
		return despawnTime;
	}

}