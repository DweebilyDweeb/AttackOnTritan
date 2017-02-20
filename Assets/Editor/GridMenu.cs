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

}