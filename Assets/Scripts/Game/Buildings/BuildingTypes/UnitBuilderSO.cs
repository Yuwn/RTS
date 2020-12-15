using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Builder", menuName = "Building/Unit Builder")]

public class UnitBuilderSO : BuildingSO
{
    [Header("Child")]

    public Enums.Building_UnitBuilder buildingName = 0;

    [Header("Unit buildable")]
    public Enums.UnitType[] unitBuildable = null;

    private UnitBuilderSO()
    {
        buildingType = Enums.BuildingType.UnitBuilder;
    }
}
