using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerSkills;

public class PlayerSkillRelatedAction : MonoBehaviour, ILifter
{
    /// <summary>
    /// 플레이어 스킬사용 및 오브젝트 관련 손의 위치 추적용 트랜스폼 (플레이어와 동일한 회전값을 가짐 = 정면이 동일)
    /// </summary>
    HandRootTracker handRootTracker;

    public Transform Hand => handRootTracker.transform;

    /// <summary>
    /// 오브젝트를 드는 범위용 자식 트랜스폼
    /// </summary>
    Transform pickUpRoot;

    /// <summary>
    /// 현재 선택된 스킬 (사용시 해당 스킬이 발동됨)
    /// </summary>
    SkillName selectSkill = SkillName.TimeLock;

    /// <summary>
    /// 현재 선택된 스킬용 프로퍼티
    /// </summary>
    SkillName SelectSkill
    {
        get => selectSkill;
        set
        {
            if (selectSkill != value)
            {
                if (reaction != null && reaction.IsSkill && GameManager.Instance.Skill.PlayerSkill.IsUableSkills[(int)value])     // 리모컨폭탄류의 스킬을 들고 있는 경우
                {

                    switch (selectSkill)
                    {
                        case SkillName.RemoteBomb:
                        case SkillName.RemoteBomb_Cube:
                            //Drop();   // 땅에 버리기
                            break;
                        case SkillName.IceMaker:
                        case SkillName.TimeLock:
                            Skill skill = reaction as Skill;
                            skill?.OffSkill();
                            //Drop();
                            break;
                        case SkillName.MagnetCatch: // 마그넷캐치가 활성화 된 상태면 스킬 변경 불가능
                            MagnetCatch magnet = reaction as MagnetCatch;
                            if (magnet != null && magnet.IsActivate)
                            {
                                value = selectSkill;
                            }
                            else if (magnet != null)
                            {
                                magnet.OffSkill();
                                //Drop();
                            }
                            break;
                    }
                }
                selectSkill = value;            // 현재 스킬 설정

                onSkillChange?.Invoke(selectSkill);         // 현재 선택된 스킬을 알림
            }
        }
    }

    public Action<SkillName> onSkillChange;

    /// <summary>
    /// 물건을 집을 수 있는 범위(높이)
    /// </summary>
    public float pickUpHeightRange = 0.5f;

    /// <summary>
    /// 물체를 던지는 힘
    /// </summary>
    public float throwPower = 5.0f;

    /// <summary>
    /// 물건을 집을 수 있는 범위(반지름)
    /// </summary>
    public float liftRadius = 0.5f;

    /// <summary>
    /// 오브젝트를 집었는 지 확인(true: 물건을 듦)
    /// </summary>
    bool isPickUp = false;

    /// <summary>
    /// 오브젝트를 집었는 지 확인용 프로퍼티
    /// </summary>
    public bool IsPickUp
    {
        get => isPickUp;
        private set
        {
            if (isPickUp != value)  // 다른 값일 때만 가능 = 맨손일때만 들 수 있고 들고 있을 때만 내릴 수 있음
            {
                isPickUp = value;
                animator.SetBool(Hash_IsPickUp, isPickUp);
                if(reaction is Skill)
                {
                    animator.SetInteger(Hash_SkillNumber, (int)SelectSkill);
                }
                else
                {
                    animator.SetInteger(Hash_SkillNumber, SkillNumber_None);
                }
                onPickUp?.Invoke(value);
                // 추가: 마그넷 애니메이션 등 다른 애니메이션 if로 구분하기
            }
        }
    }


    /// <summary>
    /// 현재 들고있는 오브젝트 (들고있지 않으면 null)
    /// </summary>
    ReactionObject reaction;

    Animator animator;

    /// <summary>
    /// 오브젝트를 들었을 경우를 알리는 델리게이트
    /// </summary>
    public Action<bool> onPickUp;

    /// <summary>
    /// 손 따라다니는 트랜스폼 이동하기 시작하라고 알리는 델리게이트
    /// </summary>
    public Action onHandRootMove;

    /// <summary>
    /// F : 스킬 사용
    /// </summary>
    public Action onSkill;

    /// <summary>
    /// X : 스킬 취소
    /// </summary>
    public Action onSkillCancel;

    /// <summary>
    /// 우클릭
    /// </summary>
    public Action onSkillInteraction;

    /// <summary>
    /// 특수스킬키를 눌렀을 때 발동되는 델리게이트
    /// </summary>
    public Action<SpecialKey> onSpecialKey;

    // Hashes
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");
    readonly int Hash_SkillNumber = Animator.StringToHash("SkillNumber");
    readonly int Hash_IsSkillUse = Animator.StringToHash("IsSKillUse");
    readonly int Hash_SpecialMotion = Animator.StringToHash("SpecialMotion");

    const int SkillNumber_None = 0;

    private void Awake()
    {
        pickUpRoot = transform.GetChild(3);

        animator = GetComponent<Animator>();                          // 애니메이션은 자식 트랜스폼인 모델에서 처리

        HandRoot handRoot = transform.GetComponentInChildren<HandRoot>();       // 플레이어 손 위치를 찾기 귀찮아서 스크립트 넣어서 찾음
        handRootTracker = transform.GetComponentInChildren<HandRootTracker>();  // 플레이어 손 위치를 추적하는 트랜스폼 => 집어든 오브젝트를 자식으로 놨을 때 정면을 플레이어의 정면으로 맞추기 위해
        PlayerSkillController skillController = GetComponent<PlayerSkillController>();
        skillController.rightClick += PickUpObjectDetect;       // 우클릭 = 물건 들기
        skillController.rightClick += onSkillInteraction;

        skillController.onThrow += ThrowMotion;                 // 던지기

        skillController.onCancel += () =>
        {
            // 던지고 있는 도중에는 취소가 안되도록 설정
            if (!isThrowing)
            {
                onSkillCancel?.Invoke();
                Drop();
                handRootTracker.OffTracking();
            }
        };
        

        skillController.onSkillActive += OnSkillAction;

        skillController.onSkillChange += (skillName) => SelectSkill = skillName;

        onHandRootMove += () => handRootTracker.OnTracking(handRoot.transform);    // 스킬 사용시 손위치추적기 동작

        int enumLenght = Enum.GetValues(typeof(PlayerSkills.SpecialKey)).Length;
        for (int i = 0; i < enumLenght; i++)
        {
            int index = i;      // 반복문 안에서 람다식을 사용할 때 변수값이 이상해 지는 것 방지하기위해 반복문 안에 변수 선언 후 사용
            skillController.onSpecialKey[i] += () => {
                animator.SetInteger(Hash_SpecialMotion, index);
                onSpecialKey?.Invoke((SpecialKey)index);

            };
        }
    }

    /// <summary>
    /// 들 수 있는 오브젝트 파악하는 메서드
    /// (상호작용키 실행시 행동)
    /// </summary>
    void PickUpObjectDetect()
    {
        if (!IsPickUp)      // 빈 손이면
        {
            Vector3 heightPoint = pickUpRoot.position;
            heightPoint.y += pickUpHeightRange;
            Collider[] hit = Physics.OverlapCapsule(pickUpRoot.position, heightPoint, liftRadius);  // 픽업 범위 파악해서 체크한 뒤

            for (int i = 0; i < hit.Length; i++)        // 범위 안의 모든 물체 중
            {
                ReactionObject detecedObj = hit[i].transform.GetComponent<ReactionObject>();
                if (detecedObj != null)   // 반응형 오브젝트가 있고
                {
                    detecedObj.TryPickUp(transform);    // 물체를 들어봤을 때
                    if (reaction != null)    // 물체가 들리면
                    {
                        break;              // 첫번째로 감지된 물체를 들고 종료
                    }
                }
            }
        }
        else if (IsPickUp && reaction != null)      // 이미 물건을 들고 있는 경우
        {
            bool onSkill = reaction is Skill;
            if (onSkill)                            // 스킬이면
            {
                switch (SelectSkill)
                {
                    case SkillName.RemoteBomb:      // 리모컨폭탄만 떨어뜨리기
                    case SkillName.RemoteBomb_Cube:
                        IsPickUp = false;
                        reaction.TryDrop();
                        reaction = null;
                        break;
                }
            }
            else                                    // 스킬이 아니면 물체 떨어뜨리기
            {
                IsPickUp = false;
                reaction.TryDrop();
                reaction = null;
            }
        }
        // 상호작용 키 들었을 때 행동 야숨에서 확인하기
    }


    bool isThrowing = false;

    /// <summary>
    /// 오브젝트 던지는 모션을 취하는 함수
    /// </summary>
    void ThrowMotion()
    {
        if (IsPickUp && reaction != null && reaction.IsThrowable)
        {
            animator.SetTrigger(Hash_Throw);
            isThrowing = true;
        }
    }

    /// <summary>
    /// 오브젝트 던지는 실제 행동을 하는 함수 (애니메이션 이벤트에서 동작)
    /// </summary>
    void ThrowEvent()
    {
        reaction.TryThrow(throwPower, transform);
        IsPickUp = false;
        isThrowing = false;
        reaction = null;
    }

    /// <summary>
    /// 물체 들기
    /// </summary>
    public void PickUp(ReactionObject pickUpObject)
    {
        if (!IsPickUp)
        {
            onHandRootMove?.Invoke();
            reaction = pickUpObject;
            reaction.transform.rotation = Quaternion.identity;  // 물건의 회전값 없애기 = 플레이어의 정면과 맞추기
            IsPickUp = true;
        }
    }

    /// <summary>
    /// 물체 내려놓기
    /// (취소키 실행시 행동)
    /// </summary>
    public void Drop()
    {
        if (reaction != null)
        {
            reaction.TryDrop();
            IsPickUp = false;
            reaction = null;
        }

    }

    void OnSkillAction()
    {
        if (!IsPickUp)
        {
            onHandRootMove?.Invoke();
            onSkill.Invoke();
        }
    }

    public void SetSkillUseAnimation(bool isUse)
    {
        animator.SetBool(Hash_IsSkillUse, isUse);
    }

    public void SetSelectSkill(SkillName skillName)
    {
       SelectSkill = skillName;
    }
}
