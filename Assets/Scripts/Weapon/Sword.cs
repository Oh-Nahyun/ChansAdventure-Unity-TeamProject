using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /// <summary>
    /// 칼의 Collider
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

    //    }
    //}

    /// <summary>
    /// 칼의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderEnable()
    {
        swordCollider.enabled = true;
    }

    /// <summary>
    /// 칼의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderDisable()
    {
        swordCollider.enabled = false;
    }

    /// <summary>
    /// 칼을 꺼내는 함수 (Animation 설정용)
    /// </summary>
    //public void OpenSwordWeapon()
    //{
    //    gameObject.SetActive(true);
    //}

    /// <summary>
    /// 칼을 넣는 함수 (Animation 설정용)
    /// </summary>
    //public void CloseSwordWeapon()
    //{
    //    gameObject.SetActive(false);
    //}
}
