using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector] public int health = 0;
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public type curType = type.Slave;

    // Start is called before the first frame update
    void Start()
    {
        // UI
        camTransform = FindObjectOfType<Camera>().transform;
        // STATS
        health = maxHealth;        
    }

    // Update is called once per frame
    void Update()
    {
        healthUI();

        Death();
    }

    private void healthUI()
    {
        Debug.Log(health);

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
