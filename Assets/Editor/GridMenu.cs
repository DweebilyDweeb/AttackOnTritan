using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridMenu {

	[MenuItem ("Grid/Load Grid")]
	static void LoadGrid() {
		//Debug.Log("Do Something");
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to load grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to load grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().Load();
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Load Grid");
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem ("Grid/Clear Grid")]
	static void ClearGrid() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to clear grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to clear grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().Clear();
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Clear Grid");
			}
		}
	}

	[MenuItem ("Grid/Calculate Neighbours")]
	static void CalculateNeighbours() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to clear grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to clear grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().SetGridsNeighbours();
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Clear Grid");
			}
		}
	}

	[MenuItem ("Grid/Save To File")]
	static void SaveGridToFile() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to clear grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to clear grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().SaveToCSV("Assets\\Grid Layouts");
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Clear Grid");
			}
		}
	}

	[MenuItem("Grid/Render Grids")]
	static void RenderGrids() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to clear grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to clear grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().RenderGrids(true);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem("Grid/Derender Grids")]
	static void DerenderGrids() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to clear grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to clear grid.");
			} else {
				gridSystem.GetComponent<GridSystem>().RenderGrids(false);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem("Grid/Remove Grids Inside Terrain")]
	static void RemoveGridsInsideTerrain() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid Terrain Remover selected. Unable to clear grid.");
		} else {
			GameObject gridTerrainRemover = (GameObject)Selection.activeObject;
			if (gridTerrainRemover.GetComponent<GridTerrainRemover>() == null) {
				Debug.Log("Selected GameObject has no Grid Terrain Remover. Unable to clear grid.");
			} else {
				gridTerrainRemover.GetComponent<GridTerrainRemover>().RemoveGridsInsideTerrain();
				EditorUtility.SetDirty(gridTerrainRemover.GetComponent<GridTerrainRemover>().gridSystem);
			}
		}
	}

}