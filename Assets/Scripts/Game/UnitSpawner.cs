using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{

    [SerializeField] private GameObject barrackPanel = null;
    [SerializeField] private GameObject slavePrefabs = null;

    [SerializeField] private GameObject unitSpawnPos = null;
    [SerializeField] private GameObject unitDestPos = null;

    private int unitInQueue = 0;
    private float createUnitCooldown = 0;
    private float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        createUnitCooldown = .5f;
        cooldown = createUnitCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void SpawnSlave()
    {
        unitInQueue++;

        if (unitInQueue > 0)
        {
            if (cooldown >= createUnitCooldown)
            {
                Debug.Log("create slave");
                GameObject go = Instantiate(slavePrefabs);
                go.transform.position = unitSpawnPos.transform.position;
                //go.gameObject.GetComponent<Unit>().GetComponent<NavMeshAgent>().destination(unitDestPos.transform.position);
                //go.transform.position = Vector3.up - Vector3.forward * 5 + Vector3.right * 4 + new Vector3(Random.value, 0f, Random.value);
                cooldown = 0;
                unitInQueue--;
            }
        }
    }

    public void OpenPanel()
    {
        barrackPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        barrackPanel.SetActive(false);
    }
}
