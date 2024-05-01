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
    IceMaker iceMaker;
    TimeLock timeLock;

    // Delegates
    // 스킬 발동 관련 델리게이트
    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;
    /// <summary>
    /// 특수스킬키를 눌렀을 때 발동되는 델리게이트
    /// </summary>
    public Action<SpecialKey> onSpecialKey;

    Action onDrop;

    public enum SpecialKey
    {
        None = 0,               // 키를 뗐을 때
        SquareBracket_Open,     // 대괄호 열기 "["
        SquareBracket_Close,    // 대괄호 닫기 "]"
    }

    /// <summary>
    /// 현재 활성화된 스킬
    /// </summary>
    SkillName currentSkill = SkillName.RemoteBomb;

    SkillName CurrentSkill
    {
        get => currentSkill;
        set
        {
            if (currentSkill != value)
            {
                currentSkill = value;
                ConnectSkill(currentSkill);
            }
        }
    }

    /// <summary>
    /// 선택된 스킬이 바뀌었음을 알리는 델리게이트 (F1:리모컨폭탄 F2:리모컨폭탄큐브 F3:마그넷캐치 F4:아이스메이커 F5:타임록)
    /// </summary>
    public Action<SkillName> onSkillSelect;


    void Start()
    {
        PlayerSkillController skillController = GetComponent<PlayerSkillController>();

        skillController.onSkillActive += OnSkill;
        skillController.rightClick += () => useSkillAction?.Invoke();
        skillController.onCancel += () => offSkillAction?.Invoke();

        int enumLenght = Enum.GetValues(typeof(SpecialKey)).Length;
        for (int i = 0; i < enumLenght; i++)
        {
            int index = i;      // 반복문 안에서 람다식을 사용할 때 변수값이 이상해 지는 것 방지하기위해 반복문 안에 변수 선언 후 사용
            skillController.onSpecialKey[i] += () => {
                onSpecialKey?.Invoke((SpecialKey)index);
            };
        }

        PlayerSkillRelatedAction relatedAction = GetComponent<PlayerSkillRelatedAction>();

        relatedAction.onSkillChange += (skillName) =>
        {
            CurrentSkill = skillName;
        };
        onDrop += relatedAction.Drop;
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
                if (iceMaker == null)
                {
                    iceMaker = Factory.Instance.GetIceMaker();
                    iceMaker.OnSKillInitialize(transform);
                    currentActiveSkill = iceMaker;
                }
                break;
            case SkillName.TimeLock:
                if (timeLock == null)
                {
                    timeLock = Factory.Instance.GetTimeLock();
                    timeLock.OnSKillInitialize(transform);
                    currentActiveSkill = timeLock;
                }
                break;
        }

        ConnectSkill(CurrentSkill);

        // 순서 중요: 스킬발동 -> 들기
        onSKillAction?.Invoke();                     // 스킬 발동
        currentActiveSkill?.TryPickUp(transform);    // 들기 (해당 transform의(Player) 다른 스크립트인 PlayerRelatedAction에 ILifter가 존재)
    }

    void ConnectSkill(SkillName skiilName)
    {
        // 현재 스킬 종류로 설정
        CurrentSkill = skiilName;

        // 스킬 변경시 델리게이트 연결 해제
        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;
        onSpecialKey = null;

        // 설정된 스킬이 현재 발동 중이면 스킬 관련(시작, 사용중, 종료) 델리게이트 연결
        // 설정된 스킬로 현재사용중인 스킬 연결 (각 스킬이 없으면 null)
        switch (CurrentSkill)
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
                    onSpecialKey = magnetCatch.InputSpecialKey;

                    magnetCatch.cancelSkill = CancelSkill;
                }
                break;
            case SkillName.IceMaker:
                currentActiveSkill = iceMaker;
                if (iceMaker != null)
                {
                    onSKillAction = iceMaker.OnSkill;
                    useSkillAction = iceMaker.UseSkill;
                    offSkillAction = iceMaker.OffSkill;
                    onSpecialKey = iceMaker.InputSpecialKey;

                    iceMaker.cancelSkill = CancelSkill;

                }
                break;
            case SkillName.TimeLock:
                currentActiveSkill = timeLock;
                if (timeLock != null)
                {
                    onSKillAction = timeLock.OnSkill;
                    useSkillAction = timeLock.UseSkill;
                    offSkillAction = timeLock.OffSkill;

                    timeLock.cancelSkill = CancelSkill;

                }
                break;
        }
    }
    // 스킬 변경시 타임록/아이스메이커 테스트(취소되나?)
    void CancelSkill()
    {
        onDrop?.Invoke();

        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;
        onSpecialKey = null;

        //offSkillAction?.Invoke();
        switch (CurrentSkill)
        {
            case SkillName.RemoteBomb:
                remoteBomb.cancelSkill = null;
                remoteBomb = null;
                break;
            case SkillName.RemoteBomb_Cube:
                remoteBombCube.cancelSkill = null;
                remoteBombCube = null;
                break;
            case SkillName.MagnetCatch:
                magnetCatch.cancelSkill = null;
                magnetCatch = null;
                break;
            case SkillName.IceMaker:
                iceMaker.cancelSkill = null;
                iceMaker = null;
                break;
            case SkillName.TimeLock:
                timeLock.cancelSkill = null;
                timeLock = null;
                break;
        }
        currentActiveSkill = null;
    }
}
