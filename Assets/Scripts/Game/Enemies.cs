using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    [Header("Objects Needed")]
    [SerializeField] private NavMeshAgent navMesh = null;

    [Header("Caracteristiques")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private bool canDestroyBuilding = true;
    private float health = 0;
    private float timeBetween2Attacks = 2;
    private float cooldownAttack = 0;

    [Header("Detection")]
    [SerializeField] private float detectionDist = 0;
    private bool isChasing = false;
    private bool isSearching = false;
    private GameObject go = null;
    private Collider[] hitColliders = null;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Detection();

        cooldownAttack += Time.deltaTime;
    }

    private void Detection()
    {
        if (!isChasing)
        {
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
            navMesh.SetDestination(go.transform.position);

            if (Vector3.Distance(transform.position, go.transform.position) <= 1.5f)
            {
                // attack
                if (cooldownAttack > timeBetween2Attacks)
                {
                    cooldownAttack = 0;
                    if (go.gameObject.tag == "Unit")
                    {
                        go.GetComponent<Unit>().health -= 50;
                    }
                    else if (go.gameObject.tag == "Barrack" && canDestroyBuilding)
                    {
                        go.GetComponent<Building>().health -= 5;
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
}
