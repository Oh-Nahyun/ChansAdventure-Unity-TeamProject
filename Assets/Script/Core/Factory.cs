using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    /// <summary>
    /// 노이즈 반지름
    /// </summary>
    public float noisePower = 0.5f;

    SwordSkeletonPool swordSkeletonPool;
    DamageTextPool damageTextPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        swordSkeletonPool = GetComponentInChildren<SwordSkeletonPool>();
        if (swordSkeletonPool != null) swordSkeletonPool.Initialize();

        damageTextPool = GetComponentInChildren<DamageTextPool>();
        if (damageTextPool != null) damageTextPool.Initialize();
    }

    /// <summary>
    /// 검사스켈레톤 하나 가져오는 함수
    /// </summary>
    /// <returns>배치된 슬라임 하나</returns>
    public SwordSkeleton GetEnemy()
    {
        return swordSkeletonPool.GetObject();
    }

    /// <summary>
    /// 검사스켈레톤 하나를 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 슬라임 하나</returns>
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
    /// <returns>배치된 슬라임 하나</returns>
    public SwordSkeleton GetEnemy(int index, Vector3 position, float angle = 0.0f)
    {
        return swordSkeletonPool.GetObject(index, position, angle * Vector3.forward);
    }

    /// <summary>
    /// 데미지 텍스트를 생성하는 함수
    /// </summary>
    /// <param name="damage">설정될 데미지</param>
    /// <returns></returns>
    public GameObject GetDamageText(int damage, Vector3? position)
    {
        return damageTextPool.GetObject(damage, position);
    }
}