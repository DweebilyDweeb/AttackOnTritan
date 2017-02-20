using UnityEngine;
using System.Collections;

public class AIBehaviour : MonoBehaviour {

	public enum AI_STATE {
		MOVE,
		ATTACK,
		DEAD,
		NUM_AI_STATE,
	}

	private MonoBehaviour[] states;
	private AI_STATE currentState;

	void Awake() {
		currentState = AI_STATE.MOVE;
	}

	// Use this for initialization
	void Start () {
		states = new MonoBehaviour[(uint)AI_STATE.NUM_AI_STATE];
		states[(uint)AI_STATE.MOVE] = gameObject.GetComponent<AIMovement>();
		states[(uint)AI_STATE.ATTACK] = gameObject.GetComponent<AIAttack>();
		states[(uint)AI_STATE.DEAD] = gameObject.GetComponent<AIDeath>();
	}
	
	// Update is called once per frame
	void Update () {
		//We only want to enable the currentState.
		if (states[(uint)currentState].enabled == false) {
			states[(uint)currentState].enabled = true;
		}

		for (AI_STATE i = 0; i < AI_STATE.NUM_AI_STATE; ++i) {
			if (i != currentState) {
				states[(uint)i].enabled = false;
			}
		}

		switch (currentState) {
			case AI_STATE.MOVE:
				//If we reached the end, then attack.
				AIMovement moveState = (AIMovement)states[(uint)currentState];
				if (moveState.IsDone()) {
					print("Movement Done.");
					currentState = AI_STATE.ATTACK;
				}
				break;
			case AI_STATE.ATTACK:
				//AIAttack attackState = (AIAttack)states[(uint)currentState];
				break;
			case AI_STATE.DEAD:
				//Do nothing.
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

}