using UnityEngine;
using System.Collections;

public class TurretTowerScript : MonoBehaviour {

    public int tileID;
    public GridSystem gridSystem;

	// Use this for initialization
	void Start () {
        GameObject grid = gridSystem.GetGrid(tileID);
        if (grid != null)
        {
            transform.position = grid.GetComponent<Transform>().position;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
