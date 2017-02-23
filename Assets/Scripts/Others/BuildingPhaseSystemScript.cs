using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingPhaseSystemScript : MonoBehaviour {

    public SelectedGridScript selectingGrid;        // The grid that does the selection for the building phase
    public GameObject[] towerPrefabs;               // The towers that players can choose to plant

    public int amountToBuildTowers { get; set; }
    public int numberOfBuildableWalls { get; set;}

    // Buttons to check
    public Button sellTurretButton;
    public Button sellWallButton;
    public Button upgradeButton;
    public Button nextWaveButton;

    public Text wall;
    public Text currency;

	// Use this for initialization
    void Start()
    {
        amountToBuildTowers = 10000;
        numberOfBuildableWalls = 10;
        if (selectingGrid == null)
        {
            return;
        }
        if (towerPrefabs.Length <= 0)
        {
            return;
        }
        selectingGrid.selectedPrefab = towerPrefabs[0];
        UpdateText();

        if (sellTurretButton ||
            sellWallButton     ||
            upgradeButton      ||
            nextWaveButton)
        {
            print("No button");
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SellTurret()
    {
        amountToBuildTowers += selectingGrid.DestroySelectedTurret();
        UpdateText();
    }

    public void SellWall()
    {
        amountToBuildTowers += selectingGrid.DestroySelectedWall();
        numberOfBuildableWalls += 1;
        UpdateText();
    }

    public void UpgradeTurret()
    {
        amountToBuildTowers -= selectingGrid.UpgradeSelectedTurret();
        UpdateText();
    }

    public void NextWaveButton()
    {

    }

    public void SelectedTowerButton()
    {
        sellTurretButton.gameObject.SetActive(true);
        sellWallButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
    }

    public void SelectedWallButton()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(false);
    }

    public void SelectedNothingButton()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
    }

    public void DisableAllButtons()
    {
        sellTurretButton.gameObject.SetActive(false);
        sellWallButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        nextWaveButton.gameObject.SetActive(false);
    }

    public void UpdateText()
    {
        wall.text = numberOfBuildableWalls.ToString();
        currency.text = amountToBuildTowers.ToString();
    }
}
