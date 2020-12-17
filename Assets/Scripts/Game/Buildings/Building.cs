using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingSO building = null;
    private Camera mainCamera = null;

    private int buildableUnitsCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        buildableUnitsCount = GetComponent<UnitBuilderSO>().unitBuildable.Length;
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
    }

    private void OnBuildingClick()
    {
        Debug.Log("On building click");

        UI_Manager.instance.isWinOpened = true;
        UI_Manager.instance.buildingName.text = building.BuildingPrefab.name;

        if (building.buildingType == Enums.BuildingType.UnitBuilder)
        {
            UI_Manager.instance.buildingWindow_unitsNames[0].GetComponent<Text>().text = ((UnitBuilderSO)building).unitBuildable[0].ToString();

            if (building.BuildingPrefab.name == "TownHall")
            {
                foreach (GameObject grid in UI_Manager.instance.buildingWindow_unitGrid)
                    if (grid.activeInHierarchy)
                        grid.SetActive(false);
                UI_Manager.instance.buildingWindow_unitGrid[0].SetActive(true);
            }
        }

    }
}
