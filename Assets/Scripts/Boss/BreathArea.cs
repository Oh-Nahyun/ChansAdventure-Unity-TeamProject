using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathArea : MonoBehaviour
{
    Boss boss;

    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
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
