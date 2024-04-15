using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareDragon : EnemyBase
{
    public Waypoints waypoints;

    public Action<float> onDamage;

    readonly int die_Hash = Animator.StringToHash("Die");
    readonly int damage_Hash = Animator.StringToHash("Damage");

    Animator animator;

    Collider[] colliders;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if(CurrentHealth <= 0)
        {
            // �״� �ִϸ��̼�
            animator.SetTrigger(die_Hash);
            colliders = GetComponentsInChildren<Collider>();
        }
        else
        {
            // �´� �ִϸ��̼�
            animator.SetTrigger(damage_Hash);
        }
    }

    public void Test(float damage)
    {
        if(Input.GetButton("Space"))
        {
            TakeDamage(10.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onDamage?.Invoke(damage);
        }
    }
}
