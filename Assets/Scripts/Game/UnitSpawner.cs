using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{

    [SerializeField] private GameObject barrackPanel;
    [SerializeField] private GameObject slavePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void SpawnSlave()
    {
        Debug.Log("create slave");
        GameObject go = Instantiate(slavePrefabs);
        go.transform.position = Vector3.up - Vector3.forward * 5 + Vector3.right * 4 + new Vector3(Random.value, 0f, Random.value) ;
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
