using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PoolObjectType
{
    Arrow = 0,
}

public enum SkillType
{
    RemoteBombPool = 0,
    RemoteBombCubePool,
    MagnetCatchPool,
    IceMakerPool,
    TimeLock
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
    SwordSkeletonPool swordSkeletonPool;
    DamageTextPool damageTextPool;
    ItemPool itemPool;
    NightmareDragonPool nightmareDragonPool;

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
        swordSkeletonPool = GetComponentInChildren<SwordSkeletonPool>();
        if (swordSkeletonPool != null) swordSkeletonPool.Initialize();
        damageTextPool = GetComponentInChildren<DamageTextPool>();
        if (damageTextPool != null) damageTextPool.Initialize();
        nightmareDragonPool = GetComponentInChildren<NightmareDragonPool>();
        if(nightmareDragonPool != null) nightmareDragonPool.Initialize();

        // Inventory Branch
        itemPool = GetComponentInChildren<ItemPool>();
        if (itemPool != null) itemPool.Initialize();

        // Player Weapon Arrow
        arrowPool = GetComponentInChildren<ArrowPool>();
        if (arrowPool != null)
            arrowPool.Initialize();

    }

    /// <summary>
    /// 풀에 있는 스킬 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(SkillType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case SkillType.RemoteBombPool:
                result = remoteBombPool.GetObject(position, euler).gameObject;
                break;
            case SkillType.RemoteBombCubePool:
                result = remoteBombCubePool.GetObject(position, euler).gameObject;
                break;
            case SkillType.MagnetCatchPool:
                result = magnetCatchPool.GetObject(position, euler).gameObject;
                break;
            case SkillType.IceMakerPool:
                result = iceMakerPool.GetObject(position, euler).gameObject;
                break;
            case SkillType.TimeLock:
                result = timeLockPool.GetObject(position, euler).gameObject;
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
    /// Factory에서 아이템을 생성하는 함수
    /// </summary>
    /// <param name="slot">소환할 아이템 슬롯</param>
    /// <param name="count">소환할 아이템 개수</param>
    /// <param name="position">소환할 위치</param>
    /// <returns></returns>
    public GameObject GetItemObject(ItemData itemData, uint count = 1, Vector3? position = null)
    {
        return itemPool.GetItemObject(itemData, count, position);
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
}