﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Building : MonoBehaviour
{
    public BuildingSO building = null;
    private Camera mainCamera = null;

    private int buildableUnitsCount = 0;

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

        //InitPos = building.buildingPrefab.GetComponentsInChildren<GameObject>()[0].transform;
        //DestPos = building.buildingPrefab.GetComponentsInChildren<GameObject>()[1].transform;
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
            StartCoroutine(CreateUnit());
        }
    }

    private void OnBuildingClick()
    {
        //Debug.Log("On building click");

        UI_Manager.instance.isWinOpened = true;
        UI_Manager.instance.buildingName.text = building.buildingPrefab.name;

        foreach (GameObject grid in UI_Manager.instance.buildingWindow_unitGrid)
            if (grid.activeInHierarchy)
                grid.SetActive(false);

        if (building.buildingType == Enums.BuildingType.UnitBuilder)
        {

            if (building.buildingPrefab.name == "TownHall")
            {
                UI_Manager.instance.buildingWindow_unitGrid[0].SetActive(true);
                UI_Manager.instance.buildingWindow_unitsNames[0].text = ((UnitBuilderSO)building).unitBuildable[0].ToString();
            }

            if (building.buildingPrefab.name == "Barrack")
            {
                UI_Manager.instance.buildingWindow_unitGrid[1].SetActive(true);
                UI_Manager.instance.buildingWindow_unitsNames[1].text = ((UnitBuilderSO)building).unitBuildable[1].ToString();
                UI_Manager.instance.buildingWindow_unitsNames[2].text = ((UnitBuilderSO)building).unitBuildable[2].ToString();
                UI_Manager.instance.buildingWindow_unitsNames[3].text = ((UnitBuilderSO)building).unitBuildable[3].ToString();
                UI_Manager.instance.buildingWindow_unitsNames[4].text = ((UnitBuilderSO)building).unitBuildable[4].ToString();
            }
        }
    }

    public IEnumerator CreateUnit()
    {
        curTimeBuilder += Time.deltaTime;

        UI_Manager.instance.FillCreationUnitBar(curTimeBuilder, debugTimeToBuildUnit);
        UI_Manager.instance.unitsInCreationCount = unitsInCreation.Count;


        if (unitsInCreation[unitsInCreation.Count - 1] == Enums.UnitName.Slave)
        {
            //Debug.Log("slave in creation...");


            if (curTimeBuilder > debugTimeToBuildUnit)
            {
                curTimeBuilder = 0;

                GameObject go = Instantiate(((UnitBuilderSO)building).slavePrefabs);
                go.transform.position = new Vector3(InitPos.transform.position.x, go.transform.position.y, InitPos.transform.position.z);
                go.gameObject.GetComponent<NavMeshAgent>().destination = DestPos.transform.position;
                //Debug.Log("slave created");

                unitsInCreation.RemoveAt(unitsInCreation.Count - 1);
            }
        }
        yield return 0;
    }
}
