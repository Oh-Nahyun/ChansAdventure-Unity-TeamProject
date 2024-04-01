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

    SwordSkeletonPool swordSkeleton;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        swordSkeleton = GetComponentInChildren<SwordSkeletonPool>();
        if (swordSkeleton != null) swordSkeleton.Initialize();
    }

    /// <summary>
    /// 슬라임 하나 가져오는 함수
    /// </summary>
    /// <returns>배치된 슬라임 하나</returns>
    public SwordSkeleton GetSwordSkeleton()
    {
        return swordSkeleton.GetObject();
    }

    /// <summary>
    /// 슬라임 하나를 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 슬라임 하나</returns>
    public SwordSkeleton GetSwordSkeleton(Vector3 position, float angle = 0.0f)
    {
        return swordSkeleton.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 슬라임 하나를 특정 웨이포인트를 사용하고, 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="index">사용할 웨이포인트의 인덱스</param>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 슬라임 하나</returns>
    public SwordSkeleton GetSwordSkeleton(int index, Vector3 position, float angle = 0.0f)
    {
        return swordSkeleton.GetObject(index, position, angle * Vector3.forward);
    }

}