using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemies : MonoBehaviour
{
    [Header("Objects Needed")]
    [SerializeField] private NavMeshAgent navMesh = null;

    [Header("Caracteristiques")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private bool canDestroyBuilding = true;
    public float health = 0;
    private float timeBetween2Attacks = 2;
    private float cooldownAttack = 0;

    [Header("Detection")]
    [SerializeField] private float detectionDist = 0;
    private bool isChasing = false;
    private bool isSearching = false;
    private GameObject go = null;
    private Collider[] hitColliders = null;

    [Header("UI")]
    [SerializeField] private Canvas UI = null;
    [SerializeField] private Image healthImg = null;
    private Transform camTransform = null;

    private enum EnemyState
    {
        Idle,
        Wanderer,
        Chasing,
        Attacking
    }

    private EnemyState state = EnemyState.Idle;
    private EnemyState nextState = 0;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = FindObjectOfType<Camera>().transform;

        health = maxHealth;

        nextState = state;
    }

    // Update is called once per frame
    void Update()
    {
        //ActionBehaviour();

        Detection();

        cooldownAttack += Time.deltaTime;

        HealthUI();
        Death();
    }

    private void ActionBehaviour()
    {
        switch (state)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Wanderer:
                break;
            case EnemyState.Chasing:
                break;
            case EnemyState.Attacking:
                break;
            default:
                break;
        }
    }


    private void Detection()
    {
        if (!isChasing)
        {
            navMesh.SetDestination(transform.position);

            hitColliders = Physics.OverlapSphere(transform.position, detectionDist);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Unit" || (hitCollider.gameObject.tag == "Barrack" && canDestroyBuilding))
                {
                    go = hitCollider.gameObject;
                    isChasing = true;
                    break;
                }
            }
        }

        if (isChasing)
        {
            Chasing();
        }
    }

    private void Chasing()
    {
        if (go != null)
        {
            if (ClosestTarget() != null)
            {
                go = ClosestTarget();
            }

            navMesh.SetDestination(go.transform.position);

            if (Vector3.Distance(transform.position, go.transform.position) <= 1.5f)
            {
                // attack
                if (cooldownAttack > timeBetween2Attacks)
                {
                    cooldownAttack = 0;
                    if (go.gameObject.tag == "Unit")
                    {
                        go.GetComponent<Unit>().health -= 20;
                    }
                    else if (go.gameObject.tag == "Barrack" && canDestroyBuilding)
                    {
                        //go.GetComponent<Building>().health -= 5;
                    }
                }
            }
            else if (Vector3.Distance(transform.position, go.transform.position) >= detectionDist)
            {
                isChasing = false;
                navMesh.destination = transform.position;
            }
        }
        else if (go.gameObject == null)
        {
            isChasing = false;
            navMesh.destination = transform.position;
        }
    }

    private GameObject ClosestTarget()
    {
        hitColliders = Physics.OverlapSphere(transform.position, detectionDist);
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (go.gameObject != hitCollider.gameObject)
                {
                    float distanceFromCurTarget = Vector3.Distance(transform.position, go.transform.position);
                    float distanceFromCollider = Vector3.Distance(transform.position, hitCollider.transform.position);

                    if (distanceFromCurTarget > distanceFromCollider)
                    {
                        return hitCollider.gameObject;
                    }
                }
            }
        }
        return null;
    }

    private void HealthUI()
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



}
