using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
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


    [HideInInspector] public int health = 0;
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public type curType = type.Slave;

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
    private UnitState nextState = 0;


    // Start is called before the first frame update
    void Start()
    {
        // UI
        camTransform = FindObjectOfType<Camera>().transform;
        // STATS
        health = maxHealth;
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
        }
        if (nextState != UnitState.Null)
            state = nextState;
    }

    private void healthUI()
    {
        //Debug.Log(health);

        healthImg.fillAmount = health / (float)maxHealth;

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
        Debug.Log("IdleEnter");
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
        Debug.Log("IdleExit");
    }
    #endregion

    #region GoToTree
    private void OnGoToTreeEnter()
    {
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
        Debug.Log("Leaving tree");
    }
    #endregion

    #region GoToFood
    private void OnGoToFoodEnter()
    {
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
        Debug.Log("Leaving berries");
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
            target.GetComponent<Wood>().TakeDamages(25f * Time.deltaTime);
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

    }
    #endregion


}
