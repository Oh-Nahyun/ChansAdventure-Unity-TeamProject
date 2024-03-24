using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBase : NPCBase
{
    Interaction interaction;

    public Transform warpPoint; // 워프할 위치
    public Transform player; // 워프할 위치
    public bool warpReady;

    protected override void Awake()
    {
        isWarp = true;
        interaction = FindObjectOfType<Interaction>();
        player = interaction.gameObject.transform;
    }

    public void WarpToWarpPoint()
    {
        if (warpPoint != null)
        {
            player.position = warpPoint.position;
        }
    }


}
