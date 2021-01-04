using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    //public UnitSO unit = null;

    [Header("UI")]
    [SerializeField] private Canvas UI = null;
    [SerializeField] private Image healthImg = null;

    private Transform camTransform = null;

    public enum type
    {
        Slave,
        Soldier
    }

    public enum UnitState
    {
        Null,
        Idle,
        GoTo_Tree,
        Logging,
        GoTo_Bush,
        Harvesting,
        GoTo_Stockage,
        GoTo_Enemy,
        Fighting
    }

    public enum UnitLastJob
    {
        Null,
        Logging,
        Harvesting
    }


    public int health = 0;
    public bool isSelected = false;
    public type curType = type.Slave;

    public int unitCost = 0;

    private int maxStock = 20;
    private int foodStock = 0;
    private int woodStock = 0;

    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [HideInInspector] public Unit leader = null;
    [HideInInspector] public Vector3 leaderToAgent = Vector3.zero;
    [HideInInspector] public GameObject target = null;
    private GameObject nextTarget = null;
    private float speed = 0;
    private Vector3 dest;

    private UnitState state = UnitState.Idle;
    private UnitLastJob lastJob = UnitLastJob.Null;
    private UnitState nextState = 0;

    private GameObject go = null;
    private Collider[] hitColliders = null;
    private float detectionDist = 3f;

    private float timeBetween2Attacks = 2;
    private float cooldownAttack = 0;


    // Start is called before the first frame update
    void Start()
    {
        // UI
        camTransform = FindObjectOfType<Camera>().transform;
        // STATS
        health = 10;//unit.maxHealth;
        // JOB
        nextState = state;
        speed = navMeshAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        healthUI();

        ActionBehaviour();
        Death();

        Fight();
    }

    private void Fight()
    {
        cooldownAttack += Time.deltaTime;

        hitColliders = Physics.OverlapSphere(transform.position, detectionDist);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                go = hitCollider.gameObject;
                break;
            }
        }

        if (go != null)
        {
            if (Vector3.Distance(transform.position, go.transform.position) <= 2f)
            {
                // attack
                if (cooldownAttack > timeBetween2Attacks)
                {
                    cooldownAttack = 0;
                    if (go.gameObject.tag == "Enemy")
                    {
                        go.GetComponent<Enemies>().health -= 25;
                    }
                }
            }
        }
    }

    private void ActionBehaviour()
    {
        switch (state)
        {
            case UnitState.Idle:
                if (nextState == UnitState.Idle)
                {
                    OnIdleEnter();
                }
                OnIdleUpdate();
                if (nextState != UnitState.Null)
                {
                    OnIdleExit();
                }
                break;

            case UnitState.GoTo_Tree:
                if (nextState == UnitState.GoTo_Tree)
                {
                    OnGoToTreeEnter();
                }
                OnGoToTreeUpdate();
                if (nextState != UnitState.Null)
                {
                    OnGoToTreeExit();
                }
                break;

            case UnitState.Logging:
                if (nextState == UnitState.Logging)
                {
                    OnLoggingEnter();
                }
                OnLoggingUpdate();
                if (nextState != UnitState.Null)
                {
                    OnLoggingExit();
                }
                break;

            case UnitState.GoTo_Bush:
                if (nextState == UnitState.GoTo_Bush)
                {
                    OnGoToFoodEnter();
                }
                OnGoToFoodUpdate();
                if (nextState != UnitState.Null)
                {
                    OnGoToFoodExit();
                }
                break;

            case UnitState.Harvesting:
                if (nextState == UnitState.Harvesting)
                {
                    OnHarvestingEnter();
                }
                OnHarvestingUpdate();
                if (nextState != UnitState.Null)
                {
                    OnHarvestingExit();
                }
                break;

            case UnitState.GoTo_Stockage:
                if (nextState == UnitState.GoTo_Stockage)
                {
                    OnGoToBarrackEnter();
                }
                OnGoToBarrackUpdate();
                if (nextState != UnitState.Null)
                {
                    OnGoToBarrackExit();
                }
                break;

            case UnitState.GoTo_Enemy:
                break;

            case UnitState.Fighting:
                break;

            default:
                break;
        }
        if (nextState != UnitState.Null)
            state = nextState;
    }

    private void healthUI()
    {
        //Debug.Log(health);

        healthImg.fillAmount = health / (float)10f;
        //healthImg.fillAmount = health / (float)unit.maxHealth;

        UI.transform.LookAt(healthImg.transform.position + camTransform.forward);
    }

    private void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    private int IndexFromMask(int mask)
    {
        for (int i = 0; i < 32; ++i)
        {
            if ((1 << i) == mask)
                return i;
        }
        return -1;
    }

    public void ChangeState(UnitState _state)
    {
        nextState = _state;
    }

    public void SetDestination(Vector3 dest)
    {
        navMeshAgent.destination = dest;
    }


    #region Idle
    private void OnIdleEnter()
    {
        //Debug.Log("IdleEnter");
        nextState = UnitState.Null;
        target = null;
    }
    private void OnIdleUpdate()
    {
        if (leader != null)
        {
            navMeshAgent.destination = leader.transform.position + leaderToAgent;
        }
    }
    private void OnIdleExit()
    {
        //Debug.Log("IdleExit");
    }
    #endregion

    #region GoToTree
    private void OnGoToTreeEnter()
    {
        lastJob = UnitLastJob.Logging;

        //Debug.Log("Go to tree");
        NavMeshHit hit;
        NavMesh.SamplePosition(target.transform.position, out hit, 5f, -1);
        dest = hit.position;
        navMeshAgent.destination = dest;
        nextState = UnitState.Null;
    }
    private void OnGoToTreeUpdate()
    {
        if (target == null)
        {
            ChangeState(UnitState.GoTo_Stockage);
        }
        if (Vector3.Distance(dest, transform.position) < 1.5f)
        {
            ChangeState(UnitState.Logging);
        }
    }
    private void OnGoToTreeExit()
    {
        //Debug.Log("Leaving tree");
    }
    #endregion

    #region GoToFood
    private void OnGoToFoodEnter()
    {
        lastJob = UnitLastJob.Harvesting;

        //Debug.Log("Go to berries");
        NavMeshHit hit;
        NavMesh.SamplePosition(target.transform.position, out hit, 5f, -1);
        dest = hit.position;
        navMeshAgent.destination = dest;
        nextState = UnitState.Null;
    }
    private void OnGoToFoodUpdate()
    {
        if (target == null)
        {
            ChangeState(UnitState.GoTo_Stockage);
        }
        if (Vector3.Distance(dest, transform.position) < 1.5f)
        {
            ChangeState(UnitState.Harvesting);
        }
    }
    private void OnGoToFoodExit()
    {
        //Debug.Log("Leaving berries");
    }
    #endregion

    #region Logging
    private void OnLoggingEnter()
    {
        nextState = UnitState.Null;
        navMeshAgent.destination = transform.position;
    }
    private void OnLoggingUpdate()
    {
        if (target == null)
        {
            ChangeState(UnitState.GoTo_Stockage);
        }
        else
        {
            target.GetComponent<Wood>().TakeDamages(10f * Time.deltaTime);
        }
    }
    private void OnLoggingExit()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        if (trees[0] == null)
            return;

        nextTarget = trees[0];

        if (trees.Length <= 1)
            return;
        for (int i = 1; i < trees.Length; i++)
        {
            if (Vector3.Distance(transform.position, trees[i].transform.position) < Vector3.Distance(transform.position, nextTarget.transform.position))
            {
                nextTarget = trees[i];
            }
        }
        if (Vector3.Distance(nextTarget.transform.position, transform.position) > 25f)
        {
            nextTarget = null;
        }
    }
    #endregion

    #region Harvest
    private void OnHarvestingEnter()
    {
        nextState = UnitState.Null;
        navMeshAgent.destination = transform.position;
    }
    private void OnHarvestingUpdate()
    {
        if (target == null)
        {
            ChangeState(UnitState.GoTo_Stockage);
        }
        else
        {
            target.GetComponent<Food>().TakeDamages(25f * Time.deltaTime);
        }
    }
    private void OnHarvestingExit()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        if (foods[0] == null)
            return;

        nextTarget = foods[0];

        if (foods.Length <= 1)
            return;
        for (int i = 1; i < foods.Length; i++)
        {
            if (Vector3.Distance(transform.position, foods[i].transform.position) < Vector3.Distance(transform.position, nextTarget.transform.position))
            {
                nextTarget = foods[i];
            }
        }
        if (Vector3.Distance(nextTarget.transform.position, transform.position) > 25f)
        {
            nextTarget = null;
        }
    }
    #endregion

    #region GoToBarrack
    private void OnGoToBarrackEnter()
    {
        target = GameObject.FindGameObjectWithTag("Storage");
        NavMeshHit hit;
        NavMesh.SamplePosition(target.transform.position, out hit, 15f, -1);
        dest = hit.position;
        navMeshAgent.destination = dest;
        nextState = UnitState.Null;
    }
    private void OnGoToBarrackUpdate()
    {
        if (Vector3.Distance(dest, transform.position) < 1.5f)
        {
            if (nextTarget != null)
            {
                target = nextTarget;
                nextTarget = null;
                if (lastJob == UnitLastJob.Harvesting)
                    ChangeState(UnitState.GoTo_Bush);
                else if (lastJob == UnitLastJob.Logging)
                    ChangeState(UnitState.GoTo_Tree);
            }
            else
            {
                ChangeState(UnitState.Idle);
            }
        }
    }
    private void OnGoToBarrackExit()
    {
        if (lastJob == UnitLastJob.Harvesting)
        {
            ResourcesManager.instance.AddFood(40);
        }
        if (lastJob == UnitLastJob.Logging)
        {
            ResourcesManager.instance.AddWood(40);
        }
    }
    #endregion


}
