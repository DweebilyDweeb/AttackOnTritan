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

	[MenuItem ("Grid/Save To File")]
	static void SaveGridToFile() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to save to file.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to save to file.");
			} else {
				gridSystem.GetComponent<GridSystem>().SaveToCSV("Assets\\Grid Layouts");
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Clear Grid");
			}
		}
	}

	[MenuItem ("Grid/Calculate Neighbours")]
	static void CalculateNeighbours() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to calculate neighbours.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to calculate neighbours.");
			} else {
				gridSystem.GetComponent<GridSystem>().SetGridsNeighbours();
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
				//Undo.RecordObject(gridSystem.GetComponent<GridSystem>(), gridSystem.name + "Clear Grid");
			}
		}
	}

	[MenuItem("Grid/Render Grids")]
	static void RenderGrids() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to render grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to render grids.");
			} else {
				gridSystem.GetComponent<GridSystem>().RenderGrids(true);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem("Grid/Derender Grids")]
	static void DerenderGrids() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to derender grid.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to derender grids.");
			} else {
				gridSystem.GetComponent<GridSystem>().RenderGrids(false);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem ("Grid/Enable Grid Colliders")]
	static void EnableGridCollider() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to enable grid colliders.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to enable grid colliders.");
			} else {
				gridSystem.GetComponent<GridSystem>().EnableGridCollider(true);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem ("Grid/Disable Grid Colliders")]
	static void DisableGridCollider() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid System selected. Unable to disable grid colliders.");
		} else {
			GameObject gridSystem = (GameObject)Selection.activeObject;
			if (gridSystem.GetComponent<GridSystem>() == null) {
				Debug.Log("Selected GameObject has no Grid System. Unable to disable grid colliders.");
			} else {
				gridSystem.GetComponent<GridSystem>().EnableGridCollider(false);
				EditorUtility.SetDirty(gridSystem.GetComponent<GridSystem>());
			}
		}
	}

	[MenuItem ("Grid/This is here to prevent accidentally clicking on clear grid.")]
	static void PlaceHolder0() {
		Debug.Log("Phew, that was close. Did you nearby click Clear Grid by accident?");
	}

	[MenuItem("Grid/Remove Grids Inside Terrain")]
	static void RemoveGridsInsideTerrain() {
		if (Selection.activeObject == null) {
			Debug.Log("No Grid Terrain Remover selected. Unable to remove grids inside terrain.");
		} else {
			GameObject gridTerrainRemover = (GameObject)Selection.activeObject;
			if (gridTerrainRemover.GetComponent<GridTerrainRemover>() == null) {
				Debug.Log("Selected GameObject has no Grid Terrain Remover. Unable to remove grids inside terrain.");
			} else {
				gridTerrainRemover.GetComponent<GridTerrainRemover>().RemoveGridsInsideTerrain();
				EditorUtility.SetDirty(gridTerrainRemover.GetComponent<GridTerrainRemover>().gridSystem);
			}
		}
	}

	[MenuItem ("Grid/This is here to prevent accidentally clicking on clear grid as well.")]
	static void PlaceHolder1() {
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

}