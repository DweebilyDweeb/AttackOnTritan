using UnityEngine;
using System.Collections;

public class WagonMoveForward : MonoBehaviour {
    float movementSpeed;
    float z;
    Vector3 startPosition;
    // Use this for initialization
    void Start () {
        movementSpeed = 10.0f;
        startPosition = gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //print("Framerate: " + 1.0f / Time.deltaTime);
        gameObject.GetComponent<Transform>().Translate(Time.deltaTime * -movementSpeed, 0, 0, Space.World);
        if (gameObject.GetComponent<Transform>().position.x < -3)
        {
            gameObject.GetComponent<Transform>().position = startPosition;
        }
    }
}
