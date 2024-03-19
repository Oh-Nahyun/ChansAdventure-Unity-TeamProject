using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float HP = 100.0f;

    readonly int die_Hash = Animator.StringToHash("Die");
    readonly int damage_Hash = Animator.StringToHash("Damage");

    Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    public void TakeDamage(float damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            // 죽는 애니메이션
            animator.SetTrigger(die_Hash);
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            // 맞는 애니메이션
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
}
