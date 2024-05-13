using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 스테이지 세팅 스크립트
/// </summary>
public class BossStageSetting : MonoBehaviour
{
    Boss boss;

    void Start()
    {
        boss = FindAnyObjectByType<Boss>(); // 보스 찾기
        boss.gameObject.SetActive(false);   // 보스 비활성화
    }

    public Boss GetBoss()
    {
        if (boss == null)
        {
            Debug.LogWarning($"BossStageSetting : Boss 오브젝트를 찾을 수 없습니다");
        }

        return boss;
    }
}
