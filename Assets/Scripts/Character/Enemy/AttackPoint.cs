using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    SwordSkeleton skeleton;
    NightmareDragon nightmareDragon;

    BoxCollider attackCollider;
    

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>();   // 플레이어 찾기
        nightmareDragon = GetComponentInParent<NightmareDragon>();
        attackCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// 공격 충돌영역을 켜고 끄는 함수
    /// </summary>
    /// <param name="isEnable"></param>
    public void BladeVolumeEnable(bool isEnable)
    {
        attackCollider.enabled = isEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackPoint"))
        {
            // 몸 공격
            IBattler target = other.GetComponent<IBattler>();
            if (target != null)
            {
                if(nightmareDragon != null)
                {
                    target.Defence(nightmareDragon.AttackPower);
                }
                else
                {
                    skeleton.Attack(target, false);        
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
