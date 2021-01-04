using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit", order = 1)]

public class UnitSO : ScriptableObject
{
    [Header("Components")]
    public GameObject unitPrefab = null;

    [Header("Stats")]
    public Enums.UnitType unitType = 0;
    public int maxHealth = 0;
    public float maxMoveSpeed = 0;
    public float atkSpeed = 0;
    public int atkDamage = 0;

    [Header("Inventory")]
    //private List<Resources> inventory = null;
    public int maxCapacity = 0;

    [Header("Cost")]
    public int cost_food = 0;
    public int cost_wood = 0;

    [Header("Habilities")]
    public bool job_harvester = false;
    public bool job_builder = false;
    public bool job_fighter = false;
    public bool job_demolition = false;

    [Header("Spells")]
    public bool canUseSpells = false;
    public bool spell_aoe = false;
    public bool spell_monoCible = false;
    public bool spell_multiCible = false;
}
