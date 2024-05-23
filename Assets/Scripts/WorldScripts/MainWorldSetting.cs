using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldSetting : MonoBehaviour
{
    public Transform EnemySpawnPosition;
    public Transform ItemDropPosition;

    private void Start()
    {
        //for(int i = 0; i < 3; i++)
        //{
        //    Vector3 noisePosition = Random.onUnitSphere.normalized * 5f;  // 구 범위네 랜덤 위치 설정
        //}

        // 아이템 소환
        ItemDataManager dataManager = GameManager.Instance.ItemDataManager;

        Factory.Instance.GetItemObject(dataManager[ItemCode.Coin], ItemDropPosition.position);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.Arrow], 10, ItemDropPosition.position, true);
    }
}
