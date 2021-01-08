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

        CreationRequestSorting();
        if (unitsInCreationQueue != null && unitsInCreationQueue.Count > 0)
        {
            StartCoroutine(CreateUnitFromQueue());
        }
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
                    if (((UnitBuilderSO)building).droppableUnits[i].unitName == Enums.UnitName.Slave)
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
                    if (((UnitBuilderSO)building).droppableUnits[i].unitName == Enums.UnitName.Soldier)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[1].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // GRENADIER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitName == Enums.UnitName.Grenadier)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[2].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // SNIPER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitName == Enums.UnitName.Sniper)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[3].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                    // GUNNER
                    else if (((UnitBuilderSO)building).droppableUnits[i].unitName == Enums.UnitName.Gunner)
                    {
                        UI_Manager.instance.buildingWindow_unitsNames[4].text = ((UnitBuilderSO)building).unitBuildable[i].ToString();
                    }
                }
            }
        }
    }

    private void CreationRequestSorting()
    {
        if (unitsInCreationRequest.Count > 0)
        {

            //Debug.Log("3 : Creation request sorting...");
            if (ResourcesManager.instance.FoodQuantity >= unitsInCreationRequest[0].cost_food
                && ResourcesManager.instance.WoodQuantity >= unitsInCreationRequest[0].cost_wood)
            {
                //Debug.Log("4 : Consume resources to create unit");
                ResourcesManager.instance.UseFood(unitsInCreationRequest[0].cost_food);
                ResourcesManager.instance.UseWood(unitsInCreationRequest[0].cost_wood);
                //Debug.Log("5 : Unit added in creation queue");
                unitsInCreationQueue.Add(unitsInCreationRequest[0]);
            }
            unitsInCreationRequest.Remove(unitsInCreationRequest[0]);
            //Debug.Log("6 : Unit removed from creation request");

            //Debug.Log("Unit in request : " + unitsInCreationRequest.Count);

        }
    }

    public IEnumerator CreateUnitFromQueue()
    {
        curTimeBuilder += Time.deltaTime;

        //Debug.Log("7 : Unit in creation...");
        if (curTimeBuilder > unitsInCreationQueue[0].makingTime)
        {
            curTimeBuilder = 0;

            GameObject go = Instantiate(unitsInCreationQueue[0].unitPrefab.gameObject);
            unitsInCreationQueue.Remove(unitsInCreationQueue[0]);

            go.GetComponent<NavMeshAgent>().Warp(InitPos.position);

            //Debug.Log("8 : Unit created");
        }

        //Debug.Log("Unit in queue : " + unitsInCreationQueue.Count);
        yield return 0;
    }
}
