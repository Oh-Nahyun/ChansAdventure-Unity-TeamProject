using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 스테이지 세팅 스크립트
/// </summary>
public class BossStageSetting : MonoBehaviour
{
    Boss boss;

    public Transform dropposition;
    public GameObject exitObj;

    void Start()
    {
        boss = FindAnyObjectByType<Boss>(); // 보스 찾기
        boss.gameObject.SetActive(false);   // 보스 비활성화

        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4], dropposition.position);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8], dropposition.position);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9],5 ,dropposition.position);
    }

    private void Update()
    {
        if(!boss.IsAlive)
        {
            exitObj.SetActive(true);
        }
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
