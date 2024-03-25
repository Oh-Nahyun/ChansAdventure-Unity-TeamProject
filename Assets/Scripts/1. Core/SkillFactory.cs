using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum SkillType
{
    RemoteBombPool = 0,
    MagnetCatchPool,
}

public class SkillFactory : Singleton<SkillFactory>
{
    RemoteBombPool remoteBombPool;
    MagnetCatchPool magnetCatchPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        remoteBombPool = GetComponentInChildren<RemoteBombPool>();
        if (remoteBombPool != null) remoteBombPool.Initialize();
        magnetCatchPool = GetComponentInChildren<MagnetCatchPool>();
        if (magnetCatchPool != null) magnetCatchPool.Initialize();
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
            case SkillType.MagnetCatchPool:
                result = magnetCatchPool.GetObject(position, euler).gameObject;
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
    public MagnetCatch GetMagnetCatch(Vector3? position = null, float angle = 0.0f)
    {
        return magnetCatchPool.GetObject(position, angle * Vector3.forward);
    }

}