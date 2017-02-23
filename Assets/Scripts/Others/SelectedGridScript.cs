
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectedGridScript : MonoBehaviour
{
    // Check if the placing logic is correct
    private bool isAbleToPlacePrefab;
    // Check if there are enough resources to build
    private bool hasResources;

    // The renderer for the grid
    private Renderer theGridRenderer;
    // The selected and the actual grid in the gridSystem
    private Grid selectedGrid;

    // Where should the selected grid be at when first instantiated
    public int startSelectedGridID;

    // The grid system
    public GridSystem theGridSystem;
    // The building phase 
    public BuildingPhaseSystemScript buildingPhaseSystem;

    // Where the spawners are to check for ai blocking
    public GameObject[] spawners;
    // The selection the player chose
    public GameObject selectedPrefab;

    // Not CSGO. Just the instantiated version of the selectedPrefab, to show where the turret/tower is
    private GameObject showcaseGO;

    // Testing
    public Camera mainCamera;
    // Use this for initialization
    void Start()
    {
        isAbleToPlacePrefab = true;
        hasResources = true;

        theGridRenderer = GetComponent<Renderer>();

        if (theGridSystem == null)
        {
            Debug.Log("No grid system found");
            return;
        }
        if (theGridSystem.GetGrid(startSelectedGridID) == null)
        {
            Debug.Log("Not valid grid");
            return;
        }
        /*selectedGrid = theGridSystem.GetGrid(startSelectedGridID).GetComponent<Grid>();
        transform.position = theGridSystem.GetGrid(startSelectedGridID).transform.position;

        CanGameobjectBePlaced();
        showcaseGO = GameObject.Instantiate(selectedPrefab);
        showcaseGO.transform.SetParent(transform);
        showcaseGO.transform.position = selectedGrid.transform.position;
        */

        // Testing
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<Grid>() != null)
            {
                print(hit.transform.GetComponent<Grid>().GetID());
                selectedGrid = hit.transform.GetComponent<Grid>();
                transform.position = theGridSystem.GetGrid(selectedGrid.GetID()).transform.position;
                showcaseGO = GameObject.Instantiate(selectedPrefab);
                showcaseGO.transform.SetParent(transform);
                showcaseGO.transform.position = selectedGrid.transform.position;
            }
        }
        CanGameobjectBePlaced();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Move the tile
            //SelectedTileToRight();
            // Translate the selected accordingly
            ChangeTurretTranslateOnTower();
            // Check if can be placed
            CanGameobjectBePlaced();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           // SelectedTileToLeft();
            ChangeTurretTranslateOnTower();
            CanGameobjectBePlaced();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //SelectedTileToUp();
            ChangeTurretTranslateOnTower();
            CanGameobjectBePlaced();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //SelectedTileToDown();
            ChangeTurretTranslateOnTower();
            CanGameobjectBePlaced();
        }
        else if (Input.GetKeyDown(KeyCode.Return))*/
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<Grid>() != null)
            {
                if (selectedGrid != hit.transform.GetComponent<Grid>())
                {
                    selectedGrid = hit.transform.GetComponent<Grid>();
                    transform.position = theGridSystem.GetGrid(selectedGrid.GetID()).transform.position;
                    ChangeTurretTranslateOnTower();
                    CanGameobjectBePlaced();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isAbleToPlacePrefab && hasResources)
                PlaceGameObject();
        }

        // If there is no selection
        if (selectedPrefab == null)
        {
            // Change color to default white
            theGridRenderer.material.SetColor("_Color", Color.white);
        }
        // If it is able to place the prefab
        else if (isAbleToPlacePrefab && hasResources)
        {
            // Change color to peaceful green
            theGridRenderer.material.SetColor("_Color", Color.green);
        }
        // If it is unable to place the prefab
        else if (!isAbleToPlacePrefab || !hasResources)
        {
            // Change color to bloody red
            theGridRenderer.material.SetColor("_Color", Color.red);
        }
    }

    // To move the tile in 4 directions 
    /*private void SelectedTileToRight()
    {
        // Right
        int index = selectedGrid.transform.GetSiblingIndex();
        if (selectedGrid.GetID() / theGridSystem.GetNumColumns() !=
            theGridSystem.transform.GetChild(index + 1).GetComponent<Grid>().GetID() / theGridSystem.GetNumColumns())
            return;

        selectedGrid = theGridSystem.transform.GetChild(index + 1).GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }
    private void SelectedTileToLeft()
    {
        // Left
        int index = selectedGrid.transform.GetSiblingIndex();
        if (selectedGrid.GetID() % theGridSystem.GetNumColumns() == 0)
            return;
        if (selectedGrid.GetID() / theGridSystem.GetNumColumns() !=
            theGridSystem.transform.GetChild(index - 1).GetComponent<Grid>().GetID() / theGridSystem.GetNumColumns())
            return;
        selectedGrid = theGridSystem.transform.GetChild(index - 1).GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }
    private void SelectedTileToUp()
    {
        // Up
        int row = selectedGrid.GetID() / theGridSystem.GetNumColumns();
        if (row == theGridSystem.GetNumRows())
            return;
        int nextID = selectedGrid.GetID() + theGridSystem.GetNumColumns();
        while (theGridSystem.GetGrid(nextID) == null)
        {
            if (nextID / theGridSystem.GetNumColumns() == theGridSystem.GetNumRows())
                return;
            nextID += theGridSystem.GetNumColumns();
        }
        selectedGrid = theGridSystem.GetGrid(nextID).GetComponent<Grid>();

        transform.position = selectedGrid.transform.position;
    }
    private void SelectedTileToDown()
    {
        // Down
        int row = selectedGrid.GetID() / theGridSystem.GetNumColumns();
        if (row == 0)
            return;
        int nextID = selectedGrid.GetID() - theGridSystem.GetNumColumns();
        while (theGridSystem.GetGrid(nextID) == null)
        {
            if (nextID / theGridSystem.GetNumColumns() == 0)
                return;
            nextID -= theGridSystem.GetNumColumns();
        }
        selectedGrid = theGridSystem.GetGrid(nextID).GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }
    */
    // Check functions
    private void CheckWallsCanBuild()
    {
        if(selectedGrid.wall != null)
        {
            isAbleToPlacePrefab = false;
            return;
        }
        selectedGrid.wall = new GameObject();
        for (int i = 0; i < spawners.Length; ++i)
        {
            int startID = spawners[i].GetComponent<MonsterSpawner>()._startID;
            int endID = spawners[i].GetComponent<MonsterSpawner>()._endID;
            List<int> path = theGridSystem.Search(startID, endID);
            if (path == null)
            {
                isAbleToPlacePrefab = false;
                break;
            }
            else
            {
                isAbleToPlacePrefab = true;
            }
        }
        Destroy(selectedGrid.wall);
    }
    private void CheckTurretsCanBuild()
    {
        isAbleToPlacePrefab = (selectedGrid.wall != null);
    }

    // Call the above check functions according to the selection and changing the selection grid color accordingly
    private void CanGameobjectBePlaced()
    {
        if (selectedPrefab.CompareTag("Wall"))
        {
            CheckWallsCanBuild();
        }
        else if (selectedPrefab.CompareTag("Turret"))
        {
            CheckTurretsCanBuild();
        }
        else
            return;
    }
    // Called to place the gameobject
    private void PlaceGameObject()
    {
        // If we are selecting a turret
        if (selectedPrefab.CompareTag("Turret"))
        {
            // If there's a wall and there are no towers
            if (selectedGrid.wall != null && selectedGrid.tower == null)
            {
                selectedGrid.tower = GameObject.Instantiate(selectedPrefab);
                selectedGrid.tower.transform.SetParent(selectedGrid.wall.transform);
                selectedGrid.tower.transform.position = selectedGrid.wall.transform.position;

                CapsuleCollider wallCollider = selectedGrid.wall.GetComponent<CapsuleCollider>();
                selectedGrid.tower.transform.position = new Vector3(selectedGrid.wall.transform.position.x, (wallCollider.height + wallCollider.center.y) / 2, selectedGrid.wall.transform.position.z); 
                
                buildingPhaseSystem.amountToBuildTowers -= 1500;
            }
            // The situation when there's no wall is already checked so we skip that
            // If there's a wall and there is a tower
            else if(selectedGrid.tower != null)
            {
                // If the tower is of the same type, Upgrade!

                // If not, Sell!
            }
        }
        // If we are selecting a wall
        else if(selectedPrefab.CompareTag("Wall"))
        {
            // If there's no wall
            if (selectedGrid.wall == null)
            {
                selectedGrid.wall = GameObject.Instantiate(selectedPrefab);
                selectedGrid.wall.AddComponent<TurretTowerScript>();
                selectedGrid.wall.GetComponent<TurretTowerScript>().tileID = selectedGrid.GetID();
                selectedGrid.wall.GetComponent<TurretTowerScript>().gridSystem = theGridSystem;
                buildingPhaseSystem.numberOfBuildableWalls -= 1;
            }
        }
        CheckCostAndNumber();
    }

    // Called when the selection of the prefab changes
    public void ChangeSelected()
    {
        Destroy(showcaseGO);
        if (selectedPrefab == null)
            return;
        showcaseGO = GameObject.Instantiate(selectedPrefab);
        showcaseGO.transform.SetParent(transform);
        showcaseGO.transform.position = transform.position;
        CanGameobjectBePlaced();
        ChangeTurretTranslateOnTower();
        CheckCostAndNumber();
    }
    // A function for the turrets, to put the selection of the prefab *above* the wall to make it look realistic
    private void ChangeTurretTranslateOnTower()
    {
        if (!selectedPrefab.CompareTag("Turret"))
            return ;
        if (selectedGrid.wall != null)
        {
            CapsuleCollider wallCollider = selectedGrid.wall.GetComponent<CapsuleCollider>();
            showcaseGO.transform.position = new Vector3(showcaseGO.transform.position.x, (wallCollider.height + wallCollider.center.y) / 2, showcaseGO.transform.position.z); 
        }
        else
        {
            showcaseGO.transform.position = selectedGrid.transform.position;
        }
    }

    // A function to check if there is still enough currency and walls to keep building, else turn the grid red
    private void CheckCostAndNumber()
    {
        print(buildingPhaseSystem.amountToBuildTowers + "," + buildingPhaseSystem.numberOfBuildableWalls);
        if (selectedPrefab.CompareTag("Turret"))
        {
            if (buildingPhaseSystem.amountToBuildTowers < 1500 /*selectedPrefab get cost whatsoever*/)
            {
                hasResources = false;
            }
            else
            {
                hasResources = true;
            }
        }
        else if (selectedPrefab.CompareTag("Wall"))
        {
            if (buildingPhaseSystem.numberOfBuildableWalls <= 0)
            {
                hasResources = false;
            }
            else
            {
                hasResources = true;
            }
        }

    }
}
 