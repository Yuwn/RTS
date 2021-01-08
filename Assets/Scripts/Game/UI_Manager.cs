using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    #region Resources
    [Header("Resources Window")]
    [SerializeField] private Text foodQuantityTxt = null;
    [SerializeField] private Text woodQuantityTxt = null;
    #endregion

    #region Buildings
    [Header("Building Window")]
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
    public Building previousActiveBuilding = null;
    #endregion


    private void Start()
    {
        instance = this;

        InitListeners();
    }

    // Update is called once per frame
    void Update()
    {
        ResourcesWindowUpdate();

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
            //Debug.Log(activeBuilding.name);

            if (previousActiveBuilding != activeBuilding)
            {
                previousActiveBuilding = activeBuilding;

                foreach (GameObject grid in buildingWindow_unitGrid)
                {
                    grid.SetActive(false);
                }
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

            //CREATION UNIT BAR - ACTIVE / UNACTIVE
            if (((UnitBuilderSO)activeBuilding.building).droppableUnits.Length > 0)
                buildingWindow_CreationUnitBar.SetActive(true);
            else
                buildingWindow_CreationUnitBar.SetActive(false);
        }
    }

    private void UnitCreationBar()
    {
        if (activeBuilding != null)
        {
            if (activeBuilding.unitsInCreationQueue.Count > 0)
            {
                unitsInCreationCount = activeBuilding.unitsInCreationQueue.Count; // UNIT COUNT                
                buildingWindow_CreationUnitBar_fill.fillAmount = activeBuilding.curTimeBuilder / activeBuilding.unitsInCreationQueue[0].makingTime; // FILL BAR
            }
            else
            {
                unitsInCreationCount = 0;
                buildingWindow_CreationUnitBar_fill.fillAmount = 0;
            }
        }
        else
        {
            buildingWindow_CreationUnitBar_fill.fillAmount = 0; // UNFILL BAR
            unitsInCreationCount = 0; // SET COUNT AT 0
        }
        buildingWindow_CreationUnitCount.text = unitsInCreationCount.ToString();
    }

    public void ClickOnUnitCreationButton(Enums.UnitName _unit)
    {
        //Debug.Log("1 : Clic on button :" + _unit.ToString());
        for (int i = 0; i < ((UnitBuilderSO)activeBuilding.building).droppableUnits.Length; i++)
        {
            if (((UnitBuilderSO)activeBuilding.building).droppableUnits[i].unitName == _unit)
            {
                //Debug.Log("2 : " + _unit.ToString() + " asked");
                UnitSO unitRequesting = Instantiate(((UnitBuilderSO)activeBuilding.building).droppableUnits[i]);
                activeBuilding.unitsInCreationRequest.Add(unitRequesting);
                //Debug.Log("2' : " + _unit.ToString() + " added in requesting queue");
                break;
            }
        }
    }

    private void InitListeners()
    {
        buildingWindow_unitsButtons[0].onClick.AddListener(() => ClickOnUnitCreationButton(Enums.UnitName.Slave));
        buildingWindow_unitsButtons[1].onClick.AddListener(() => ClickOnUnitCreationButton(Enums.UnitName.Soldier));
        buildingWindow_unitsButtons[2].onClick.AddListener(() => ClickOnUnitCreationButton(Enums.UnitName.Grenadier));
        buildingWindow_unitsButtons[3].onClick.AddListener(() => ClickOnUnitCreationButton(Enums.UnitName.Sniper));
        buildingWindow_unitsButtons[4].onClick.AddListener(() => ClickOnUnitCreationButton(Enums.UnitName.Gunner));
    }

    private void ResourcesWindowUpdate()
    {
        if (foodQuantityTxt.text != ResourcesManager.instance.FoodQuantity.ToString())
            foodQuantityTxt.text = ResourcesManager.instance.FoodQuantity.ToString();

        if (woodQuantityTxt.text != ResourcesManager.instance.WoodQuantity.ToString())
            woodQuantityTxt.text = ResourcesManager.instance.WoodQuantity.ToString();
    }
}
