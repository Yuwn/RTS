using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingSpawner : MonoBehaviour
{
    private Camera mainCamera = null;
    [SerializeField] private UnitBuilderSO[] droppableUnitBuilder = null;



    public void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CreateBuilding(Enums.Building_UnitBuilder.TownHall);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateBuilding(Enums.Building_UnitBuilder.Barrack);
        }
    }

    public void CreateBuilding(Enums.Building_UnitBuilder _unitBuilder)
    {
        //Debug.Log("Create Building");
        StartCoroutine(SpawnBuilding(_unitBuilder));
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
}
