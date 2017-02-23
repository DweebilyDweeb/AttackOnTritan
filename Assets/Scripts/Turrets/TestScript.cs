using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 0);

    }

    // Update is called once per frame
    void Update () {
	}
}
