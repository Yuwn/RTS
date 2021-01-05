using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingSpawner : MonoBehaviour
{
    private Camera mainCamera = null;
    [SerializeField] private UnitBuilderSO[] droppableUnitBuilder = null;
    [SerializeField] private FarmSO[] droppableFarm = null;

    public void Start()
    {
        mainCamera = Camera.main;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SpawnBuilding(Enums.Building_UnitBuilder.TownHall));
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SpawnBuilding(Enums.Building_UnitBuilder.Barrack));
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(SpawnBuilding(Enums.Building_Farm.Storage));
        }
    }

    private IEnumerator SpawnBuilding(Enums.Building_UnitBuilder _unitBuilder)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Ground") == true)
            {
                if (_unitBuilder == Enums.Building_UnitBuilder.TownHall)
                {
                    UnitBuilderSO dropBuilding = Instantiate(droppableUnitBuilder[0]);
                    GameObject go = Instantiate(dropBuilding.buildingPrefab.gameObject);
                    go.GetComponent<Building>().building = dropBuilding;
                    dropBuilding.buildingType = Enums.BuildingType.UnitBuilder;
                    go.transform.position = new Vector3(hit.point.x, go.transform.position.y, hit.point.z);
                }
                else if (_unitBuilder == Enums.Building_UnitBuilder.Barrack)
                {
                    UnitBuilderSO dropBuilding = Instantiate(droppableUnitBuilder[1]);
                    GameObject go = Instantiate(dropBuilding.buildingPrefab.gameObject);
                    go.GetComponent<Building>().building = dropBuilding;
                    dropBuilding.buildingType = Enums.BuildingType.UnitBuilder;
                    go.transform.position = new Vector3(hit.point.x, go.transform.position.y, hit.point.z);
                }
            }
        }
        yield return 0;
    }

    private IEnumerator SpawnBuilding(Enums.Building_Farm _farm)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Ground") == true)
            {
                if (_farm == Enums.Building_Farm.Storage)
                {
                    Debug.Log("debug");
                    FarmSO dropBuilding = Instantiate(droppableFarm[0]);
                    GameObject go = Instantiate(dropBuilding.buildingPrefab.gameObject);
                    go.GetComponent<Building>().building = dropBuilding;
                    dropBuilding.buildingType = Enums.BuildingType.Farm;
                    go.transform.position = new Vector3(hit.point.x, go.transform.position.y, hit.point.z);
                }
            }
        }
        yield return 0;
    }
}
