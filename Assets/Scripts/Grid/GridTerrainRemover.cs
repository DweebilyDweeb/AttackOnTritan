using UnityEngine;
using System.Collections;

public class GridTerrainRemover : MonoBehaviour {

    public Terrain terrain;
    public GridSystem gridSystem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void RemoveGridsInsideTerrain() {
		if (terrain == null || gridSystem == null) {
			return;
		}
		gridSystem.RemoveGridsInsideTerrain(terrain);
    }

}
