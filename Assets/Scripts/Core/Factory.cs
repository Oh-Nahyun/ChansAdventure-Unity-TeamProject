using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Arrow = 0,
}


public enum EnemyObjectType
{
    SwordSkeleton = 0,
    NightMareDragon,
}

public class Factory : Singleton<Factory>
{
    RemoteBombPool remoteBombPool;
    RemoteBombCubePool remoteBombCubePool;
    MagnetCatchPool magnetCatchPool;
    IceMakerPool iceMakerPool;
    IceMaker_IcePool iceMaker_IcePool;
    TimeLockPool timeLockPool;
    TimeLockArrowPool timeLockArrowPool;
    SwordSkeletonPool swordSkeletonPool;
    DamageTextPool damageTextPool;
    ItemPool itemPool;
    NightmareDragonPool nightmareDragonPool;
    FireBallPool fireballPool;

    ArrowPool arrowPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        remoteBombPool = GetComponentInChildren<RemoteBombPool>();
        if (remoteBombPool != null) remoteBombPool.Initialize();
        remoteBombCubePool = GetComponentInChildren<RemoteBombCubePool>();
        if (remoteBombCubePool != null) remoteBombCubePool.Initialize();
        magnetCatchPool = GetComponentInChildren<MagnetCatchPool>();
        if (magnetCatchPool != null) magnetCatchPool.Initialize();
        iceMakerPool = GetComponentInChildren<IceMakerPool>();
        if (iceMakerPool != null) iceMakerPool.Initialize();
        iceMaker_IcePool = GetComponentInChildren<IceMaker_IcePool>();
        if (iceMaker_IcePool != null) iceMaker_IcePool.Initialize();
        timeLockPool = GetComponentInChildren<TimeLockPool>();
        if (timeLockPool != null) timeLockPool.Initialize();
        timeLockArrowPool = GetComponentInChildren<TimeLockArrowPool>();
        if (timeLockArrowPool != null) timeLockArrowPool.Initialize();
        swordSkeletonPool = GetComponentInChildren<SwordSkeletonPool>();
        if (swordSkeletonPool != null) swordSkeletonPool.Initialize();
        damageTextPool = GetComponentInChildren<DamageTextPool>();
        if (damageTextPool != null) damageTextPool.Initialize();
        nightmareDragonPool = GetComponentInChildren<NightmareDragonPool>();
        if(nightmareDragonPool != null) nightmareDragonPool.Initialize();

        fireballPool = GetComponentInChildren<FireBallPool>();
        if (fireballPool != null) fireballPool.Initialize();

        // Inventory Branch
        itemPool = GetComponentInChildren<ItemPool>();
        if (itemPool != null) itemPool.Initialize();

        // Player Weapon Arrow
        arrowPool = GetComponentInChildren<ArrowPool>();
        if (arrowPool != null)
            arrowPool.Initialize();

        DisableAllObject();
    }

    /// <summary>
    /// 풀에 있는 스킬 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="name">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public Skill GetSkill(SkillName name, Vector3? position = null, Vector3? euler = null)
    {
        Skill result = null;
        switch (name)
        {
            case SkillName.RemoteBomb:
                result = remoteBombPool.GetObject(position, euler);
                break;
            case SkillName.RemoteBomb_Cube:
                result = remoteBombCubePool.GetObject(position, euler);
                break;
            case SkillName.MagnetCatch:
                result = magnetCatchPool.GetObject(position, euler);
                break;
            case SkillName.IceMaker:
                result = iceMakerPool.GetObject(position, euler);
                break;
            case SkillName.TimeLock:
                result = timeLockPool.GetObject(position, euler);
                break;
        }

        return result;
    }

    /// <summary>
    /// 리모컨폭탄을 가져오는 함수
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <returns>활성화된 리모컨폭탄</returns>
    public RemoteBomb GetRemoteBomb(Vector3? position = null, float angle = 0.0f)
    {
        return remoteBombPool.GetObject(position, angle * Vector3.forward);
    }
    public RemoteBombCube GetRemoteBombCube(Vector3? position = null, float angle = 0.0f)
    {
        return remoteBombCubePool.GetObject(position, angle * Vector3.forward);
    }
    public MagnetCatch GetMagnetCatch(Vector3? position = null, float angle = 0.0f)
    {
        return magnetCatchPool.GetObject(position, angle * Vector3.forward);
    }
    public IceMaker GetIceMaker(Vector3? position = null, float angle = 0.0f)
    {
        return iceMakerPool.GetObject(position, angle * Vector3.forward);
    }
    public IceMaker_Ice GetIceMaker_Ice(Vector3? position = null, float angle = 0.0f)
    {
        return iceMaker_IcePool.GetObject(position, angle * Vector3.forward);
    }
    public TimeLock GetTimeLock(Vector3? position = null, float angle = 0.0f)
    {
        return timeLockPool.GetObject(position, angle * Vector3.forward);
    }
    public TimeLockArrow GetTimeLockArrow(Vector3? position = null, float angle = 0.0f)
    {
        return timeLockArrowPool.GetObject(position, angle * Vector3.forward);
    }

    // 적 생성 함수 ------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 검사스켈레톤 하나 가져오는 함수
    /// </summary>
    /// <returns>배치된 검사스켈레톤 하나</returns>
    public SwordSkeleton GetEnemy()
    {
        return swordSkeletonPool.GetObject();
    }

    /// <summary>
    /// 검사스켈레톤 하나를 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 검사스켈레톤 하나</returns>
    public SwordSkeleton GetEnemy(Vector3 position, float angle = 0.0f)
    {
        return swordSkeletonPool.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 검사스켈레톤 하나를 특정 웨이포인트를 사용하고, 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="index">사용할 웨이포인트의 인덱스</param>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 검사스켈레톤 하나</returns>
    public SwordSkeleton GetEnemy(int index, Vector3 position, float angle = 0.0f)
    {
        return swordSkeletonPool.GetObject(index, position, angle * Vector3.forward);
    }

    /// <summary>
    /// 나이트메어드래곤 하나 가져오는 함수
    /// </summary>
    /// <returns>배치된 나이트메어드래곤 하나</returns>
    public NightmareDragon GetNightmareDragonEnemy()
    {
        return nightmareDragonPool.GetObject();
    }

    /// <summary>
    /// 나이트메어드래곤 하나를 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 나이트메어드래곤 하나</returns>
    public NightmareDragon GetNightmareDragonEnemy(Vector3 position, float angle = 0.0f)
    {
        return nightmareDragonPool.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 나이트메어드래곤 하나를 특정 웨이포인트를 사용하고, 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="index">사용할 웨이포인트의 인덱스</param>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 나이트메어드래곤 하나</returns>
    public NightmareDragon GetNightmareDragonEnemy(int index, Vector3 position, float angle = 0.0f)
    {
        return nightmareDragonPool.GetObject(index, position, angle * Vector3.forward);
    }

    // -------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 데미지 텍스트를 생성하는 함수
    /// </summary>
    /// <param name="damage">설정될 데미지</param>
    /// <returns></returns>
    public GameObject GetDamageText(int damage, Vector3? position)
    {
        return damageTextPool.GetObject(damage, position);
    }

    // Inventory Branch
    /// <summary>
    /// Factory에서 아이템 1개 생성하는 함수
    /// </summary>
    /// <param name="slot">소환할 아이템 슬롯</param>
    /// <param name="position">소환할 위치</param>
    /// <returns></returns>
    public GameObject GetItemObject(ItemData itemData, Vector3? position = null)
    {
        GameObject obj = itemPool.GetItemObject(itemData, position);

        return obj;
    }

    /// <summary>
    /// 아이템들을 생성하는 함수
    /// </summary>
    /// <param name="itemData">생성할 아이템 데이터</param>
    /// <param name="count">아이탬 개수</param>
    /// <param name="position">아이템 위치</param>
    /// <param name="getNoise">true면 포지션 + 랜덤위치 설정, false면 position에 생성</param>
    /// <returns></returns>
    public GameObject[] GetItemObjects(ItemData itemData, uint count = 1, Vector3? position = null, bool getNoise = false)
    {
        GameObject[] objs = new GameObject[count];  // 아이템 개수만큼 증가
        Vector3? itemPosition = Vector3.zero;       // 설정될 아이템 위치

        for(int i = 0; i < objs.Length; i++)
        {
            Vector3 noisePosition = Random.onUnitSphere.normalized * 1.5f;  // 구 범위네 랜덤 위치 설정
            if(getNoise)
            {
                itemPosition = position + noisePosition;                    // 아이템 위치 설정
            }

            objs[i] = itemPool.GetItemObject(itemData, itemPosition);       // 배열에 아이템 저장
        }

        return objs;    // 아이템풀에 아이템 반환
    }

    // Player Weapon Arrow
    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="euler">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.Arrow:
                result = arrowPool.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    public FireBall GetFireBall()
    {
        return fireballPool.GetObject();
    }
    public FireBall GetFireBall(Vector3 position, float angle = 0.0f)
    {
        return fireballPool.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 풀에 있는 모든 오브젝트를 비활성화 하는 함수
    /// </summary>
    void DisableAllObject()
    {
        Transform child;
        for(int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(0);

            for (int j = 0; j < child.childCount; j++)
            {
                if(child.gameObject.GetComponent<SwordSkeletonPool>() != null
                || child.gameObject.GetComponent<NightmareDragonPool>() != null)
                {
                    child.GetChild(j + 1).gameObject.SetActive(false);
                }

                child.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
}