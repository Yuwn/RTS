using UnityEngine;
using UnityEngine.UI;


public class BuildingSO : ScriptableObject
{
    [Header("Parent")]
    [HideInInspector] public Enums.BuildingType buildingType = 0;

    [Space]
    public GameObject BuildingPrefab = null;

    [Header("Stats")]
    public int healthMax = 0;

    [Header("UI")]
    [SerializeField] private GameObject ui_window = null;
    [SerializeField] private Text ui_buildingName = null;
}
