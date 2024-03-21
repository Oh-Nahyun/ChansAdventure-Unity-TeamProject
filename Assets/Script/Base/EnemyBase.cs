using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHP = 100.0f;

    float currentHP = 0;
    public float CurrentHP
    {
        get => currentHP;
        set
        {
            if(currentHP != value)
            {
                currentHP = value;
            }
        }
    }

    public float damage = 20.0f;
}
