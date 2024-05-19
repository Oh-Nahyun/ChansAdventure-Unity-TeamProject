using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    /// <summary>
    /// 활의 Collider
    /// </summary>
    Collider bowCollider;

    void OnEnable()
    {
        bowCollider = GetComponent<Collider>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        // 적에게 활을 맞췄을 경우
    //    }
    //}

    /// <summary>
    /// 활의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void BowColliderEnable()
    {
        bowCollider.enabled = true;
    }

    /// <summary>
    /// 활의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void BowColliderDisable()
    {
        bowCollider.enabled = false;
    }

    /// <summary>
    /// 활을 꺼내는 함수 (Animation 설정용)
    /// </summary>
    public void OpenBowWeapon()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 활을 넣는 함수 (Animation 설정용)
    /// </summary>
    public void CloseBowWeapon()
    {
        gameObject.SetActive(false);
    }
}
