using UnityEngine;
using System.Collections;

public class AIBehaviour : MonoBehaviour {

	public enum AI_STATE {
		FOLLOW_PATH,
		ATTACK_CRYSTAL,
		DEAD,
		NUM_AI_STATE,
	}

	private MonoBehaviour[] states;
	private AI_STATE currentState;

	private bool stop;

	void Awake() {
		currentState = AI_STATE.FOLLOW_PATH;
	}

	// Use this for initialization
	void Start () {
		stop = false;
		states = new MonoBehaviour[(uint)AI_STATE.NUM_AI_STATE];
		states[(uint)AI_STATE.FOLLOW_PATH] = gameObject.GetComponent<AIFollowPath>();
		states[(uint)AI_STATE.ATTACK_CRYSTAL] = gameObject.GetComponent<AIAttackCrystal>();
		states[(uint)AI_STATE.DEAD] = gameObject.GetComponent<AIDeath>();
	}
	
	// Update is called once per frame
	void Update () {
		if (stop) {
			return;
		}

		//We only want to enable the currentState.
		if (states[(uint)currentState].enabled == false) {
			states[(uint)currentState].enabled = true;
		}

		//Disable the other states.
		for (AI_STATE i = 0; i < AI_STATE.NUM_AI_STATE; ++i) {
			if (i != currentState) {
				states[(uint)i].enabled = false;
			}
		}

		switch (currentState) {
			case AI_STATE.FOLLOW_PATH:
				//If we reached the end, then attack the crystal.
				AIFollowPath moveState = (AIFollowPath)states[(uint)currentState];
				if (moveState.IsDone()) {
					currentState = AI_STATE.ATTACK_CRYSTAL;
				}
				break;
			case AI_STATE.ATTACK_CRYSTAL:
				AIAttackCrystal attackState = (AIAttackCrystal)states[(uint)currentState];
				if (attackState.IsDone()) {
					stop = true;
				}
				break;
			case AI_STATE.DEAD:
				AIDeath deadState = (AIDeath)states[(uint)currentState];
				if (deadState.IsDone()) {
					stop = true;
					gameObject.SetActive(false);
					//GameObject.Destroy(gameObject);
				}
				break;
			default:
				print("Invalid AIBehaviour State!");
				break;
		}

		//No matter what, dead is dead.
		if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0) {
			currentState = AI_STATE.DEAD;
		}
	}
		
	public void Reset() {
		stop = false;
		gameObject.GetComponent<Health>().SetCurrentHealth(gameObject.GetComponent<Health>().GetMaxHealth());
		currentState = AI_STATE.FOLLOW_PATH;
		gameObject.SetActive(true);

		//Reset our stuff.
		gameObject.GetComponent<AIFollowPath>().Reset();
		gameObject.GetComponent<AIDeath>().Reset();
		gameObject.GetComponent<AIAnimation>().Reset();
		gameObject.GetComponent<AIAnimation>().enabled = true;
	}

}