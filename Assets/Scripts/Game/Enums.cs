using UnityEngine;

public class Enums
{
    public enum BuildingType
    {
        None,
        UnitBuilder,
        Defenses,
        Farm,
        Upgrader
    }

    public enum Building_UnitBuilder
    {
        None,
        TownHall, // To build slaves
        Barrack // build soldiers
        //Hospital // Heal units near
    }
    public enum Building_Farm
    {
        None,
        Storage, // Increase resources max, storage
        Farm, // to farm foods
        Sawmill // to seed wood near
    }

    public enum Building_Upgrader
    {
        None,
        House, // Increase pop max
        Blacksmith // To upgrade units
    }

    public enum Building_Defenses
    {
        None,
        Wall, // to protect buildings
        Door, // opener in walls
        Tower, // Attack enemies units near
        Mine // Explose in area when enemies units collide
    }

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
