using UnityEngine;

public class Enums
{
    public enum BuildingType
    {
        None,
        UnitBuilder,
        Defenses,
        Resources,
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
        Farm, // to farm foods
        Sawmill // to seed wood near
    }

    public enum Building_Upgrader
    {
        None,
        House, // Increase pop max
        Stock, // Increase resources max
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

    public enum UnitName
    {
        None = 0,
        Slave,
        Soldier,
        Grenadier,
        Sniper,
        Gunner
    }
}
