using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBase : NPCBase
{
    Interaction interaction;

    /// <summary>
    /// 워프할 위치
    /// </summary>
    public Transform warpPoint;
    Player player;

    protected override void Awake()
    {
        otherObject = true;
        interaction = FindObjectOfType<Interaction>();
        player = FindAnyObjectByType<Player>();
        //player = interaction.gameObject.transform;
    }

    /// <summary>
    /// 플레이어를 워프시키는 함수
    /// </summary>
    public void WarpToWarpPoint()
    {
        if (warpPoint != null)
        {
            Vector3 warpPosition = warpPoint.position;
            warpPosition.y += player.transform.position.y + 3;
            
            player.transform.position = warpPosition;
        }
    }



}
