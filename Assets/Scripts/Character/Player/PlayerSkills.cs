using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static System.Collections.Specialized.BitVector32;

/// <summary>
/// 플레이어의 스킬내용만 받는 스크립트
/// </summary>
public class PlayerSkills : MonoBehaviour
{
    // components
    PlayerSkillController skillController;

    ReactionObject currentOnSkill;
    public ReactionObject CurrentOnSkill => currentOnSkill;

    RemoteBomb remoteBomb;
    RemoteBombCube remoteBombCube;
    MagnetCatch magnetCatch;

    Animator animator;

    /// <summary>
    /// 플레이어 스킬사용 및 오브젝트 관련 손의 위치 추적용 트랜스폼 (플레이어와 동일한 회전값을 가짐 = 정면이 동일)
    /// </summary>
    HandRootTracker handRootTracker;

    Transform handRootTrackerTransform;
    public Transform HandRoot => handRootTrackerTransform;

    /// <summary>
    /// 오브젝트를 드는 범위용 자식 트랜스폼
    /// </summary>
    Transform pickUpRoot;

    /// <summary>
    /// 현재 들고있는 오브젝트 (들고있지 않으면 null)
    /// </summary>
    ReactionObject reaction;

    // Delegates
    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;

    // properties
    SkillName currentSkill = SkillName.RemoteBomb;

    /// <summary>
    /// 현재 선택된 스킬 (사용시 해당 스킬이 발동됨)
    /// </summary>
    SkillName selectSkill = SkillName.RemoteBomb;

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
                switch (selectSkill)
                {
                    case SkillName.RemoteBomb:
                    case SkillName.RemoteBomb_Cube:
                    case SkillName.IceMaker:
                    case SkillName.TimeLock:
                        if (reaction != null && reaction.transform.CompareTag("Skill"))     // 리모컨폭탄류의 스킬을 들고 있는 경우
                        {
                            DropObject();   // 땅에 버리기
                        }
                        break;
                    case SkillName.MagnetCatch: // 마그넷캐치가 활성화 된 상태면 스킬 변경 불가능
                        value = selectSkill;
                        break;
                }
                selectSkill = value;            // 현재 스킬 설정
                Debug.Log($"스킬 [{selectSkill}]로 설정");
                //
                skillController.onSkillSelect?.Invoke(selectSkill);         // 현재 선택된 스킬을 알림
            }
        }
    }

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
    bool IsPickUp
    {
        get => isPickUp;
        set
        {
            if (isPickUp != value)  // 다른 값일 때만 가능 = 맨손일때만 들 수 있고 들고 있을 때만 내릴 수 있음
            {
                isPickUp = value;
                animator.SetBool(Hash_IsPickUp, isPickUp);
                // 추가: 마그넷 애니메이션 등 다른 애니메이션 if로 구분하기
            }
        }
    }

    /// <summary>
    /// 현재 사용중인 스킬이 있는지 확인 (true: 스킬 사용중)
    /// </summary>
    bool IsSkillOn => CurrentOnSkill != null;

    // Hashes
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");

    // ===

    // 입력용 델리게이트
    /// <summary>
    /// 우클릭: 상호작용
    /// </summary>
    public Action rightClick;
    /// <summary>
    /// 좌클릭: 공격 (스킬에서 사용 x)
    /// </summary>
    public Action leftClick;
    /// <summary>
    /// 휠: 마그넷캐치 연결시 앞뒤이동
    /// </summary>
    public Action<float> onScroll;
    /// <summary>
    /// z: 던지기
    /// </summary>
    Action onThrow;
    /// <summary>
    /// f: 스킬 사용
    /// </summary>
    public Action onSkill;
    /// <summary>
    /// x: 취소 (야숨 행동 파악중)
    /// </summary>
    public Action onCancel;

    /// <summary>
    /// 선택된 스킬이 바뀌었음을 알리는 델리게이트 (F1:리모컨폭탄 F2:리모컨폭탄큐브 F3:마그넷캐치 F4:아이스메이커 F5:타임록)
    /// </summary>
    public Action<SkillName> onSkillSelect;

    /// <summary>
    /// 오브젝트를 들었을 경우를 알리는 델리게이트
    /// </summary>
    public Action onPickUp;


    // ===

    void Awake()
    {
        skillController = GetComponent<PlayerSkillController>();
        animator = GetComponent<Animator>();

        if (skillController == null)
        {
            Debug.LogError("skillController가 존재하지 않습니다.");
        }
        else
        {
            skillController.onSkillSelect += ConnectSkill; // onSkillSelect가 같은 스크립트 내에서 처리됨

            //skillController.onSkillActive += () => onSKillAction?.Invoke(); // 
            skillController.onSkillActive += OnSkill; // 
            //skillController.rightClick += () => useSkillAction?.Invoke();
            skillController.onCancel += CancelSkill;

            //onSKillAction += OnSkill;

            animator = GetComponent<Animator>();                          // 애니메이션은 자식 트랜스폼인 모델에서 처리

            HandRoot handRoot = transform.GetComponentInChildren<HandRoot>();       // 플레이어 손 위치를 찾기 귀찮아서 스크립트 넣어서 찾음
            handRootTracker = transform.GetComponentInChildren<HandRootTracker>();  // 플레이어 손 위치를 추적하는 트랜스폼 => 집어든 오브젝트를 자식으로 놨을 때 정면을 플레이어의 정면으로 맞추기 위해
            handRootTrackerTransform = handRoot.transform; // 24.04.05

            pickUpRoot = transform.GetChild(2);

            //cameraRoot = transform.GetComponentInChildren<CameraRootMover>().transform;

            rightClick += PickUpObjectDetect;       // 우클릭 = 물건 들기
            onThrow += ThrowObject;                 // 던지기
            onCancel += DropObject;                // 취소

            onPickUp += () => handRootTracker.OnTracking(handRoot.transform);   // 물건을 들면 손위치추적기 동작
            onSkill += () => handRootTracker.OnTracking(handRoot.transform);    // 스킬 사용시 손위치추적기 동작
            onCancel += handRootTracker.OffTracking;



            skillController.onRemoteBomb += OnRemoteBomb;  // skill 1
            skillController.onRemoteBomb_Cube += OnRemoteBomb_Cube;// skill 2
            skillController.onMagnetCatch += OnMagnetCatch;// skill 3
            skillController.onIceMaker += OnIceMaker;// skill 4
            skillController.onTimeLock += OnTimeLock; // skill 5
            skillController.onThrow += ThrowObject;
}
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
                    remoteBomb = Factory.Instance.GetRemoteBomb(); // 팩토리에서 리모컨폭탄 가져온 뒤 리모컨 폭탄 변수에 설정
                    currentOnSkill = remoteBomb;                        // 현재 사용중인 스킬은 리모컨폭탄
                }
                else
                {
                    CancelSkill();          // 리모컨폭탄이 소환되어 있으면 터지면서 스킬 종료
                }

                break;
            case SkillName.RemoteBomb_Cube:
                if (remoteBombCube == null)
                {
                    //Debug.Log("실행 : 리모컨 폭탄 큐브");
                    remoteBombCube = Factory.Instance.GetRemoteBombCube();
                    currentOnSkill = remoteBombCube;
                }
                else
                {
                    CancelSkill();
                }
                break;
            case SkillName.MagnetCatch:
                if (magnetCatch == null)
                {
                    //Debug.Log("실행 : 마그넷 캐치");
                    magnetCatch = Factory.Instance.GetMagnetCatch();
                    currentOnSkill = magnetCatch;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

        ConnectSkill(currentSkill);

        if (currentOnSkill != null)
        {
            currentOnSkill.PickUp(HandRoot);

            //currentOnSkill.transform.SetParent(HandRoot);
            //currentOnSkill.transform.position = HandRoot.position;
            //currentOnSkill.transform.forward = player.transform.forward;
        }

        onSKillAction?.Invoke();
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
                currentOnSkill = remoteBomb;
                if (remoteBomb != null)
                {
                    onSKillAction = remoteBomb.OnSkill;
                    useSkillAction = remoteBomb.UseSkill;
                    offSkillAction = remoteBomb.OffSkill;
                }

                break;
            case SkillName.RemoteBomb_Cube:
                currentOnSkill = remoteBombCube;
                if (remoteBombCube != null)
                {
                    onSKillAction = remoteBombCube.OnSkill;
                    useSkillAction = remoteBombCube.UseSkill;
                    offSkillAction = remoteBombCube.OffSkill;
                }
                break;
            case SkillName.MagnetCatch:
                currentOnSkill = magnetCatch;
                if (magnetCatch != null)
                {
                    onSKillAction = magnetCatch.OnSkill;
                    useSkillAction = magnetCatch.UseSkill;
                    offSkillAction = magnetCatch.OffSkill;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

    }

    /// <summary>
    /// 들 수 있는 오브젝트 파악하는 메서드
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
                reaction = hit[i].transform.GetComponent<ReactionObject>();
                if (reaction != null && reaction.IsThrowable)   // 들 수 있는 첫번째 오브젝트를 들고 종료
                {
                    PickUpObject();
                    break;
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
                        reaction.Drop();
                        reaction = null;
                        break;
                }
            }
            else                                    // 스킬이 아니면 물체 떨어뜨리기
            {
                IsPickUp = false;
                reaction.Drop();
                reaction = null;
            }
        }
        // 상호작용 키 들었을 때 행동 야숨에서 확인하기
    }

    /// <summary>
    /// 오브젝트 던지는 메서드
    /// </summary>
    void ThrowObject()
    {
        if (IsPickUp && reaction != null)
        {
            animator.SetTrigger(Hash_Throw);
            reaction.Throw(throwPower, transform);
            IsPickUp = false;
            reaction = null;
        }
    }

    /// <summary>
    /// 오브젝트를 드는 메서드
    /// </summary>
    void PickUpObject()
    {
        IsPickUp = true;
        onPickUp?.Invoke();
        reaction.PickUp(handRootTracker.transform);         // 물건 들기
        reaction.transform.rotation = Quaternion.identity;  // 물건의 회전값 없애기 = 플레이어의 정면과 맞추기
    }

    /// <summary>
    /// 취소 행동용 메서드 (아직 확인중)
    /// </summary>
    void DropObject()
    {
        // 취소키 야숨에서 확인하기
        /*if(IsPickUp && reaction != null)
        {
            IsPickUp = false;
            reaction.Drop();
            reaction = null;
        }*/
        if (IsSkillOn && reaction != null)          // 스킬이 사용중이면 모두 취소
        {
            IsPickUp = false;
            reaction.Drop();
            reaction = null;
        }

    }

    void CancelSkill()
    {
        offSkillAction?.Invoke();
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                remoteBomb = null;
                currentOnSkill = null;
                break;
            case SkillName.RemoteBomb_Cube:
                remoteBombCube = null;
                currentOnSkill = null;
                break;
            case SkillName.MagnetCatch:
                magnetCatch = null;
                currentOnSkill = null;
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }

    private void OnTimeLock()
    {
        SelectSkill = SkillName.TimeLock;
    }

    private void OnIceMaker()
    {
        SelectSkill = SkillName.IceMaker;
    }

    private void OnMagnetCatch()
    {
        SelectSkill = SkillName.MagnetCatch;
    }

    private void OnRemoteBomb_Cube()
    {
        SelectSkill = SkillName.RemoteBomb_Cube;
    }

    private void OnRemoteBomb()
    {
        SelectSkill = SkillName.RemoteBomb;
    }
}
