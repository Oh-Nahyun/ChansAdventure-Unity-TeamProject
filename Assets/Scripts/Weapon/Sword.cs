using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /// <summary>
    /// 검의 Collider
    /// </summary>
    Collider swordCollider;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        // 적에게 검을 맞췄을 경우
    //    }
    //}

    /// <summary>
    /// 검의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderEnable()
    {
        swordCollider.enabled = true;
    }

    /// <summary>
    /// 검의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderDisable()
    {
        swordCollider.enabled = false;
    }

    /// <summary>
    /// 검을 꺼내는 함수 (Animation 설정용)
    /// </summary>
    public void OpenSwordWeapon()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 검을 넣는 함수 (Animation 설정용)
    /// </summary>
    public void CloseSwordWeapon()
    {
        gameObject.SetActive(false);
    }
}
