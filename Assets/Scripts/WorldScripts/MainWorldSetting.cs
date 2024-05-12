using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldSetting : MonoBehaviour
{
    public Transform EnemySpawnPosition;

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 noisePosition = Random.onUnitSphere.normalized * 5f;  // 구 범위네 랜덤 위치 설정
            Factory.Instance.GetEnemy(EnemySpawnPosition.position + noisePosition);
        }
    }
}
