using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // CE QUE J'ETAIS ENTRAIN DE FAIRE
    /*
     * La liaison entre le batiment et l'ui
     */
    public static UI_Manager instance;

    [SerializeField] private GameObject buildingWindow_bg = null;
    public Text buildingName = null;
    [Header("Buttons")]
    public GameObject[] buildingWindow_unitGrid = null; // 0=TownHall, 1=Barrack
    public Button[] buildingWindow_unitsButtons = null;
    public Text[] buildingWindow_unitsNames = null;
    public Text[] buildingWindow_unitsCost = null;
    [Header("Unit Creation Bar")]
    public GameObject buildingWindow_CreationUnitBar = null;
    private Image buildingWindow_CreationUnitBar_fill = null;
    private Text buildingWindow_CreationUnitCount = null;

    [HideInInspector] public bool isWinOpened = false;

    private int creationUnitCount = 0;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWinOpened)
            buildingWindow_bg.SetActive(true);
        else
            buildingWindow_bg.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isWinOpened = false;
        }

        // CREATION UNIT BAR
        if (buildingWindow_CreationUnitCount != null)
        {
            if (creationUnitCount > 0)
            {
                buildingWindow_CreationUnitBar.SetActive(true);
            }
            else
            {
                buildingWindow_CreationUnitBar.SetActive(false);
            }
        }
    }

    public void FillCreationUnitBar(float _curTimer, float _max)
    {
        buildingWindow_CreationUnitBar_fill.fillAmount = _curTimer / _max;
    }

    public void Set_CreationUnitCount(int _count)
    {
        buildingWindow_CreationUnitCount.text = _count.ToString();
        creationUnitCount = _count;
    }
}
