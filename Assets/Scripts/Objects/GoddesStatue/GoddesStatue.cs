using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddesStatue : MonoBehaviour
{

    /// <summary>
    /// Æ½´ç È¸º¹ÇÒ ·®
    /// </summary>
    public float tickRegen = GameManager.Instance.Player.maxHP;

    /// <summary>
    /// Æ½ ÀÎÅÍ¹ú
    /// </summary>
    public float inverval = 1;

    /// <summary>
    /// È¸º¹ Æ½ °³¼ö
    /// </summary>
    public uint tickCount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IHealth health = other.GetComponent<IHealth>();
            if (health != null)
            {
                tickRegen = GameManager.Instance.Player.maxHP;
                health.HealthRegenerateByTick(tickRegen, inverval, tickCount);
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
