using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    [SerializeField] private GameObject buildingWindow_bg = null;
    public Text buildingName = null;
    [Header("Buttons")]
    public GameObject[] buildingWindow_unitGrid = null; // 0=TownHall, 1=Barrack
    public Button[] buildingWindow_unitsButtons = null; // 0=Slave, 1=Soldier, 2=Grenadier
    public Text[] buildingWindow_unitsNames = null;
    public Text[] buildingWindow_unitsCost = null;
    [Header("Unit Creation Bar")]
    public GameObject buildingWindow_CreationUnitBar = null;
    private Image buildingWindow_CreationUnitBar_fill = null;
    private Text buildingWindow_CreationUnitCount = null;

    [HideInInspector] public bool isWinOpened = false;

    public Building activeBuilding = null;
    private int unitsInCreationCount = 0;

    private void Start()
    {
        instance = this;

        buildingWindow_unitsButtons[0].onClick.AddListener(() => UnitCreation(Enums.UnitName.Slave));
        buildingWindow_unitsButtons[1].onClick.AddListener(() => UnitCreation(Enums.UnitName.Soldier));
        buildingWindow_unitsButtons[2].onClick.AddListener(() => UnitCreation(Enums.UnitName.Grenadier));
        buildingWindow_unitsButtons[3].onClick.AddListener(() => UnitCreation(Enums.UnitName.Sniper));
        buildingWindow_unitsButtons[4].onClick.AddListener(() => UnitCreation(Enums.UnitName.Gunner));
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

    public void FillCreationUnitBar(float _curTimer, float _max)
    {
        buildingWindow_CreationUnitBar_fill.fillAmount = _curTimer / _max;
    }

    public void CreationUnitCount()
    {
        if (activeBuilding != null)
        {
            unitsInCreationCount = activeBuilding.unitsInCreation.Count;
        }
        else
        {
            unitsInCreationCount = 0;
        }
        buildingWindow_CreationUnitCount.text = unitsInCreationCount.ToString();
    }

    private void UnitCreationBar()
    {
        // CREATION UNIT BAR
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
        Debug.Log("Add unit to creation");
        activeBuilding.unitsInCreation.Add(_unit);
    }
}
