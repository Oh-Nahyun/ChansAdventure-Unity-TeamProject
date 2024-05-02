using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : RecycleObject
{
    SwordSkeleton skeleton;
    NightmareDragon NightmareDragon;

    private void Awake()
    {
        skeleton = GetComponentInParent<SwordSkeleton>(true);   // 플레이어 찾기
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

    ///// <summary>
    ///// 무기 콜라이더 켜는 함수
    ///// </summary>
    //private void WeaponBladeEnable()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = true;
    //    }

    //    // onWeaponBladeEnabe 켜라고 신호보내기
    //    onWeaponBladeEnabe?.Invoke(true);
    //}

    ///// <summary>
    ///// 무기 콜라이더 끄는 함수
    ///// </summary>
    //private void WeaponBladeDisable()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = false;
    //    }

    //    // onWeaponBladeEnabe 끄라고 신호보내기
    //    onWeaponBladeEnabe?.Invoke(false);
    //}
}
