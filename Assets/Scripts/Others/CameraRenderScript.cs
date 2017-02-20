using UnityEngine;
using System.Collections;

public class CameraRenderScript : MonoBehaviour {

    private void Start()
    {
        enabled = false;
    }

    private void Update()
    {

    }

    void OnBecameVisible()
    {
        enabled = true;
    }
    void OnBecameInvisible()
    {
        enabled = false;
    }
}
