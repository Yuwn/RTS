using UnityEngine;

[CreateAssetMenu(fileName = "New Farm Building", menuName = "Building/Farm")]

public class FarmSO : BuildingSO
{
    [Header("Child")]

    public Enums.Building_Farm buildingName = 0;

    [Header("Stats")]
    public int ShelterSize = 0;

    private FarmSO()
    {
        buildingType = Enums.BuildingType.Farm;
        //Debug.Log(buildingType);
    }

    public void OnClick()
    {
        base.OnClick();
    }
}
