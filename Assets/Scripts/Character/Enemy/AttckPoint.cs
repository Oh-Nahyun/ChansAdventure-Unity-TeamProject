using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon NightmareDragon;

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // 플레이어 찾기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BodyPoint"))
        {
            // 몸 공격
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                skeleton.Attack(target, false);

            }
        }
        else if (other.CompareTag("WeakPoint"))
        {
            // 약점 공격
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                skeleton.Attack(target, true);
            }
        }
    }
}
