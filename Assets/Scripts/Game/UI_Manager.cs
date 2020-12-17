using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // CE QUE J'ETAIS ENTRAIN DE FAIRE
    /*
     * La liaison entre le batiment et l'ui
     */
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

    public Building activeBuilding = null;

    [HideInInspector] public bool isWinOpened = false;


    private int creationUnitCount = 0;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        WindowGateKeeper();


        // CREATION UNIT BAR
        if (buildingWindow_CreationUnitCount != null)
        {
            if (creationUnitCount > 0)
            {
                buildingWindow_CreationUnitBar.SetActive(true);
            }
            else
            {
                buildingWindow_CreationUnitBar.SetActive(false);
            }
        }
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
            if (activeBuilding.building.BuildingPrefab.name == "TownHall")
            {

            }
            else if (activeBuilding.building.BuildingPrefab.name == "Barrack")
            {

            }
        }
    }

    public void FillCreationUnitBar(float _curTimer, float _max)
    {
        buildingWindow_CreationUnitBar_fill.fillAmount = _curTimer / _max;
    }

    public void Set_CreationUnitCount(int _count)
    {
        buildingWindow_CreationUnitCount.text = _count.ToString();
        creationUnitCount = _count;
    }

    private void OnUnitButtonClick()
    {
        if (isWinOpened)
        {
            if (buildingWindow_unitGrid[0].activeSelf)
            {
                buildingWindow_unitsButtons[0].onClick.AddListener(() => UnitCreation(Enums.UnitName.Slave));
            }
            else if (buildingWindow_unitGrid[1].activeSelf)
            {
                buildingWindow_unitsButtons[1].onClick.AddListener(() => UnitCreation(Enums.UnitName.Soldier));
                buildingWindow_unitsButtons[2].onClick.AddListener(() => UnitCreation(Enums.UnitName.Grenadier));
                buildingWindow_unitsButtons[3].onClick.AddListener(() => UnitCreation(Enums.UnitName.Sniper));
                buildingWindow_unitsButtons[4].onClick.AddListener(() => UnitCreation(Enums.UnitName.Gunner));
            }

        }
    }

    public void UnitCreation(Enums.UnitName _unit)
    {
        // TownHall
        if (_unit == Enums.UnitName.Slave)
        {

        }
    }
}
