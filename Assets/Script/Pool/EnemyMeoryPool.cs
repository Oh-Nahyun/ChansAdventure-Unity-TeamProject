using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적을 메모리 풀로 관리할 스크립트
/// </summary>
public class EnemyMeoryPool : MonoBehaviour
{

    /// <summary>
    /// 타겟 (목표) 위치
    /// </summary>
    [SerializeField]
    Transform target;
    /// <summary>
    /// 적이 등장라기 전 적의 등장 위치를 알려주는 프리팹 넣기
    /// </summary>
    [SerializeField]
    GameObject enemySpawnPointPrefab;

    /// <summary>
    /// 생성되는 적 프리팹을 담을 공간
    /// </summary>
    [SerializeField]
    GameObject enemyPrefab;

    /// <summary>
    /// 적 생성 주기
    /// </summary>
    [SerializeField]
    float enemySpawnTime = 1.0f;

    /// <summary>
    /// 타일 생성 후 적이 등장하기까지 대기 시간
    /// </summary>
    [SerializeField]
    float enemySpawnLatency = 1.0f;

    /// <summary>
    /// 적 등장 위치를 알려주는 오브젝트 생성을 활성활/ 비활성화를 관리하기 위한 변수
    /// </summary>
    MemoryPool spawnPointMemoryPool;

    /// <summary>
    /// 적 생성을 활성화 / 비활성화를 관리하기 위한 변수
    /// </summary>
    MemoryPool enemyMemoryPool;

    /// <summary>
    /// 동시에 생성되는 적의 숫자
    /// </summary>
    int concurrentGenerationNumber = 1;

    /// <summary>
    /// 맵의 크기
    /// </summary>
    [Header("MapScale, EnemySpawnRange")]
    [SerializeField]
    Vector2Int mapSize = new Vector2Int(100, 100);

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);   // 경고 바꾸기 인스턴스 변수에 생성자
        enemyMemoryPool = new MemoryPool(enemyPrefab);                  // 경고 바꾸기 인스턴스 변수에 생성자
        //spawnPointMemoryPool = AddedComponent(enemySpawnPointPrefab) as MemoryPool;


        StartCoroutine("SpawnTile");    // 타일맵 생성하는 코루틴 시작
    }

    /// <summary>
    /// 적이 생성되기 전에 생기는 기둥을 생성하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnTile()
    {
        // 현재 한번에 생성되는 기둥의 수
        int currentNumber = 0;
        // 최대 한번에 생성되는 기둥의 수
        int maximumNumber = 50;

        while(true)
        {
            for(int i = 0; i < concurrentGenerationNumber; ++i) // 동시에 생성되는 적의 수 만큼
            {
                GameObject item = spawnPointMemoryPool.ActivePoolItem(); // 적의 스폰되는 기둥 오브젝트를 활성화 시켜줌

                // 적 기둥 위치 = x와 z는 랜덤으로 위치 설정
                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.y * 0.49f), 1,
                                                      Random.Range(-mapSize.x * 0.49f, mapSize.y * 0.49f));

                StartCoroutine("SpawnEnemy", item); // 적 생성 코루틴 시작
            }

            currentNumber++; // 현재 한번에 생성되는 적의 기둥 수 증가

            if(currentNumber >= maximumNumber) // 적의 기둥이 한번에 생성되는 수가 최대 수 보다 크면 
            {
                currentNumber = 0;      // 현재 한번에 생성되는 적의 기둥 수 초기화
                concurrentGenerationNumber++;   // 동시에 적이 생성되는 수 증가
            }

            yield return new WaitForSeconds(enemySpawnTime);    // 스폰 주기에 따라 생성
        }
    }

    /// <summary>
    /// 적 스폰하는 코루틴
    /// </summary>
    /// <param name="point">스폰할 위치</param>
    /// <returns></returns>
    IEnumerator SpawnEnemy(GameObject point)
    {
        // 적 생성 전에 나타나는 기둥 타일 생성 시간이 지나고 나서
        yield return new WaitForSeconds(enemySpawnLatency);

        // 적의 오브젝트를 생성
        GameObject item = enemyMemoryPool.ActivePoolItem();
        // 적의 위치를 point의 위치로 설정
        item.transform.position = point.transform.position;

        // 적 이동 스크립트를 찾아 Setup 메소드의 매개변수에 target 전달
        item.GetComponent<EnemyStatus>().Setup(target, this);

        // 적 생성전에 생기는 타일 오브젝트 비활성화
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    /// <summary>
    /// 적 비활성화하는 함수
    /// </summary>
    /// <param name="enemy">비활성화할 적</param>
    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy); // 비활성화
    }
}
