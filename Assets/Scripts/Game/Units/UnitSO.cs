using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit", order = 1)]

public class UnitSO : ScriptableObject
{
    public enum UnitType
    {
        None = 0,
        Slave,
        Soldier,
        Grenadier,
        Sniper,
        Gunner
    }

}
