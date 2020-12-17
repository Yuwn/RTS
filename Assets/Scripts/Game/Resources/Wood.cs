﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private float maxHP = 50f;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamages(float damages)
    {
        hp -= damages;
    }
}
