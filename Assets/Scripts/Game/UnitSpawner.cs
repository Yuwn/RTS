using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{
    [Header("Object needed")]

    [SerializeField] private GameObject slavePrefabs = null;

    [Header("Spawn Pos")]
    [SerializeField] private GameObject unitSpawnPos = null;
    [SerializeField] private GameObject unitDestPos = null;

    private int unitInQueue = 0;
    private float timeToCreateUnit = 0;
    private float unitCreation = 0;

    Building activeBuilding = null;
    Enums.UnitName unitToBuild = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activeBuilding != null && unitToBuild != Enums.UnitName.None)
        {
            StartCoroutine(CreateUnit(activeBuilding, unitToBuild));
            activeBuilding = null;
            unitToBuild = Enums.UnitName.None;
        }
    }

    public IEnumerator CreateUnit(Building _activeBuilding, Enums.UnitName _unitName)
    {
        Building tmpActiveBuilding = _activeBuilding;
        Enums.UnitName tmpUnitName = _unitName;

        unitInQueue++;
        Debug.Log("add slave in creation...");
        yield return new WaitForSeconds(2f);
        GameObject go = Instantiate(slavePrefabs);
        go.transform.position = unitSpawnPos.transform.position;
        go.gameObject.GetComponent<Unit>().GetComponent<NavMeshAgent>().destination = tmpActiveBuilding.DestPos.transform.position;
        Debug.Log("slave created");
        //go.transform.position = Vector3.up - Vector3.forward * 5 + Vector3.right * 4 + new Vector3(Random.value, 0f, Random.value);

        yield return 0;
    }

    public void AddUnitToQueue(Building _activeBuilding, Enums.UnitName _unitName)
    {
        activeBuilding = _activeBuilding;
        unitToBuild = _unitName;
    }
}
