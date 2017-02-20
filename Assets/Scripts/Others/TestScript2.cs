using UnityEngine;
using System.Collections;

public class TestScript2 : MonoBehaviour {

    public GameObject theSelectingGrid;
    public GameObject[] towerPrefabs;

    private SelectedGridScript theSelectingGridScript;
    private int towerCount;

	// Use this for initialization
	void Start () {
	    if(theSelectingGrid == null)
        {
            print("No selecting grid");
            return;
        }
        theSelectingGridScript = theSelectingGrid.GetComponent<SelectedGridScript>();
        if(theSelectingGridScript == null)
        {
            print("no selecting grid script");
            return;
        }

        if(towerPrefabs.Length <= 0)
        {
            print("No towers");
            return;
        }
        towerCount = towerPrefabs.Length;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Z))
        {
            theSelectingGridScript.towerPrefab = towerPrefabs[0];
            theSelectingGridScript.ChangeSelectedTower();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            theSelectingGridScript.towerPrefab = towerPrefabs[1];
            theSelectingGridScript.ChangeSelectedTower();
        }
	}
}
