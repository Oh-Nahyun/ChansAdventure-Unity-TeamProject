using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum SkillName
{
    RemoteBomb = 1,
    RemoteBomb_Cube = 2,
    MagnetCatch = 3,
    TimeLock = 4,
    IceMaker = 5
}

public class Skill : ReactionObject
{
    [Header("스킬 데이터")]
    /// <summary>
    /// 스킬 사용 거리
    /// </summary>
    public float skillDistance = 50.0f;
    /// <summary>
    /// 사용중인 오브젝트 (현재 플레이어만)
    /// </summary>
    protected Transform user;
    /// <summary>
    /// 스킬의 종류(이름)
    /// </summary>
    public SkillName skillName = SkillName.RemoteBomb;

    // 쿨타임 어디서 처리? 지금은 PlayerSkills에서 생각중
    /// <summary>
    /// 쿨타임 (아직 설정 안함)
    /// </summary>
    public float coolTime = 1.0f;
    /// <summary>
    /// 현재 쿨타임 (아직 설정 안함)
    /// </summary>
    protected float currCoolTime = 0.0f;

    /// <summary>
    /// 현재 작동 중인지 확인용 변수
    /// </summary>
    protected bool isActivate = false;

    /// <summary>
    /// 스킬 사용시 화면 전환되는 카메라
    /// (살짝 옆으로 이동)
    /// </summary>
    protected SkillVCam skillVcam;

    /// <summary>
    /// 카메라가 켜져야 할 때 발동하는 델리게이트
    /// </summary>
    protected Action camOn;
    /// <summary>
    /// 카메라가 꺼져야 할 때 발동하는 델리게이트
    /// </summary>
    protected Action camOff;
    /// <summary>
    /// 스킬 취소 처리용 델리게이트
    /// (풀로 돌아갈 때 동작)
    /// </summary>
    public Action cancelSkill;
    /// <summary>
    /// 카메라 중앙
    /// </summary>
    protected readonly Vector3 Center = new Vector3(0.5f, 0.5f, 0.0f);

    protected override void Awake()
    {
        base.Awake();
        rigid.isKinematic = true;           // 들고있을 경우 Kinematic으로 지정, 리모컨폭탄류는 던지면 변경됨
        reactionType |= ReactionType.Skill; // 반응 타입에 스킬 추가 (별다른 반응은 없고 구별용)
    }

    protected virtual void Start()
    {
        if (skillVcam == null)
        {
            skillVcam = GameManager.Instance.Cam.SkillCam;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (skillVcam != null)
        {
            // 현재 사용중인 스킬이 활성화 되면 스킬카메라와 연결 (나머지 제거)
            camOn = skillVcam.OnSkillCamera;
            camOff = skillVcam.OffSkillCamera;
        }
        else
        {
            skillVcam = GameManager.Instance.Cam.SkillCam;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // 비활성화 시 초기화
        isActivate = false;
    }
    /// <summary>
    /// 스킬이 활성화 됐을 때 초기화 메서드
    /// </summary>
    /// <param name="user">스킬 사용자</param>
    public virtual void OnSKillInitialize(Transform user)
    {
        this.user = user;
    }

    /// <summary>
    /// 스킬 소환했을 때 실행될 메서드(현재: F 키)
    /// </summary>
    public void OnSkill()
    {
        if (!isActivate)
        {
            OnSKillAction();
        }
    }
    /// <summary>
    /// 스킬 소환 했을 때 행동
    /// </summary>
    protected virtual void OnSKillAction()
    {
        camOn?.Invoke();
    }

    /// <summary>
    /// 스킬 발동 했을 때 실행될 메서드(현재: 우클릭)
    /// </summary>
    public void UseSkill()
    {
        if (!isActivate)
        {
            UseSkillAction();
        }
    }
    /// <summary>
    /// 스킬 발동 했을 때 행동
    /// </summary>
    protected virtual void UseSkillAction()
    {
        camOff?.Invoke();
        isActivate = true;
    }
    /// <summary>
    /// 스킬 종료 메서드(현재: X 키)
    /// </summary>
    public void OffSkill()
    {
        OffSKillAction();
    }
    /// <summary>
    /// 스킬 종료 행동
    /// </summary>
    protected virtual void OffSKillAction()
    {
        camOff?.Invoke();
        isActivate = false;
        ReturnToPool();

    }
    /// <summary>
    /// 풀로 돌아갈 때 할 행동
    /// </summary>
    protected override void ReturnAction()
    {
        cancelSkill?.Invoke();
        // 파괴 애니메이션 추가
    }
}
