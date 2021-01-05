using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Building : MonoBehaviour
{
    public BuildingSO building = null;
    private Camera mainCamera = null;

    private int buildableUnitsCount = 0;

    public List<UnitSO> unitsInCreationRequest = null;
    public List<UnitSO> unitsInCreationQueue = null;
    public List<Enums.UnitName> unitsInCreation = null;

    public Transform InitPos = null;
    public Transform DestPos = null;

    public float debugTimeToBuildUnit = 2f;
    public float curTimeBuilder = 0;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        if (building.buildingType == Enums.BuildingType.UnitBuilder)
        {
            buildableUnitsCount = ((UnitBuilderSO)building).unitBuildable.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnBuildingClick();
                }
            }
        }

        if (unitsInCreation != null && unitsInCreation.Count > 0)
        {
            StartCoroutine(CreateUnitFromQueue());
            //StartCoroutine(CreateUnit());
        }
    }

    private void CreationRequestSorting()
    {
        Debug.Log("creation request sorting...");
        if (ResourcesManager.instance.FoodQuantity >= unitsInCreationRequest[unitsInCreationRequest.Count - 1].cost_food
            && ResourcesManager.instance.WoodQuantity >= unitsInCreationRequest[unitsInCreationRequest.Count - 1].cost_wood)
        {
            //Debug.Log("Consume resources to create unit");
            ResourcesManager.instance.UseFood(unitsInCreationRequest[unitsInCreationRequest.Count - 1].cost_food);
            ResourcesManager.instance.UseWood(unitsInCreationRequest[unitsInCreationRequest.Count - 1].cost_wood);
            //Debug.Log("Add unit to creation");
            unitsInCreationQueue.Add(unitsInCreationRequest[unitsInCreationRequest.Count - 1]);
        }
        unitsInCreation.RemoveAt(unitsInCreation.Count - 1);

    }

    private void OnBuildingClick()
    {
        //Debug.Log("On building click");

        if (building.buildingType == Enums.BuildingType.UnitBuilder)
        {
            if (building.buildingPrefab.name == "TownHall")
            {
                for (int i = 0; i < ((UnitBuilderSO)building).droppableUnits.Length; i++)
                {
                    // SLAVE
                    if (((UnitBuilderSO)building).droppableUnits[i].unitType == Enums.UnitName.Slave)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[0].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                        break;
                    }
                }
            }
            else if (building.buildingPrefab.name == "Barrack")
            {
                for (int i = 0; i < ((UnitBuilderSO)building).droppableUnits.Length; i++)
                {
                    // SOLDIER
                    if (((UnitBuilderSO)building).droppableUnits[i].unitType == Enums.UnitName.Soldier)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[1].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // GRENADIER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitType == Enums.UnitName.Grenadier)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[2].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // SNIPER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitType == Enums.UnitName.Sniper)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[3].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // GUNNER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitType == Enums.UnitName.Gunner)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[4].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                }
            }
        }
    }

    public IEnumerator CreateUnit()
    {
        curTimeBuilder += Time.deltaTime;

        Enums.UnitName unitType = unitsInCreation[unitsInCreation.Count - 1];

        if (unitsInCreation[unitsInCreation.Count - 1] == Enums.UnitName.Slave)
        {
            //Debug.Log("slave in creation...");

            if (curTimeBuilder > debugTimeToBuildUnit)
            {
                curTimeBuilder = 0;

                UnitSO dropUnit = new UnitSO();
                for (int i = 0; i < ((UnitBuilderSO)building).droppableUnits.Length; i++)
                {
                    if (((UnitBuilderSO)building).droppableUnits[i].unitType == unitType)
                    {
                        dropUnit = Instantiate(((UnitBuilderSO)building).droppableUnits[i]);
                        break;
                    }
                }
                GameObject go = Instantiate(dropUnit.unitPrefab.gameObject);
                go.GetComponent<Unit>().unit = dropUnit;
                dropUnit.unitType = unitType;
                go.transform.position = InitPos.position;
                go.GetComponent<NavMeshAgent>().destination = DestPos.position;

                //Debug.Log("unit created");

                unitsInCreation.RemoveAt(unitsInCreation.Count - 1);
            }
        }
        yield return 0;
    }

    public IEnumerator CreateUnitFromQueue()
    {
        curTimeBuilder += Time.deltaTime;


        //Debug.Log("slave in creation...");

        if (curTimeBuilder > debugTimeToBuildUnit)
        {
            curTimeBuilder = 0;

            UnitSO unitToCreate = unitsInCreationQueue[unitsInCreationQueue.Count - 1];
            unitsInCreationQueue.RemoveAt(unitsInCreation.Count - 1);

            GameObject go = Instantiate(unitToCreate.unitPrefab.gameObject);
            go.GetComponent<Unit>().unit = unitToCreate;
            go.transform.position = InitPos.position;
            go.GetComponent<NavMeshAgent>().destination = DestPos.position;

            //Debug.Log("unit created");

        }
        yield return 0;
    }
}
