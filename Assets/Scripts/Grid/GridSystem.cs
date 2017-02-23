using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using GridID = System.Int32;

public class GridSystem : MonoBehaviour
{
	private class SearchNode
	{
		public GridID gridID;
		public int gCost; //From the start to here.
		public int hCost; //From here to the end.
		public SearchNode parent;

		public SearchNode(GridID _gridID, int _gCost = 0, int _hCost = 0)
		{
			gridID = _gridID;
			gCost = _gCost;
			hCost = _hCost;
			parent = null;
		}

		public int GetFCost()
		{
			return gCost + hCost;
		}
	}

	public Vector3 originPosition; // Position of the start

	public Grid gridPrefab; // Prefab of the grid we're using.

	public TextAsset csvFile; // Reference of CSV file

	public int initNumRows, initNumCols;
	[SerializeField][HideInInspector]
	private int numRows; // Number of rows
	[SerializeField][HideInInspector]
	private int numColumns; // Number of columns
	[SerializeField]
	private GameObject[] grids; // Dynamic array of grids[numRows * numCols]

	public GameObject tritanCrystalPrefab;
	[SerializeField]
	private List<GridID> tritanCrystalGridIDs;

	void Awake()
	{		
	}

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	private int ComputeID(int _row, int _column)
	{
		return _row * numColumns + _column;
	}

	private void LoadGrid()
	{
		print("Loading Grid");
		numRows = Mathf.Max(0, initNumRows);
		numColumns = Mathf.Max(0, initNumCols);
		if (numRows == 0 || numColumns == 0)
		{
			return;
		}

		grids = new GameObject[numRows * numColumns];
		for (int row = 0; row < numRows; ++row)
		{
			for (int column = 0; column < numColumns; ++column)
			{
				GridID id = ComputeID(row, column);
				grids [id] = GameObject.Instantiate (gridPrefab.gameObject);
				grids [id].GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform>());
				grids[id].GetComponent<Grid>().FindParentGridSystem();
				Vector3 gridScale = gridPrefab.gameObject.GetComponent<Transform> ().localScale;
				grids [id].GetComponent<Transform> ().position = new Vector3 (column * gridScale.x + originPosition.x, originPosition.y, row * gridScale.z + originPosition.z);
				grids [id].GetComponent<Grid>().SetID (id);
			}
		}
	}

	// Loads CSV file
	private void LoadCSV()
	{
		print("Loading Grid From CSV.");
		char lineSeparator = '\n';
		char fieldSeparator = ',';

		//Load the CSV file into a string.
		string[] lines = csvFile.text.Split(lineSeparator);

		//Get the number of rows & columns.
		numRows = lines.Length;
		numColumns = 0;
		foreach (string record in lines)
		{
			int numCommas = 0;
			foreach (char c in record)
			{
				if (c == fieldSeparator)
				{
					++numCommas;
				}
			}
			numColumns = Mathf.Max(numCommas + 1, numColumns); //The number of columns is always the number of commas + 1.
		}

		//Create our grid.
		grids = new GameObject[numRows * numColumns];

		int row = 0;
        foreach(string record in lines)
        {
			int column = 0;
            string[] fields = record.Split(fieldSeparator);
            foreach(string field in fields)
            {
				if (field.Length > 0 && field[0] == '1')
				{
					GridID id = ComputeID(row, column);
					grids[id] = GameObject.Instantiate (gridPrefab.gameObject);
					grids[id].GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform>());
					grids[id].GetComponent<Grid>().FindParentGridSystem();
					Vector3 gridScale = gridPrefab.gameObject.GetComponent<Transform> ().localScale;
					grids[id].GetComponent<Transform> ().position = new Vector3 (column * gridScale.x + originPosition.x, originPosition.y, row * gridScale.z + originPosition.z);
					grids[id].GetComponent<Grid>().SetID (id);
				}
				++column;
            }
			++row;
        }
	}

	public void SetGridsNeighbours()
	{
		for (int row = 0; row < numRows; ++row)
		{
			for (int column = 0; column < numColumns; ++column)
			{
				GridID id = row * numColumns + column;
				if (grids [id] == null)
				{
					continue;
				}

				GridID previousRowID = -1;
				GridID nextRowID = -1;
				GridID previousColumnID = -1;
				GridID nextColumnID = -1;

				{
					GameObject previousRowNeighbour = GetGrid (row - 1, column);
					if (previousRowNeighbour != null)
					{
						previousRowID = previousRowNeighbour.GetComponent<Grid> ().GetID ();
					}
				}
				{
					GameObject nextRowNeighbour = GetGrid (row + 1, column);
					if (nextRowNeighbour != null)
					{
						nextRowID = nextRowNeighbour.GetComponent<Grid> ().GetID ();
					}
				}
				{
					GameObject previousColumnNeighbour = GetGrid (row, column - 1);
					if (previousColumnNeighbour != null)
					{
						previousColumnID = previousColumnNeighbour.GetComponent<Grid> ().GetID ();
					}
				}
				{
					GameObject nextColumnNeighbour = GetGrid (row, column + 1);
					if (nextColumnNeighbour != null)
					{
						nextColumnID = nextColumnNeighbour.GetComponent<Grid> ().GetID ();
					}
				}

				grids [id].GetComponent<Grid> ().SetNeighbourIDs (previousRowID, nextRowID, previousColumnID, nextColumnID);
			}
		}
	}

	// Get the tile from its ID
	public GameObject GetGrid(GridID _id)
	{
		if (grids == null || _id < 0 || _id >= grids.Length)
		{
			return null;
		}

		return grids[_id];
	}

	public GameObject GetGrid(int _row, int _column)
	{
		if (_row < 0 || _row >= numRows)
		{
			return null;
		}
		if (_column < 0 || _column >= numColumns)
		{
			return null;
		}

		return grids[_row * numColumns + _column];
	}

	private int ComputeHCost(GridID _startID, GridID _endID)
	{
		int startColumn = _startID % numColumns;
		int startRow = (_startID - startColumn) / numColumns; //Actually no need to minus first.

		int endColumn = _endID & numColumns;
		int endRow = (_endID - endColumn) / numColumns; //Actually no need to minus first.

		return Mathf.Abs(endRow - startRow) + Mathf.Abs (endColumn - startColumn);
	}

	private bool InList(List<SearchNode> _list, GridID _gridID)
	{
		for (int i = 0; i < _list.Count; ++i)
		{
			if (_list [i].gridID == _gridID)
			{
				return true;
			}
		}

		return false;
	}

	// Pathfinding search function from start to end
	public List<GridID> Search(GridID _startID, GridID _endID)
	{
		if (GetGrid (_startID) == null || GetGrid(_endID) == null) //Check if the path is even remotely possible.
		{
			print("Unable to search path. _startID or _endID is invalid.");
			return null;
		}

		if (GetGrid (_startID).GetComponent<Grid> ().wall != null || GetGrid (_endID).GetComponent<Grid> ().wall != null) //I can't go through walls. What am I, a ghost?
		{
			print("Unable to search path. _startID or _endID has a wall.");
			return null;
		}

		List<GridID> result = new List<GridID>();
		List<SearchNode> openList = new List<SearchNode>();
		List<SearchNode> closedList = new List<SearchNode>();

		openList.Add(new SearchNode(_startID, 0, ComputeHCost(_startID, _endID)));

		//Look through our Open List.
		while (openList.Count != 0)
		{
			//Find the cheapest node.
			SearchNode cheapestNode = openList [0];
			for (int i = 1; i < openList.Count; ++i)
			{
				if (openList [i].GetFCost() < cheapestNode.GetFCost())
				{
					cheapestNode = openList [i];
				}
			}
				
			//Look through our neighbours and get the cheapest one.
			List<GridID> neighbourIDs = GetGrid(cheapestNode.gridID).GetComponent<Grid>().GetNeighbourIDs();
			for (int j = 0; j < neighbourIDs.Count; ++j)
			{
				int neighbourID = neighbourIDs[j];

				if (neighbourID < 0) //No neighbour there.
				{
					continue;
				}

				if (GetGrid (neighbourID).GetComponent<Grid> ().wall != null) //The neighbour has a wall. No go.
				{
					continue;
				}

				if (InList (closedList, neighbourID)) //If it is in the closed list, move on.
				{
					continue;
				}

				SearchNode neighbourNode = null;

				for (int k = 0; k < openList.Count; ++k) //Check if it is in the open list.
				{
					if (openList[k].gridID == neighbourID)
					{
						neighbourNode = openList [k];
						break;
					}
				}

				int movementCost = 1;
				if (neighbourNode != null) //If it is in the Open List
				{
					int gCost = cheapestNode.gCost + movementCost;
					if (gCost < neighbourNode.gCost)
					{
						neighbourNode.gCost = gCost;
						neighbourNode.parent = cheapestNode;
					}
				}
				else //Make a new node.
				{
					neighbourNode = new SearchNode (neighbourID, movementCost + cheapestNode.gCost, ComputeHCost (neighbourID, _endID));
					neighbourNode.parent = cheapestNode;
					openList.Add (neighbourNode);
				}

				if (neighbourNode.gridID == _endID) //Have we found the one we're looking for? (As in the node, not the love of our lives.)
				{
					SearchNode currentNode = neighbourNode;
					while (currentNode != null)
					{
						result.Insert (0, currentNode.gridID);
						currentNode = currentNode.parent;
					}

					return result; //Return our path.
				}
			}

			openList.Remove (cheapestNode);
			closedList.Add (cheapestNode);
		}

		return null; //Paiseh, no path found.
	}

	public void Clear()
	{
		numRows = 0;
		numColumns = 0;

		if (grids == null)
		{
			return;
		}


		for (int i = 0; i < grids.Length; ++i) {
			if (grids[i] == null)
			{
				continue;
			}
			GameObject.DestroyImmediate(grids[i]);
			grids[i] = null;
		}

		grids = null;
	}

	public void Load()
	{
		if (gridPrefab == null) //If there's no prefab, don't do anything.
		{
			return;
		}

		if (csvFile == null) //Load the CSV file by default.
		{
			Clear();
			LoadGrid();
		}
		else
		{
			Clear();
			LoadCSV();
		}

		SetGridsNeighbours();

		foreach (GridID gridID in tritanCrystalGridIDs)
		{
			GameObject grid = GetGrid(gridID);
			if (grid != null)
			{
				GameObject tritanCrystal = GameObject.Instantiate(tritanCrystalPrefab);
				tritanCrystal.transform.SetParent(grid.transform);
				tritanCrystal.transform.localPosition = new Vector3(0, 0, 0);
				grid.GetComponent<Grid>().tritanCrystal = tritanCrystal;
			}
		}
	}

	public void SaveToCSV(string _folderPath)
	{
		//If there's a grid, save as 1. Else, save as 0.
		string[] gridData = new string[numRows]; //Add numRows to account for the extra "/r" at the end.
		for (int row = 0; row < numRows; ++row)
		{			
			gridData[row] = "";
			for (int column = 0; column < numColumns; ++column)
			{
				GridID gridID = ComputeID(row, column);
				if (GetGrid(gridID) == null)
				{
					//string.Concat(gridData[row], "0,");
					gridData[row] += "0,";
				}
				else
				{
					//string.Concat(gridData[row], "1,");
					gridData[row] += "1,";
				}
			}
		}

		string fileName = "New Grid Layout";
		string filePath = _folderPath + "\\" + fileName; //The file path.
		int numTries = 1; //Start from index 1 instead of zero because that seems to be how most files are done.
		while (File.Exists(filePath + ".csv"))
		{
			filePath = _folderPath + "\\" + fileName + " (" + numTries.ToString() + ")";
			++numTries;
		}
		filePath += ".csv";

		var file = File.CreateText(filePath);
		for (int row = 0; row < numRows; ++row)
		{
			//print(gridData[row]);
			file.WriteLine(gridData[row]);
		}
		file.Close();

		print("Saving Grid Layout to " + filePath + ". Refresh folder for it to show up. If it does not appear after 30 seconds, tell Terry. Remember to Calculate Neighbours if you haven't done so.");
	}

	public void RenderGrids(bool _render = true)
	{
		for (int i = 0; i < grids.Length; ++i)
		{
			if (grids[i] != null)
			{
				grids[i].GetComponent<MeshRenderer>().enabled = _render;
			}
		}
	}

	public void EnableGridCollider(bool _colliderOn = true)
	{
		for (int i = 0; i < grids.Length; ++i)
		{
			if (grids[i] != null)
			{
				grids[i].GetComponent<BoxCollider>().enabled = _colliderOn;
			}
		}
	}

	public int GetNumRows()
	{
		return numRows;
	}

	public int GetNumColumns()
	{
		return numColumns;
	}

	public List<GridID> GetTritanCrystalGridIDs()
	{
		return tritanCrystalGridIDs;
	}

    public void RemoveGridsInsideTerrain(Terrain _terrain)
	{
        for (int i = 0; i < grids.Length; ++i)
		{
			if (grids[i] == null) 
			{
				continue;
			}

			if (_terrain.SampleHeight(grids[i].transform.position) > grids[i].transform.position.y) {
				GameObject.DestroyImmediate(grids[i]);
			} else {
				Vector3[] corners = new Vector3[4];
				corners[0] = grids[i].GetComponent<Transform>().position + (grids[i].GetComponent<Transform>().right * grids[i].transform.localScale.x * 0.5f);
				corners[1] = grids[i].GetComponent<Transform>().position - (grids[i].GetComponent<Transform>().right * grids[i].transform.localScale.x * 0.5f);
				corners[2] = grids[i].GetComponent<Transform>().position + (grids[i].GetComponent<Transform>().forward * grids[i].transform.localScale.z * 0.5f);
				corners[3] = grids[i].GetComponent<Transform>().position - (grids[i].GetComponent<Transform>().forward * grids[i].transform.localScale.z * 0.5f);

				for (int j = 0; j < 4; ++j) {
					if (_terrain.SampleHeight(corners[j]) > grids[i].transform.position.y) {
						GameObject.DestroyImmediate(grids[i]);
						break;
					}
				}
			}
        }

		SetGridsNeighbours();
    }

}