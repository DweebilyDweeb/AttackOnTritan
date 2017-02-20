using UnityEngine;
using System.Collections;

public class BuildingPhaseSystemScript : MonoBehaviour {

    public SelectedGridScript selectingGrid;

    public int amountToBuildTowers { get; set; }
    public int numberOfBuildableWalls { get; set;}

	// Use this for initialization
	void Start () {
	    if(selectingGrid == null)
        {
            Debug.Log("No selecting grid to debug NOOB");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
