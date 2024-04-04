using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IBattler
{
    public float HP { get; set; }

    float maxHP;
    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }

    bool isAlive = false;
    public bool IsAlive => isAlive;

    public Action onDie { get; set; }

    float attackPower;
    public float AttackPower => attackPower;

    float defencePower;

    public float DefencePower => defencePower;

    public Action<int> onHit { get; set; }

    public void Attack(IBattler target)
    {
        
    }

    public void Defence(float damage)
    {
        
    }

    public void Die()
    {
        
    }

    public void HealthRegenerate(float totalRegen, float duration)
    {
        
    }

    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        
    }


}
