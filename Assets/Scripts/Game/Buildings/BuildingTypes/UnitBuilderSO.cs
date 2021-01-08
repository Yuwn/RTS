using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Builder", menuName = "Building/Unit Builder")]

public class UnitBuilderSO : BuildingSO
{
    [Header("Child")]

    public Enums.Building_UnitBuilder buildingName = 0;

    [Header("Unit buildable")]
    public Enums.UnitName[] unitBuildable = null;
    //public GameObject slavePrefabs = null;
    
    public UnitSO[] droppableUnits = null;


    private UnitBuilderSO()
    {
        buildingType = Enums.BuildingType.UnitBuilder;
    }

    public void OnClick()
    {
        base.OnClick();
    }
}
