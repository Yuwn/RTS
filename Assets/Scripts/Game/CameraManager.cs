using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;

    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float zoomSpeed = 0;
    [SerializeField] private Vector2 minMaxZoom = Vector2.zero;
    [SerializeField] float sprintFactor = 0;
    private float currentSprint = 0;
    private Vector3 defaultPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        // MOVEMENT
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * moveSpeed * currentSprint * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // MOVE MORE FAST
        if (Input.GetKey(KeyCode.RightShift))
        {
            currentSprint = sprintFactor;
        }
        else
        {
            currentSprint = 1f;
        }

        // ZOOM
        Vector3 zoom = new Vector3(0f, 0f, Input.GetAxis("Mouse ScrollWheel"));
        if (transform.position.y > minMaxZoom.x && zoom.normalized.z < 0f)
        {
            transform.Translate(zoom, Space.Self);
        }
        else if (transform.position.y < minMaxZoom.y && zoom.normalized.z < 0f)
        {
            transform.Translate(zoom, Space.Self);
        }

        // RESET POS CAM
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            transform.position = defaultPosition;
        }
    }
}