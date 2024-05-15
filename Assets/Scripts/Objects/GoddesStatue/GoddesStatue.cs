using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddesStatue : MonoBehaviour
{

    /// <summary>
    /// 틱당 회복할 량 ( 캐릭터 최대 채력 )
    /// </summary>
    public float tickRegen = 10f;

    /// <summary>
    /// 틱 인터벌
    /// </summary>
    public float inverval = 0.2f;

    /// <summary>
    /// 회복 틱 개수
    /// </summary>
    public uint tickCount = 100;

    private void Start()
    {
        tickRegen = GameManager.Instance.Player.MaxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IHealth health = GameManager.Instance.Player as IHealth;
            if (health != null)
            {
                tickRegen = GameManager.Instance.Player.MaxHP;
                health.HealthRegenerateByTick(tickRegen * 0.1f, inverval, tickCount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tickRegen = 0;
        }
    }



}
