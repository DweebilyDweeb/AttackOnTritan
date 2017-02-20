using UnityEngine;
using System.Collections;

public class MainMenuRotate : MonoBehaviour
{
    float z;
    float rotatespeed;
    // Use this for initialization
    void Start()
    {
        z = 0.0f;
        rotatespeed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        z += (Time.fixedDeltaTime * rotatespeed);
        if (z > 360)
        {
            z = 0.0f;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, z, 0);
        //transform.rotation = new Quaternion(0, 1, 0, z);

    }
}
