using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectedGridScript : MonoBehaviour {

    public GameObject theGridSystem;

    private GridSystem theGridSystemScript;

    private GameObject selectedGrid;
    private Grid selectedGridScript;
    private Renderer theGridRenderer;

    public GameObject towerPrefab;
    private GameObject showcaseTower;

    public GameObject[] spawners;

    private bool isGridSelectable;
    private bool isAbleToPlaceTower;

    private int testCostOfTurrets;
    // Use this for initialization
    void Start()
    {
        if(theGridSystem == null)
        {
            print("No grid System in selected Grid");
            return;
        }

        theGridSystemScript = theGridSystem.GetComponent<GridSystem>();
        selectedGrid = theGridSystemScript.GetGrid(36);
        selectedGridScript = selectedGrid.GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;

        showcaseTower = GameObject.Instantiate(towerPrefab);
        showcaseTower.transform.SetParent(transform);
        showcaseTower.transform.position = transform.position;

        theGridRenderer = GetComponent<Renderer>();

        isGridSelectable = true;
        isAbleToPlaceTower = true;

        testCostOfTurrets = 5000;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectedTileToRight();
            PossiblePath();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectedTileToLeft();
            PossiblePath();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectedTileToUp();
            PossiblePath();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectedTileToDown();
            PossiblePath();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            PlantTurret();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveObjectsOnGrid();
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            print(testCostOfTurrets);
        }
        if (isGridSelectable && isAbleToPlaceTower)
        {
            theGridRenderer.material.SetColor("_Color", Color.green);
        }
        else if(selectedGridScript.tower || selectedGridScript.wall || !isAbleToPlaceTower)
        {
            theGridRenderer.material.SetColor("_Color", Color.red);
        }
        
    }

    
    private void SelectedTileToRight()
    {
        // Right
        int index = selectedGrid.transform.GetSiblingIndex();
		if (selectedGridScript.GetID() / theGridSystemScript.GetNumColumns() !=
			theGridSystem.transform.GetChild(index + 1).GetComponent<Grid>().GetID() / theGridSystemScript.GetNumColumns())
            return;
        
        selectedGrid = theGridSystem.transform.GetChild(index + 1).gameObject;
        selectedGridScript = selectedGrid.GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }

    private void SelectedTileToLeft()
    {
        // Left
        int index = selectedGrid.transform.GetSiblingIndex();
		if (selectedGridScript.GetID() % theGridSystemScript.GetNumColumns() == 0)
            return;
		if (selectedGridScript.GetID() / theGridSystemScript.GetNumColumns() !=
			theGridSystem.transform.GetChild(index -1).GetComponent<Grid>().GetID() / theGridSystemScript.GetNumColumns())
            return;
        selectedGrid = theGridSystem.transform.GetChild(index - 1).gameObject;
        selectedGridScript = selectedGrid.GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }

    private void SelectedTileToUp()
    {
        // Up
		int row = selectedGridScript.GetID() / theGridSystemScript.GetNumColumns();
		if (row == theGridSystemScript.GetNumRows())
            return;
		int nextID = selectedGridScript.GetID() + theGridSystemScript.GetNumColumns();
        while (theGridSystemScript.GetGrid(nextID) == null)
        {
			if (nextID / theGridSystemScript.GetNumColumns() == theGridSystemScript.GetNumRows())
                return;
			nextID += theGridSystemScript.GetNumColumns();
        }
        selectedGrid = theGridSystemScript.GetGrid(nextID);
        selectedGridScript = selectedGrid.GetComponent<Grid>();
       
        transform.position = selectedGrid.transform.position;
    }

    private void SelectedTileToDown()
    {
        // Down
		int row = selectedGridScript.GetID() / theGridSystemScript.GetNumColumns();
        if (row == 0)
            return;
		int nextID = selectedGridScript.GetID() - theGridSystemScript.GetNumColumns();
        while (theGridSystemScript.GetGrid(nextID) == null)
        {
			if (nextID / theGridSystemScript.GetNumColumns() == 0)
                return;
			nextID -= theGridSystemScript.GetNumColumns();
        }
        selectedGrid = theGridSystemScript.GetGrid(nextID);
        selectedGridScript = selectedGrid.GetComponent<Grid>();
        transform.position = selectedGrid.transform.position;
    }


    private void PlantTurret()
    {
        if (selectedGridScript.wall != null)
        {
            Debug.Log("No wall to plant your turret");
            return;
        }
        if(!isGridSelectable)
        {
            Debug.Log("Blocks AI path");
            return;
        }
        if(!isAbleToPlaceTower)
        {
            Debug.Log("Insufficient cost or hit max turret limit");
                return;
        }
        selectedGridScript.tower = GameObject.Instantiate(towerPrefab);
        selectedGridScript.tower.transform.position = selectedGrid.transform.position;
        testCostOfTurrets -= 1500;
        CalculateCostOfTower();
    }

    private void RemoveObjectsOnGrid()
    {
        if (selectedGridScript.wall == null && selectedGridScript.tower == null)
        {
            Debug.Log("There's nothing to sell you baka");
            return;
        }
        else if (selectedGridScript.tower != null)
        {
            Destroy(selectedGridScript.tower);
            testCostOfTurrets += 1500;
            CalculateCostOfTower();
        }
    }

    public void ChangeSelectedTower()
    {
        Destroy(showcaseTower);
        showcaseTower = GameObject.Instantiate(towerPrefab);
        showcaseTower.transform.SetParent(transform);
        showcaseTower.transform.position = transform.position;
        CalculateCostOfTower();
    }

    private void PossiblePath()
    {
        selectedGridScript.wall = new GameObject();
        for (int i = 0; i < spawners.Length; ++i)
        {
            int startID = spawners[i].GetComponent<MonsterSpawner>()._startID;
            int endID = spawners[i].GetComponent<MonsterSpawner>()._endID;
            List<int> path = theGridSystemScript.Search(startID, endID);
            if (path == null)
            {
                isGridSelectable = false;
                break;
            }
            else
            {
                isGridSelectable = true;
            }
        }
        Destroy(selectedGridScript.wall);
    }

    private void CalculateCostOfTower()
    {
        int costOfTurret = 1500;
        /* if (showcaseTower == null)
         * {
         *      Debug.Log("No showcase tower bro");
         * }
         * if (showcaseTower.GetComponent<TurretScript>() == null)
         * {
         *      Debug.Log("No turret script you fool");
         * }
         * int costOfTurret = showcaseTower.GetComponent<TurretScript>().Cost;
        */
        if (testCostOfTurrets < costOfTurret)
        {
            isAbleToPlaceTower = false;
        }
        else
        {
            isAbleToPlaceTower = true;
        }
    }
}