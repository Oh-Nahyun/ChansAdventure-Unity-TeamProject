using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookVCam : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 플레이어와 카메라간의 거리
    /// </summary>
    public Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);

    void Start()
    {
        player = GameManager.Instance.Player;
    }

    void Update()
    {
        transform.localPosition = player.transform.position + offset;
    }
}
