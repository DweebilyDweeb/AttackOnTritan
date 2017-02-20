using UnityEngine;
using System.Collections;

public class TritanCrystalExplosion : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
		if (gameObject.GetComponent<AudioSource>().isPlaying == false && gameObject.GetComponent<ParticleSystem>().IsAlive(true) == false) {
			GameObject.Destroy(gameObject);
		}
	}
}