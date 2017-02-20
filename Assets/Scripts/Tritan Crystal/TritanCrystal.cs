using UnityEngine;
using System.Collections;

public class TritanCrystal : MonoBehaviour {

	public GameObject explosionPrefab;
	public Vector3[] surroundPos;

	// Use this for initialization
	void Start () {
		surroundPos = new Vector3[12];
		float angle = 360.0f / surroundPos.Length;
		for (int i = 0; i < surroundPos.Length; ++i) {
			surroundPos[i] = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 2.0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Check if we diededed.
		if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0) {			
			GameObject.Instantiate(explosionPrefab); //イクスポロージョン！
			//GameObject.Destroy(gameObject);
			gameObject.SetActive(false);
		}
	}

	public void Reset() {
		gameObject.GetComponent<Health>().SetCurrentHealth(gameObject.GetComponent<Health>().GetMaxHealth());
		gameObject.SetActive(true);
	}

}