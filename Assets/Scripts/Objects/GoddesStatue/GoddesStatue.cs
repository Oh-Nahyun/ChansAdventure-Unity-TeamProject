using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddesStatue : MonoBehaviour
{

    /// <summary>
    /// Æ½´ç È¸º¹ÇÒ ·®
    /// </summary>
    public float tickRegen = GameManager.Instance.Player.MaxHP;

    /// <summary>
    /// Æ½ ÀÎÅÍ¹ú
    /// </summary>
    public float inverval = 0.2f;

    /// <summary>
    /// È¸º¹ Æ½ °³¼ö
    /// </summary>
    public uint tickCount = 100;

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
