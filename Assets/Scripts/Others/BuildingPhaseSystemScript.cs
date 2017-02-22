using UnityEngine;
using System.Collections;

public class BuildingPhaseSystemScript : MonoBehaviour {

    public SelectedGridScript selectingGrid;        // The grid that does the selection for the building phase
    public GameObject[] towerPrefabs;               // The towers that players can choose to plant

    public int amountToBuildTowers { get; set; }
    public int numberOfBuildableWalls { get; set;}
        
	// Use this for initialization
    void Start()
    {
        amountToBuildTowers = 10000;
        numberOfBuildableWalls = 10;
        if (selectingGrid == null)
        {
            Debug.Log("No selecting grid to debug NOOB");
            return;
        }
        if (towerPrefabs.Length <= 0)
        {
            Debug.Log("No towers");
            return;
        }
        selectingGrid.selectedPrefab = towerPrefabs[0];
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            selectingGrid.selectedPrefab = towerPrefabs[0];
            selectingGrid.ChangeSelected();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            selectingGrid.selectedPrefab = towerPrefabs[1];
            selectingGrid.ChangeSelected();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            selectingGrid.selectedPrefab = towerPrefabs[2];
            selectingGrid.ChangeSelected();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            selectingGrid.selectedPrefab = null;
            selectingGrid.ChangeSelected();
        }
	}

    
}
