using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Vector2 minMaxZoom;
    [SerializeField] float sprintFactor;
    private float currentSprint;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Shader baseUnitMaterial;
    [SerializeField] private Shader selectedUnitMaterial;
    [SerializeField]
    private List<NavMeshAgent> selectedUnit;

    private Vector3 defaultPosition;
    private Vector3 selectionBoxStart;
    [SerializeField] private RectTransform selectBoxRect;
    private Vector2 mousePosStart;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
        selectedUnit = new List<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearUnitList();
        }

        // when clic left
        if (Input.GetMouseButtonDown(0))
        {
            mousePosStart = Input.mousePosition;
            selectBoxRect.sizeDelta = Vector3.zero;
            selectBoxRect.gameObject.SetActive(true);

            ClearUnitList();

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

        // during left clic
        if (Input.GetMouseButton(0))
        {
            selectBoxRect.position = mousePosStart + (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePosStart) / 2f;
            selectBoxRect.sizeDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePosStart;
            selectBoxRect.sizeDelta = new Vector2(Mathf.Abs(selectBoxRect.sizeDelta.x), Mathf.Abs(selectBoxRect.sizeDelta.y));
        }

        // left clic released
        if (Input.GetMouseButtonUp(0))
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            selectBoxRect.gameObject.SetActive(false);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Collider[] colliders = Physics.OverlapBox(selectionBoxStart + new Vector3((hit.point - selectionBoxStart).x / 2f, 1.5f, (hit.point - selectionBoxStart).z / 2f),
                    new Vector3(Mathf.Abs((hit.point - selectionBoxStart).x) / 2f, 1.5f, Mathf.Abs((hit.point - selectionBoxStart).z / 2f)));
                foreach (Collider col in colliders)
                {
                    if (col.gameObject.CompareTag("Unit") == true)
                    { 
                        col.GetComponent<MeshRenderer>().material.shader = selectedUnitMaterial;
                        selectedUnit.Add(col.GetComponent<NavMeshAgent>());
                    }
                }
            }
        }

        if (selectedUnit.Count > 0)
            if (Input.GetMouseButtonDown(1))
                GiveOrderToUnits();

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;

        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    Gizmos.DrawCube(selectionBoxStart + new Vector3((hit.point - selectionBoxStart).x / 2f, 1.5f, (hit.point - selectionBoxStart).z / 2f),
        //            new Vector3(Mathf.Abs((hit.point - selectionBoxStart).x), 3f, Mathf.Abs((hit.point - selectionBoxStart).z / 2f)));
        //}
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            currentSprint = sprintFactor;
        }
        else
        {
            currentSprint = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            transform.position = defaultPosition;
        }


        // MOVEMENT
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * moveSpeed * currentSprint * Time.deltaTime;
        transform.Translate(movement, Space.World);
        Vector3 zoom = new Vector3(0f, 0f, Input.GetAxis("Mouse ScrollWheel"));


        if (transform.position.y > minMaxZoom.x && zoom.normalized.z < 0f)
        {
            transform.Translate(zoom, Space.Self);
        }
        else if (transform.position.y < minMaxZoom.y && zoom.normalized.z < 0f)
        {
            transform.Translate(zoom, Space.Self);
        }
    }

    private void ClearUnitList()
    {
        foreach (NavMeshAgent agent in selectedUnit)
        {
            agent.GetComponent<MeshRenderer>().material.shader = baseUnitMaterial;
        }
        selectedUnit.Clear();
    }

    private void GiveOrderToUnits()
    {
        //Debug.Log("clic right mouse");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
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
}