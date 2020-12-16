using UnityEngine;
using UnityEngine.UI;


public class BuildingSO : ScriptableObject
{
    [Header("Parent")]
    [HideInInspector] public Enums.BuildingType buildingType = 0;

    [Space]
    public Building BuildingPrefab = null;

    [Header("Stats")]
    public int healthMax = 0;

    public void OnClick()
    {
        Debug.Log("on click");
    }
}
