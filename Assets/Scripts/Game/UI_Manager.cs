using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    #region Buildings
    [SerializeField] private GameObject buildingWindow_bg = null;
    public Text buildingName = null;
    [Header("Buttons")]
    public GameObject[] buildingWindow_unitGrid = null; // 0=TownHall, 1=Barrack
    public Button[] buildingWindow_unitsButtons = null; // 0=Slave, 1=Soldier, 2=Grenadier
    public Text[] buildingWindow_unitsNames = null;
    public Text[] buildingWindow_unitsCost = null;
    [Header("Unit Creation Bar")]
    [SerializeField] private GameObject buildingWindow_CreationUnitBar = null;
    [SerializeField] private Image buildingWindow_CreationUnitBar_fill = null;
    [SerializeField] private Text buildingWindow_CreationUnitCount = null;
    public int unitsInCreationCount = 0;
    [HideInInspector] public bool isWinBuildingOpened = false;
    public Building activeBuilding = null;
    #endregion


    private void Start()
    {
        instance = this;

        InitListeners();
    }

    // Update is called once per frame
    void Update()
    {
        WindowGateKeeper();
        WindowOpener();

        UnitCreationBar();
    }

    private void WindowGateKeeper()
    {

        if (activeBuilding == null)
        {
            buildingWindow_bg.SetActive(false);
        }
        else
        {
            if (!buildingWindow_bg.activeSelf)
            {
                buildingWindow_bg.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                activeBuilding = null;
            }
        }
    }

    private void WindowOpener()
    {
        if (activeBuilding != null)
        {
            foreach (GameObject grid in buildingWindow_unitGrid)
            {
                grid.SetActive(false);
            }

            buildingName.text = activeBuilding.building.buildingPrefab.name;

            if (activeBuilding.building.buildingPrefab.name == "TownHall")
            {

                buildingWindow_unitGrid[0].SetActive(true);
            }
            else if (activeBuilding.building.buildingPrefab.name == "Barrack")
            {
                buildingWindow_unitGrid[1].SetActive(true);
            }
        }
    }



    private void UnitCreationBar()
    {
        if (activeBuilding != null)
        {
            // FILL BAR
            buildingWindow_CreationUnitBar_fill.fillAmount = activeBuilding.curTimeBuilder / activeBuilding.debugTimeToBuildUnit;
            // UNIT COUNT
            unitsInCreationCount = activeBuilding.unitsInCreation.Count;
        }
        else
        {
            // UNFILL BAR
            buildingWindow_CreationUnitBar_fill.fillAmount = 0;
            // SET COUNT AT 0
            unitsInCreationCount = 0;
        }
        buildingWindow_CreationUnitCount.text = unitsInCreationCount.ToString();

        // CREATION UNIT BAR - ACTIVE/UNACTIVE
        if (unitsInCreationCount > 0)
        {
            buildingWindow_CreationUnitBar.SetActive(true);
        }
        else
        {
            buildingWindow_CreationUnitBar.SetActive(false);
        }
    }

    public void UnitCreation(Enums.UnitName _unit)
    {
        //Debug.Log("Add unit to creation");
        activeBuilding.unitsInCreation.Add(_unit);
    }

    private void InitListeners()
    {
        buildingWindow_unitsButtons[0].onClick.AddListener(() => UnitCreation(Enums.UnitName.Slave));
        buildingWindow_unitsButtons[1].onClick.AddListener(() => UnitCreation(Enums.UnitName.Soldier));
        buildingWindow_unitsButtons[2].onClick.AddListener(() => UnitCreation(Enums.UnitName.Grenadier));
        buildingWindow_unitsButtons[3].onClick.AddListener(() => UnitCreation(Enums.UnitName.Sniper));
        buildingWindow_unitsButtons[4].onClick.AddListener(() => UnitCreation(Enums.UnitName.Gunner));
    }
}
