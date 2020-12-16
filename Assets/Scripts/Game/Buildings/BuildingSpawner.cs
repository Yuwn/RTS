using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateBuilding(Enums.Building_UnitBuilder.TownHall);
        }
    }

    public void CreateBuilding(Enums.Building_UnitBuilder _unitBuilder)
    {
        Debug.Log("Create Building");
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
                UnitBuilderSO dropBuilding = Instantiate(droppableUnitBuilder[Random.Range(0, droppableUnitBuilder.Length)]);

                GameObject go = Instantiate(dropBuilding.BuildingPrefab.gameObject);
                go.GetComponent<Building>().building = dropBuilding;
                dropBuilding.buildingType = Enums.BuildingType.UnitBuilder;
                go.transform.position = new Vector3(hit.point.x, go.transform.position.y, hit.point.z);
            }
        }

        yield return 0;
    }
}
