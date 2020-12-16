using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Selection : MonoBehaviour
{
    [Header("Object needed")]
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private List<NavMeshAgent> selectedUnit = null;

    [SerializeField] private Shader unselectedUnitMaterial = null;
    [SerializeField] private Shader selectedUnitMaterial = null;

    private Vector3 selectionBoxStart = Vector3.zero;
    [SerializeField] private RectTransform selectBoxRect = null;
    private Vector2 mousePosStart = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        selectedUnit = new List<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearUnitList();
        }

        // WHEN LEFT CLIC
        if (Input.GetMouseButtonDown(0))
        {
            mousePosStart = Input.mousePosition;
            selectBoxRect.sizeDelta = Vector3.zero;
            selectBoxRect.gameObject.SetActive(true);

            if (!Input.GetKey(KeyCode.RightShift))/* || !Input.GetKey(KeyCode.RightControl)*/
            {
                ClearUnitList();
            }

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

                if (hit.collider.gameObject.CompareTag("Barrack") == true)
                {
                    hit.collider.gameObject.GetComponent<UnitSpawner>().OpenPanel();
                }
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
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            selectBoxRect.gameObject.SetActive(false);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
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
                            selectedUnit.Add(col.GetComponent<NavMeshAgent>());
                        }
                    }
                }
            }
        }

        if (selectedUnit.Count > 0)
            if (Input.GetMouseButtonDown(1))
                GiveOrderToUnits();

        ClearDeadUnits();
    }

    private void ClearUnitList()
    {
        foreach (NavMeshAgent agent in selectedUnit)
        {
            agent.GetComponent<Unit>().isSelected = false;
            agent.GetComponent<MeshRenderer>().material.shader = unselectedUnitMaterial;
        }
        selectedUnit.Clear();
    }

    private void GiveOrderToUnits()
    {
        //Debug.Log("clic right mouse");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //int layer = LayerMask.NameToLayer("Enemy");
        //int raycastLayer = ~(1 << layer);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Ground") == true)
            {
                foreach (NavMeshAgent agent in selectedUnit)
                {
                    agent.destination = hit.point;
                }
            }
            // else if collider == building
            // else if collider == enemy
            // else if collider == ressources

        }
    }

    private void ClearDeadUnits()
    {
        if (selectedUnit != null)
        {
            foreach (NavMeshAgent unit in selectedUnit)
            {
                if (unit == null)
                {
                    selectedUnit.Remove(unit);
                }
            }
        }
    }

}
