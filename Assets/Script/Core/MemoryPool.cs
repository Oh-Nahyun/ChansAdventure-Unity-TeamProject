using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트들을 관리할 풀 스크립트
/// </summary>
public class MemoryPool
{
    /// <summary>
    ///  메모리 풀로 관리되는 오브젝트
    /// </summary>
    private class PoolItem
    {
        /// <summary>
        /// 오브젝트의 활성화 / 비활설ㅇ화 정보
        /// </summary>
        public bool isActive;

        /// <summary>
        /// 화면에 보이는 실제 게임 오브젝트
        /// </summary>
        public GameObject gameObject;
    }

    /// <summary>
    /// 오브젝ㅌ트가 부족할 때 Instantiate()로 추가 생성되는 오브젝트 갯수
    /// </summary>
    int increaseCount = 5;

    /// <summary>
    ///  현재 리스트에 등록되어있는 오브젝트 갯수
    /// </summary>
    int maxCount;

    /// <summary>
    /// 현재 게임에 사용되고 있는(활성화된) 오브젝트 개수
    /// </summary>
    int activeCount;

    /// <summary>
    /// 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    /// </summary>
    GameObject poolObject;
    
    /// <summary>
    /// 관리되는 모든 오브젝트를 저장하는 리스트
    /// </summary>
    List<PoolItem> poolItemList;

    /// <summary>
    /// 외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    /// </summary>
    public int MaxCount => maxCount;

    /// <summary>
    /// 외부에서 현재 활성화된 오브젝트의 개수 확인을 위한 프로퍼티
    /// </summary>
    public int ActiveCount => activeCount;

    /// <summary>
    /// 오브젝트가 임시로 보관되는 위치
    /// </summary>
    Vector3 tmepPosition = new Vector3(48, 1, 48);

    /// <summary>
    /// 변수들 초기화 작업
    /// </summary>
    /// <param name="poolObject"></param>
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>(); //리스트 생성

        InstantiateObjects(); // 풀링 함수 실행
    }

    /// <summary>
    /// 오브젝트 생성하는 함수
    /// </summary>
    public void InstantiateObjects()
    {
        maxCount += increaseCount; // 최대 갯수 증가

        for(int i = 0; i < increaseCount; ++i) 
        {
            PoolItem poolItem = new PoolItem(); // 리스트 생성

            poolItem.isActive = false;  // 오브젝트 비활성화하게 만드느 ㄴ변수 설정
            poolItem.gameObject = GameObject.Instantiate(poolObject); // 오브젝트 생성
            poolItem.gameObject.transform.position = tmepPosition; // 오브젝트 위치 변경(임시로 보관될 위치)
            poolItem.gameObject.SetActive(false); // 오브젝트 안보이게 하기

            poolItemList.Add(poolItem); // 리스트 배열에 추가
        }
    }

    /// <summary>
    /// 현재 관리중인 모든 오브젝트를 삭제
    /// </summary>
    public void Destroyobjects()
    {
        if (poolItemList == null) return; // 아이텐 리스트가 비어있으면 반환

        int count = poolItemList.Count; // 리스트 개수 만큼 저장
        for(int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject); // 리스트의 오브젝트들 파괴
        }

        poolItemList.Clear(); 
    }

    /// <summary>
    /// 현재 비활성화 상태의 오브젝트 중 하나를 활성화로 만들어 사용하는 함수
    /// </summary>
    /// <returns></returns>
    public GameObject ActivePoolItem()
    {
        if(poolItemList == null) return null; // 리스트가 비어있으면 반환

        // 현재 생성해서 관리하는 모든 오브젝트 개수와 현재 활성화 상태인 오브젝트 개수 비교
        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 필요함
        if(maxCount == activeCount) // 모두 활성화 상태이면 
        {
            InstantiateObjects(); // 추가로 오브젝ㅌ츠 생성
        }

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i]; // 관리할 ㅇ오브젝트 리스트에 오브젝트 넣기

            if(poolItem.isActive ==  false) // 비활성화된 오브젝트만
            {
                activeCount++;

                poolItem.isActive = true; 
                poolItem.gameObject.SetActive (true);   // 활성화 

                return poolItem.gameObject; // 관리할 오브젝트 반환
            }
        }

        return null;
    }

    /// <summary>
    /// 현재 사용이 완료된 오브젝트를 비활성화 상태롤 설정
    /// </summary>
    /// <param name="removeObject">비활성화 할 오브젝트</param>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;   // 리스트가 비어있거나 비활성화할 오브젝트가 없으면 반환

        int count = poolItemList.Count;             // 리스트 크기 값 저장
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];    // 관리할 ㅇ오브젝트 리스트에 오브젝트 넣기

            if (poolItem.gameObject == removeObject) // 풀아이템의 오브젝트가 비활성화 할 오브젝트이면
            {
                activeCount--; // 카운트 줄이기

                poolItem.gameObject.transform.position = tmepPosition; // 오브젝트 비활성화할 때 위치 변경(임시로 보관될 위치)
                poolItem.isActive = false;  
                poolItem.gameObject.SetActive (false); // 비활성화

                return;
            }
        }
    }

    /// <summary>
    /// 게임에 사용중인 모든 오브젝트를 비활성화 상태로 설정하ㅏ는 함수
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return; // 리스트가 없으면 반환

        int count = poolItemList.Count; // 리스트 개수 값 저장
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];    // 관리할 ㅇ오브젝트 리스트에 오브젝트 넣기

            if (poolItem.gameObject != null && poolItem.isActive == true) // 관리할 오브젝트가 있고 활성화 상태이면
            {
                poolItem.gameObject.transform.position = tmepPosition; // 오브젝트 비활성화할 때 위치 변경(임시로 보관될 위치)
                poolItem.isActive = false;      
                poolItem.gameObject.SetActive (false);  // 비활성화
            }
        }

        activeCount = 0;    // 초기화
    }
}
