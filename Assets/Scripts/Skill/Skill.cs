using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum SkillName
{
    RemoteBomb = 0,
    RemoteBomb_Cube,
    MagnetCatch,
    IceMaker,
    TimeLock
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
    /// 쿨타임 (외부에서 사용 x)
    /// </summary>
    public float cooltime = 1.0f;
    /// <summary>
    /// 쿨타임 (get, 외부 확인용)
    /// </summary>
    public float Cooltime => cooltime;

    /// <summary>
    /// 스킬 이미지 (외부에서 사용 x)
    /// </summary>
    public Sprite skillSprite;

    /// <summary>
    /// 스킬 이미지 (get, 외부 확인용)
    /// </summary>
    public Sprite SkillImage => skillSprite;

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
    public Action<SkillName> cancelSkill;
    /// <summary>
    /// 쿨타임이 돌기 시작하라고 알리는 델리게이트
    /// </summary>
    public Action<SkillName> cooltimeReset;
    /// <summary>
    /// 스킬 모션 변경용 델리게이트
    /// </summary>
    public Action<bool> onMotionChange;

    /// <summary>
    /// 카메라 중앙
    /// </summary>
    protected readonly Vector3 Center = new Vector3(0.5f, 0.5f, 0.0f);

    protected Crosshair crosshair;

    Action OnCrosshair;
    Action OffCrosshair;

    protected override void Awake()
    {
        base.Awake();
        rigid.isKinematic = true;           // 스킬의 경우 Kinematic으로 지정, 리모컨폭탄류는 던지면 변경됨
        reactionType |= ReactionType.Skill; // 반응 타입에 스킬 추가 (별다른 반응은 없고 구별용)
        //cooltime = maxCooltime;
        isRecycle = true;
    }

    protected override void OnEnable()
    {
        StopAllCoroutines();
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

        if (crosshair == null)
        {
            crosshair = FindAnyObjectByType<Crosshair>();
            if (crosshair == null)
            {
                Debug.LogWarning("크로스헤어 찾지 못함");
            }
            else
            {
                OnCrosshair += crosshair.Open;
                OffCrosshair += crosshair.Close;
            }
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
        OnCrosshair?.Invoke();
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
        cooltimeReset?.Invoke(skillName);
        OffCrosshair?.Invoke();
        isActivate = true;
        cooltime = 0;
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
        OffCrosshair?.Invoke();
        isActivate = false;
        ReturnToPool();

    }
    /// <summary>
    /// 풀로 돌아갈 때 할 행동
    /// </summary>
    protected override void ReturnAction()
    {
        cancelSkill?.Invoke(skillName);
        cancelSkill = null;
        base.ReturnAction();
        // 파괴 애니메이션 추가
    }

    public virtual void InputSpecialKey(PlayerSkills.SpecialKey key)
    {
    }
}
