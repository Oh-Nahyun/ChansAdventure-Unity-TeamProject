using System;
using UnityEngine;

/// <summary>
/// 플레이어의 스킬내용만 받는 스크립트
/// </summary>
public class PlayerSkills : MonoBehaviour
{
    // components
    Skill currentActiveSkill;

    // 각각의 스킬들 (스킬들의 on/off 확인용, off면 null)
    RemoteBomb remoteBomb;
    RemoteBombCube remoteBombCube;
    MagnetCatch magnetCatch;

    // Delegates
    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;

    /// <summary>
    /// 휠: 마그넷캐치 연결시 앞뒤이동
    /// </summary>
    public Action<float> onScroll;

    /// <summary>
    /// 현재 활성화된 스킬
    /// </summary>
    SkillName currentSkill = SkillName.RemoteBomb;

    /// <summary>
    /// 선택된 스킬이 바뀌었음을 알리는 델리게이트 (F1:리모컨폭탄 F2:리모컨폭탄큐브 F3:마그넷캐치 F4:아이스메이커 F5:타임록)
    /// </summary>
    public Action<SkillName> onSkillSelect;


    void Awake()
    {
        PlayerSkillController skillController = GetComponent<PlayerSkillController>();

        skillController.onSkillSelect += ConnectSkill; // onSkillSelect가 같은 스크립트 내에서 처리됨
        skillController.onSkillActive += OnSkill;
        skillController.rightClick += () => useSkillAction?.Invoke();
        skillController.onCancel += () => offSkillAction.Invoke();

        skillController.onSkillChange += (skillName) => 
        {
            currentSkill = skillName;
            Debug.Log($"[{currentSkill}] 활성화");
        };
    }

    void OnSkill()
    {
        // 설정된 스킬별로 동작
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                if (remoteBomb == null)     // 리모컨폭탄이 현재 소환되어 있지 않으면
                {
                    //Debug.Log("실행 : 리모컨 폭탄");
                    remoteBomb = Factory.Instance.GetRemoteBomb();          // 팩토리에서 리모컨폭탄 가져온 뒤 리모컨 폭탄 변수에 설정
                    remoteBomb.OnSKillInitialize(transform);                // 사용자는 이스크립트를 가진 트랜스폼(=플레이어)
                    currentActiveSkill = remoteBomb;                        // 현재 사용중인 스킬은 리모컨폭탄
                }
                break;
            case SkillName.RemoteBomb_Cube:
                if (remoteBombCube == null)
                {
                    //Debug.Log("실행 : 리모컨 폭탄 큐브");
                    remoteBombCube = Factory.Instance.GetRemoteBombCube();
                    remoteBombCube.OnSKillInitialize(transform);
                    currentActiveSkill = remoteBombCube;
                }
                break;
            case SkillName.MagnetCatch:
                if (magnetCatch == null)
                {
                    //Debug.Log("실행 : 마그넷 캐치");
                    magnetCatch = Factory.Instance.GetMagnetCatch();
                    magnetCatch.OnSKillInitialize(transform);
                    currentActiveSkill = magnetCatch;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

        ConnectSkill(currentSkill);

        // 순서 중요: 들기 -> 스킬발동 (리모컨 폭탄의 경우 None 상태에서 스킬 발동을 하면 폭발하기 때문에 PickUp 상태로 시작하기 위해서)
        
        currentActiveSkill?.TryPickUp(transform);    // 들기 (해당 transform의(Player) 다른 스크립트인 PlayerRelatedAction에 ILifter가 존재)
        onSKillAction?.Invoke();                     // 스킬 발동


    }

    void ConnectSkill(SkillName skiilName)
    {
        // 현재 스킬 종류로 설정
        currentSkill = skiilName;

        // 스킬 변경시 델리게이트 연결 해제
        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;

        // 설정된 스킬이 현재 발동 중이면 스킬 관련(시작, 사용중, 종료) 델리게이트 연결
        // 설정된 스킬로 현재사용중인 스킬 연결 (각 스킬이 없으면 null)
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                currentActiveSkill = remoteBomb;
                if (remoteBomb != null)
                {
                    onSKillAction = remoteBomb.OnSkill;
                    useSkillAction = remoteBomb.UseSkill;
                    offSkillAction = remoteBomb.OffSkill;
                    remoteBomb.cancelSkill = CancelSkill;
                }

                break;
            case SkillName.RemoteBomb_Cube:
                currentActiveSkill = remoteBombCube;
                if (remoteBombCube != null)
                {
                    onSKillAction = remoteBombCube.OnSkill;
                    useSkillAction = remoteBombCube.UseSkill;
                    offSkillAction = remoteBombCube.OffSkill;
                    remoteBombCube.cancelSkill = CancelSkill;
                }
                break;
            case SkillName.MagnetCatch:
                currentActiveSkill = magnetCatch;
                if (magnetCatch != null)
                {
                    onSKillAction = magnetCatch.OnSkill;
                    useSkillAction = magnetCatch.UseSkill;
                    offSkillAction = magnetCatch.OffSkill;
                    magnetCatch.cancelSkill = CancelSkill;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }

    void CancelSkill()
    {
        //offSkillAction?.Invoke();
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                remoteBomb = null;
                currentActiveSkill = null;
                break;
            case SkillName.RemoteBomb_Cube:
                remoteBombCube = null;
                currentActiveSkill = null;
                break;
            case SkillName.MagnetCatch:
                magnetCatch = null;
                currentActiveSkill = null;
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }
}
