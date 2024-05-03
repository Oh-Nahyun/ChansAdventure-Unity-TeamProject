using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon nightmareDragon;
    

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // 플레이어 찾기
        nightmareDragon = GetComponentInParent<NightmareDragon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackPoint"))
        {
            // 몸 공격
            Player target = GameManager.Instance.Player;
            if (target != null)
            {
                if(nightmareDragon != null)
                {
                    target.Defence(nightmareDragon.AttackPower);
                }
                else
                {
                    target.Defence(skeleton.AttackPower);        // 공격 대상에게 데미지 전달
                }
            }
        }
        //else if (other.CompareTag("Player"))
        //{
        //    // 약점 공격
        //    Player target = GameManager.Instance.Player;
        //    if (target != null)
        //    {
        //        target.Defence(skeleton.AttackPower);        // 공격 대상에게 데미지 전달
        //    }
        //}
    }

    
}
