using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerSelectPanelScript : MonoBehaviour {
    // Our dearest building phase system
    public BuildingPhaseSystemScript buildingPhaseSystem;
    // Our dearest prefab for tower selection
    public GameObject towerSelectPrefab;
    // Content of scrollRect
    public GameObject content;
    // The group of toggles
    public ToggleGroup towerToggleGroup;
    // To store the tower prefabs from buildiPhaseSystem
    private GameObject[] towerPrefabs;

	// Use this for initialization
	void Start () {
        if (buildingPhaseSystem == null) 
        { 
            print("No building system"); return; 
        }
        if(!towerSelectPrefab)
        {
            print("No tower selected prefab"); return;
        }
        if (!towerToggleGroup)
        {
            print("No tower toggle group"); return;
        }
        // Store it into a gameobject array in this class
        towerPrefabs = buildingPhaseSystem.towerPrefabs;
        if(towerPrefabs.Length <= 0)
        {
            print("No towers;"); return;
        }

        float yPos = 0;
        float prefabScaleHeight = 0;
        for (int i = 0; i < towerPrefabs.Length; ++i)
        {
            GameObject towerGO = GameObject.Instantiate(towerSelectPrefab);
            TowerPrefabScript towerGOScript = towerGO.GetComponent<TowerPrefabScript>();
            towerGOScript.towerName = towerPrefabs[i].name;
            towerGOScript.towerCost = 1000 + i;
            towerGOScript.towerPrefab = towerPrefabs[i];
            towerGOScript.buildingPhaseSystem = buildingPhaseSystem;

            Transform parentTransform = gameObject.transform.parent;

            towerGO.GetComponent<Toggle>().group = towerToggleGroup;
            towerGO.transform.SetParent(content.transform);
            
            yPos -= towerGO.GetComponent<RectTransform>().rect.height / 2;

            towerGO.transform.localScale = content.transform.localScale;
            towerGO.transform.localPosition = new Vector3(0, yPos, 0) ;
            
            yPos -= towerGO.GetComponent<RectTransform>().rect.height / 2;
            if(prefabScaleHeight < towerGO.GetComponent<RectTransform>().rect.height)
                prefabScaleHeight = towerGO.GetComponent<RectTransform>().rect.height;
                
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width, prefabScaleHeight * towerPrefabs.Length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
