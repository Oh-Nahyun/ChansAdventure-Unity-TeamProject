using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
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

    public void SwordColliderEnable()
    {
        swordCollider.enabled = true;
    }

    public void SwordColliderDisable()
    {
        swordCollider.enabled = false;
    }
}
