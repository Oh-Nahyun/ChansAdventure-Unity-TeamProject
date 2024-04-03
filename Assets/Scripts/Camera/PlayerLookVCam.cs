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

    void Awake()
    {
        player = FindAnyObjectByType<Player>();

#if UNITY_EDITOR
        if(player == null)
        {
            Test_Player = FindAnyObjectByType<Test_99_Player>();
            Debug.Log($"테스트 플레이어 오브젝트에 접근되었습니다. 플레이어 스크립트를 찾을 수 없습니다.");
        }
#endif
    }

    void Update()
    {
        //transform.localPosition = player.transform.position + offset;
        transform.localPosition = Test_Player.transform.position + offset;
    }

#if UNITY_EDITOR
    Test_99_Player Test_Player;
#endif
}
