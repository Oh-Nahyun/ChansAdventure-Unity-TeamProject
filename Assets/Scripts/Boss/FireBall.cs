using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : RecycleObject
{
    Boss boss;
    float speed = 30.0f;

    private void Start()
    {
        GameObject bossObject = GameObject.FindWithTag("Player");
        boss = bossObject.GetComponent<Boss>();
    }

    private void FixedUpdate()
    {
        transform.Translate(speed * transform.forward * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IBattler target = other.GetComponentInParent<IBattler>();
            if (target != null)
            {
                boss.Attack(target, false);
            }
        }
    }
}
