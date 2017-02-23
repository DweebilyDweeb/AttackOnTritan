using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour {

	private Resolution[] resolutions;

	// Use this forq initialization
	void Start () {
		Resolution[] resolutions = Screen.resolutions;
		/*foreach (Resolution res in resolutions) {
			//print(res.width + "x" + res.height);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetSelectedResolution() {
		Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
	}

}