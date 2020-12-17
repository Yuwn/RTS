using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    [Header("Object needed")]
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private List<Unit> selectedUnit = null;
    //[SerializeField] private List<NavMeshAgent> selectedUnit = null;

    [SerializeField] private Shader unselectedUnitMaterial = null;
    [SerializeField] private Shader selectedUnitMaterial = null;

    private Vector3 selectionBoxStart = Vector3.zero;
    [SerializeField] private RectTransform selectBoxRect = null;
    private Vector2 mousePosStart = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        selectedUnit = new List<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearUnitList();
            UI_Manager.instance.activeBuilding = null;
        }

        // WHEN LEFT CLIC
        if (Input.GetMouseButtonDown(0))
        {
            mousePosStart = Input.mousePosition;
            selectBoxRect.sizeDelta = Vector3.zero;
            selectBoxRect.gameObject.SetActive(true);

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                selectionBoxStart = new Vector3(hit.point.x, 0f, hit.point.z);
                //if (hit.collider.gameObject.CompareTag("Unit") == true)
                //{
                //    //Debug.Log(hit.collider.name);
                //    //hit.collider.GetComponent<MeshRenderer>().material.shader = selectedUnitMaterial;
                //    selectedUnit.Add(hit.collider.GetComponent<NavMeshAgent>());
                //}
            }
        }

        // DURING LEFT CLIC
        if (Input.GetMouseButton(0))
        {
            // resize select box
            selectBoxRect.position = mousePosStart + (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePosStart) / 2f;
            selectBoxRect.sizeDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePosStart;
            selectBoxRect.sizeDelta = new Vector2(Mathf.Abs(selectBoxRect.sizeDelta.x), Mathf.Abs(selectBoxRect.sizeDelta.y));
        }

        // LEFT CLIC RELEASED
        if (Input.GetMouseButtonUp(0))
        {

            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.one, Mathf.Infinity, ~5);
            if (hit2D.collider == null)
            {
                Ray _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit _hit = new RaycastHit();

                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                {
                    if (_hit.collider.gameObject.CompareTag("TownHall")
                        || _hit.collider.gameObject.CompareTag("Barrack")
                        || _hit.collider.gameObject.CompareTag("Storage"))
                    {
                        UI_Manager.instance.activeBuilding = _hit.collider.gameObject.GetComponent<Building>();
                    }
                    else
                    {
                        UI_Manager.instance.activeBuilding = null;
                    }
                }
                ClearUnitList();
            }
            else
            {
                //Debug.Log(hit2D.collider.name);
            }


            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!hit.collider.gameObject.CompareTag("TownHall")
                    && !hit.collider.gameObject.CompareTag("Barrack")
                    && !hit.collider.gameObject.CompareTag("Storage"))
                {
                    Collider[] colliders = Physics.OverlapBox(selectionBoxStart + new Vector3((hit.point - selectionBoxStart).x / 2f, 1.5f, (hit.point - selectionBoxStart).z / 2f),
                        new Vector3(Mathf.Abs((hit.point - selectionBoxStart).x) / 2f, 1.5f, Mathf.Abs((hit.point - selectionBoxStart).z / 2f)));
                    // check every unit in the selection box
                    foreach (Collider col in colliders)
                    {
                        if (col.gameObject.CompareTag("Unit") == true)
                        {
                            // do not select an unit a second time
                            if (!col.GetComponent<Unit>().isSelected)
                            {
                                // apply a selected material
                                col.GetComponent<MeshRenderer>().material.shader = selectedUnitMaterial;
                                // unit is selected
                                col.GetComponent<Unit>().isSelected = true;
                                // add unit to the select unit list
                                selectedUnit.Add(col.GetComponent<Unit>());
                            }
                        }
                    }
                }
            }
            selectBoxRect.gameObject.SetActive(false);
        }

        ClearDeadUnits();

        if (Input.GetMouseButtonDown(1))
            GiveOrderToUnits();
    }

    private void GiveOrderToUnits()
    {
        //Debug.Log("clic right mouse");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        //int layer = LayerMask.NameToLayer("Enemy");
        //int raycastLayer = ~(1 << layer);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // collider == ground
            if (selectedUnit != null && selectedUnit.Count > 0)
            {
                if (hit.collider.CompareTag("Ground") == true)
                {
                    foreach (Unit unit in selectedUnit)
                    {
                        unit.SetDestination(hit.point);
                    }
                }
                else if (hit.collider.CompareTag("Tree") == true)
                {
                    foreach (Unit unit in selectedUnit)
                    {
                        unit.target = hit.collider.gameObject;
                        unit.ChangeState(Unit.UnitState.GoTo_Tree);
                    }
                }
               else if (hit.collider.CompareTag("Food") == true)
                {
                    foreach (Unit unit in selectedUnit)
                    {
                        unit.target = hit.collider.gameObject;
                        unit.ChangeState(Unit.UnitState.GoTo_Bush);
                    }
                }
            }
            else if (UI_Manager.instance.activeBuilding != null)
            {
                if (hit.collider.gameObject.CompareTag("Ground") == true)
                {
                    if (UI_Manager.instance.activeBuilding.building.buildingType == Enums.BuildingType.UnitBuilder)
                    {
                        Debug.Log("change dest pos");
                        //UI_Manager.instance.activeBuilding.DestPos.position = new Vector3(hit.collider.transform.position.x, UI_Manager.instance.activeBuilding.DestPos.position.y, hit.collider.transform.position.z);
                    }
                }
            }
            // collider == building
            // collider == enemy
            // collider == ressources
        }
    }

    private void ClearUnitList()
    {
        foreach (Unit unit in selectedUnit)
        {
            unit.GetComponent<MeshRenderer>().material.shader = unselectedUnitMaterial;
            unit.GetComponent<Unit>().isSelected = false;
            unit.leaderToAgent = Vector3.zero;
            unit.leader = null;
        }
        selectedUnit.Clear();
    }

    private void ClearDeadUnits()
    {
        if (selectedUnit != null)
        {
            foreach (Unit unit in selectedUnit)
            {
                if (unit == null)
                {
                    selectedUnit.Remove(unit);
                }
            }
        }
    }

}
