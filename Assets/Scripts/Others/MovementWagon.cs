using UnityEngine;
using System.Collections;

public class MovementWagon : MonoBehaviour {

    // Use this for initialization
    float movementSpeed;
    float z;
    bool forward;
	void Start () {
        movementSpeed = 0.2f;
        z = 0.0f;
        forward = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (forward)
        {
            // if z is below a certain amount change the direction
            z -= Time.deltaTime * movementSpeed;
            if (z < -0.15f)
            forward = false;
        }
        else
        {
            // if z is above a certain amount change the direction
            z += Time.deltaTime * movementSpeed;
            if (z > 0.15f)
            forward = true;
        }
                                                                 
        gameObject.GetComponent<Transform>().Translate(0, 0, z);
	}
}
