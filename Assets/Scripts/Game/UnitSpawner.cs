using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        timeToCreateUnit = .01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }

        CreateUnitInQueue();
        CreationBarUpdate();
    }

    public void UnitToQueue()
    {
        if (!Input.GetKey(KeyCode.Delete))
        {
            AddUnitInQueue();
        }
        else
        {
            DelUnitFromQueue();
        }

        // error gestion
        if (unitInQueue < 0)
            unitInQueue = 0;

        if (unitInQueue == 0)
            if (unitCreation > 0)
                unitCreation = 0;
        // end error
    }

    public void AddUnitInQueue()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            unitInQueue += 5;
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            unitInQueue += 100;
        }
        else
        {
            unitInQueue++;
        }
    }

    private void DelUnitFromQueue()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            unitInQueue -= 5;
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            unitInQueue -= 100;
        }
        else
        {
            unitInQueue--;
        }
    }

    private void CreateUnitInQueue()
    {
        if (unitInQueue > 0)
        {
            unitCreation += Time.deltaTime;

            if (unitCreation >= timeToCreateUnit)
            {
                unitCreation = 0;
                unitInQueue--;

                Debug.Log("create slave");
                GameObject go = Instantiate(slavePrefabs);
                go.transform.position = unitSpawnPos.transform.position;
                //go.gameObject.GetComponent<Unit>().GetComponent<NavMeshAgent>().destination(unitDestPos.transform.position);
                //go.transform.position = Vector3.up - Vector3.forward * 5 + Vector3.right * 4 + new Vector3(Random.value, 0f, Random.value);
            }
        }
    }

    private void CreationBarUpdate()
    {
        //      unitUnQueueTxt.text = unitInQueue.ToString();
        //     createBar.fillAmount = (unitCreation * 100 / timeToCreateUnit) / 100;
    }

    public void OpenPanel()
    {
        //    barrackPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        //     barrackPanel.SetActive(false);
    }
}
