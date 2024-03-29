using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 임시
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 2.0f;

    /// <summary>
    /// 물체를 던지는 힘
    /// </summary>
    public float throwPower = 5.0f;
    /// <summary>
    /// 물건을 집을 수 있는 범위(반지름)
    /// </summary>
    public float liftRadius = 0.5f;
    /// <summary>
    /// 물건을 집을 수 있는 범위(높이)
    /// </summary>
    public float pickUpHeightRange = 0.5f;

    /// <summary>
    /// 입력이 있는지 파악용 (true: 입력이 있음)
    /// </summary>
    bool isMoveInput = false;

    /// <summary>
    /// 입력 관련
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 캐릭터 모델링 자식 트랜스폼
    /// </summary>
    Transform character;
    /// <summary>
    /// 오브젝트를 드는 범위용 자식 트랜스폼
    /// </summary>
    Transform pickUpRoot;
    /// <summary>
    /// 카메라 회전용 자식 트랜스폼
    /// </summary>
    Transform cameraRoot;
    /// <summary>
    /// 카메라 회전용 프로퍼티
    /// </summary>
    public Transform CameraRoot
    {
        get => cameraRoot;
    }
    /// <summary>
    /// 플레이어 스킬용
    /// </summary>
    PlayerSkillController skillController;
    /// <summary>
    /// 플레이어 스킬용 프로퍼티
    /// </summary>
    public PlayerSkillController SkillController => skillController;
    /// <summary>
    /// 플레이어 스킬사용 및 오브젝트 관련 손의 위치 추적용 트랜스폼 (플레이어와 동일한 회전값을 가짐 = 정면이 동일)
    /// </summary>
    HandRootTracker handRootTracker;

    /// <summary>
    /// 현재 사용중인 스킬이 있는지 확인 (true: 스킬 사용중)
    /// </summary>
    bool IsSkillOn => SkillController.CurrentOnSkill != null;

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
                onSkillSelect?.Invoke(selectSkill);         // 현재 선택된 스킬을 알림
            }
        }
    }
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
    /// 현재 들고있는 오브젝트 (들고있지 않으면 null)
    /// </summary>
    ReactionObject reaction;

    // 컴포넌트
    PlayerInputActions inputActions;
    Animator animator;

    // 애니메이션 해시
    readonly int Hash_IsMove = Animator.StringToHash("IsMove");
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");

    private void Awake()
    {
        character = transform.GetChild(0);

        inputActions = new PlayerInputActions();
        animator = character.GetComponent<Animator>();                          // 애니메이션은 자식 트랜스폼인 모델에서 처리
        

        skillController = transform.GetComponent<PlayerSkillController>();

        HandRoot handRoot = transform.GetComponentInChildren<HandRoot>();       // 플레이어 손 위치를 찾기 귀찮아서 스크립트 넣어서 찾음
        handRootTracker = transform.GetComponentInChildren<HandRootTracker>();  // 플레이어 손 위치를 추적하는 트랜스폼 => 집어든 오브젝트를 자식으로 놨을 때 정면을 플레이어의 정면으로 맞추기 위해

        pickUpRoot = transform.GetChild(2);

        cameraRoot = transform.GetComponentInChildren<CameraRootMover>().transform;

        rightClick += PickUpObjectDetect;       // 우클릭 = 물건 들기
        onThrow += ThrowObject;                 // 던지기
        onCancel += DropObject;                // 취소

        onPickUp += () => handRootTracker.OnTracking(handRoot.transform);   // 물건을 들면 손위치추적기 동작
        onSkill += () => handRootTracker.OnTracking(handRoot.transform);    // 스킬 사용시 손위치추적기 동작
        onCancel += handRootTracker.OffTracking;                            // 취소시 손위치추적기 정지
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.LeftClick.performed += OnLeftClick;
        inputActions.Player.RightClick.performed += OnRightClick;

        inputActions.Player.OnSkill.performed += OnSkill;
        inputActions.Player.Skill1.performed += OnSkill1;
        inputActions.Player.Skill2.performed += OnSkill2;
        inputActions.Player.Skill3.performed += OnSkill3;
        inputActions.Player.Skill4.performed += OnSkill4;
        inputActions.Player.Skill5.performed += OnSkill5;

        inputActions.Player.Throw.performed += OnThrow;
        inputActions.Player.Cancel.performed += OnCancel;

        inputActions.Player.ScrollY.performed += OnScrollY;
    }



    private void OnDisable()
    {
        inputActions.Player.ScrollY.performed -= OnScrollY;

        inputActions.Player.Cancel.performed -= OnCancel;
        inputActions.Player.Throw.performed -= OnThrow;

        inputActions.Player.Skill5.performed -= OnSkill5;
        inputActions.Player.Skill4.performed -= OnSkill4;
        inputActions.Player.Skill3.performed -= OnSkill3;
        inputActions.Player.Skill2.performed -= OnSkill2;
        inputActions.Player.Skill1.performed -= OnSkill1;
        inputActions.Player.OnSkill.performed -= OnSkill;

        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.LeftClick.performed -= OnLeftClick;

        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
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

    /// <summary>
    /// 스킬을 발동하는 메서드
    /// </summary>
    /// <param name="_"></param>
    private void OnSkill(InputAction.CallbackContext _)
    {
        if (!IsPickUp)
        {
            // 확인 후 재작성
            //switch (selectSkill)
            //{
            //    case SkillName.RemoteBomb:
            //    case SkillName.RemoteBomb_Cube:
            //        IsPickUp = !IsSkillOn;
            //        break;
            //    case SkillName.MagnetCatch:

            //        break;
            //}
            
            IsPickUp = !IsSkillOn;      // 스킬이 현재 사용중이 아니면

            onSkill?.Invoke();
            reaction = SkillController.CurrentOnSkill;  // 손에 드는 오브젝트는 현재 사용중인 스킬
        }
    }

    private void OnSkill1(InputAction.CallbackContext _)
    {
        //if (isSKillMenuOn)
        {
            SelectSkill = SkillName.RemoteBomb;
        }
    }
    private void OnSkill2(InputAction.CallbackContext _)
    {
        //if (isSKillMenuOn)
        {
            SelectSkill = SkillName.RemoteBomb_Cube;
        }
    }
    private void OnSkill3(InputAction.CallbackContext _)
    {
        //if (isSKillMenuOn)
        {
            SelectSkill = SkillName.MagnetCatch;
        }
    }
    private void OnSkill4(InputAction.CallbackContext _)
    {
        //if (isSKillMenuOn)
        {
            SelectSkill = SkillName.IceMaker;
        }
    }
    private void OnSkill5(InputAction.CallbackContext context)
    {
        //if (isSKillMenuOn)
        {
            SelectSkill = SkillName.TimeLock;
        }
    }

    private void OnRightClick(InputAction.CallbackContext _)
    {
        rightClick?.Invoke();
    }

    private void OnLeftClick(InputAction.CallbackContext _)
    {
        leftClick?.Invoke();
    }
    private void OnThrow(InputAction.CallbackContext context)
    {
        onThrow?.Invoke();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        onCancel?.Invoke();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 tempDir = context.ReadValue<Vector2>();
        inputDir.x = tempDir.x;
        inputDir.y = 0f;
        inputDir.z = tempDir.y;
        isMoveInput = !context.canceled;
        animator.SetBool(Hash_IsMove, isMoveInput);
    }

    private void OnScrollY(InputAction.CallbackContext context)
    {
        onScroll?.Invoke(context.ReadValue<float>());
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * inputDir);
        if (inputDir.sqrMagnitude > 0.001f)
        {
            character.rotation = Quaternion.Slerp(character.rotation, Quaternion.LookRotation(inputDir) * transform.rotation, rotateSpeed * Time.deltaTime);
            //character.rotation = Quaternion.LookRotation(inputDir) * transform.rotation;
        }
        if (isMoveInput)
        {
            LookForwardPlayer(Camera.main.transform.forward);
        }
    }

    public void LookForwardPlayer(Vector3 rotate)
    {
        //rotate.x = 0;
        //rotate.z = 0;
        rotate.y = 0;
        transform.forward = rotate;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

#endif
}
