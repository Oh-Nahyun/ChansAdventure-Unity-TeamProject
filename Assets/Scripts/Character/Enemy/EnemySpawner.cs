using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : Waypoints
{
    public enum EnemyType : byte
    {
        SwordSkeleton = 0,
        NightmareDragon,
    }

    public EnemyType enemyType = EnemyType.SwordSkeleton;

    /// <summary>
    /// 스폰 간격
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// 마지막 스폰에서 지난 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 스포너에서 동시에 최대로 유지가능한 적의 수
    /// </summary>
    public int capacity = 3;

    /// <summary>
    /// 현재 스폰된 적의 수
    /// </summary>
    int count = 0;


    private void Update()
    {
        if (count < capacity)            // 캐퍼시티 확인하고
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > interval)  // 인터벌 확인
            {
                Spawn();                // 둘 다 통과가 되면 스폰
                elapsedTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// 적을 한마리 스폰하는 함수
    /// </summary>
    void Spawn()
    {
        if (!GameManager.Instance.isField)
            return;

        // 적 하나 스폰(waypoint들 중 랜덤으로 하나를 선택해서 생성)

        int randPos = Random.Range(0, children.Length);
        float randRot = Random.Range(0, 360.0f);

        switch (enemyType)
        {
            case EnemyType.SwordSkeleton:
                SwordSkeleton swordSkeleton = Factory.Instance.GetEnemy(children[randPos].position, randRot);
                swordSkeleton.onDie += () =>
                {
                    count--;
                };
                count++;
                break;
            case EnemyType.NightmareDragon:
                NightmareDragon nightmareDragon = Factory.Instance.GetNightmareDragonEnemy(children[randPos].position, randRot);
                nightmareDragon.onDie += () =>
                {
                    count--;
                };
                count++;
                break;
        }

    }
}