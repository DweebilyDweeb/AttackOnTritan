using UnityEngine;
using System.Collections;

public class PostWaveScript : MonoBehaviour {

    public int getCurrency;      // Currency player will be getting
    public int getWalls;         // Walls player will be getting

    public BuildingPhaseSystemScript buildingPhaseSystem;    // The building phase system

	// Use this for initialization
	void Start () {
        if (getCurrency < 0)
        {
            Debug.Log("player poor ah bang");
            return;
        }
        if (getWalls < 0)
        {
            Debug.Log("mexican did not pay");
            return;
        }
        if(buildingPhaseSystem == null)
        {
            Debug.Log("Building phase system is foiled");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddCurrencyAndWalls()
    {
        // Add the walls to the number of walls available
        buildingPhaseSystem.numberOfBuildableWalls += getWalls;
        // Increase the amount of currency you currently have
        buildingPhaseSystem.amountToBuildTowers += getCurrency;
        // Destroy this script since there's nothing to do with this script anymore
        Destroy(this);
    }
}
