using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    #region  PlayerMovement Values

    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    //Vector3 inputDirection = Vector3.zero;

    /// <summary>
    /// 이동 방향 (1 : 전진, -1 : 후진, 0 : 정지)
    /// </summary>
    float moveDirection = 0.0f;

    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 7.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    public float currentSpeed = 0.0f;

    /// <summary>
    /// 이동 모드
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // 걷기
        Run         // 달리기
    }

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Walk;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;        // 상태 변경
            if (currentSpeed > 0.0f)        // 이동 중인지 아닌지 확인
            {
                // 이동 중이면 모드에 맞게 속도와 애니메이션 변경
                MoveSpeedChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// 회전 방향 (1 : 우회전, -1 : 좌회전, 0 : 정지)
    /// </summary>
    float rotateDirection = 0.0f;

    /// <summary>
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    //Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 점프 정도
    /// </summary>
    public float jumpPower = 5.0f;

    /// <summary>
    /// 점프 중인지 아닌지 확인용 변수
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티 (점프중이 아닐 때)
    /// </summary>
    bool IsJumpAvailable => !isJumping;

    // 애니메이터용 해시값
    readonly int IsMoveBackHash = Animator.StringToHash("IsMoveBack");
    readonly int IsJumpHash = Animator.StringToHash("IsJump");
    readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    readonly int IsSlideHash = Animator.StringToHash("IsSlide");
    readonly int SpeedHash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;

    // 컴포넌트들
    PlayerinputActions inputActions;
    //CharacterController characterController;
    Animator animator;
    Rigidbody rigid;

    #endregion

    #region Player_Skills values

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

    // 애니메이션 해시
    readonly int Hash_IsMove = Animator.StringToHash("IsMove");
    readonly int Hash_IsPickUp = Animator.StringToHash("IsPickUp");
    readonly int Hash_Throw = Animator.StringToHash("Throw");

    /// <summary>
    /// 현재 들고있는 오브젝트 (들고있지 않으면 null)
    /// </summary>
    ReactionObject reaction;

    #endregion

    #region LifeCycle Methods
    private void Awake()
    {
        // Player Input
        inputActions = new PlayerinputActions();
        //characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        // Player Skills
        character = transform.GetChild(0);

        inputActions = new PlayerinputActions();
        animator = character.GetComponent<Animator>();                          // 애니메이션은 자식 트랜스폼인 모델에서 처리


        skillController = transform.GetComponent<PlayerSkillController>();

        HandRoot handRoot = transform.GetComponentInChildren<HandRoot>();       // 플레이어 손 위치를 찾기 귀찮아서 스크립트 넣어서 찾음
        handRootTracker = transform.GetComponentInChildren<HandRootTracker>();  // 플레이어 손 위치를 추적하는 트랜스폼 => 집어든 오브젝트를 자식으로 놨을 때 정면을 플레이어의 정면으로 맞추기 위해

        pickUpRoot = transform.GetChild(2);

        //cameraRoot = transform.GetComponentInChildren<CameraRootMover>().transform;

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

        // player input Action
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChangeInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.Slide.performed += OnSlideInput;
        //inputActions.Player.LookAround.performed += OnLookInput;
        //inputActions.Player.LookAround.canceled += OnLookInput;

        // player skill Action
        inputActions.Skill.Enable();

        inputActions.Skill.OnSkill.performed += OnSkill;
        inputActions.Skill.Skill1.performed += OnSkill1;
        inputActions.Skill.Skill2.performed += OnSkill2;
        inputActions.Skill.Skill3.performed += OnSkill3;
        inputActions.Skill.Skill4.performed += OnSkill4;
        inputActions.Skill.Skill5.performed += OnSkill5;

        inputActions.Skill.Throw.performed += OnThrow;
        inputActions.Skill.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        // player skill Action
        inputActions.Skill.Cancel.performed -= OnCancel;
        inputActions.Skill.Throw.performed -= OnThrow;

        inputActions.Skill.Skill5.performed -= OnSkill5;
        inputActions.Skill.Skill4.performed -= OnSkill4;
        inputActions.Skill.Skill3.performed -= OnSkill3;
        inputActions.Skill.Skill2.performed -= OnSkill2;
        inputActions.Skill.Skill1.performed -= OnSkill1;
        inputActions.Skill.OnSkill.performed -= OnSkill;

        inputActions.Skill.Enable();

        // player input Action

        //inputActions.Player.LookAround.canceled -= OnLookInput;
        //inputActions.Player.LookAround.performed -= OnLookInput;
        inputActions.Player.Slide.performed -= OnSlideInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChangeInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;


        //inputActions.Player.RightClick.performed -= OnRightClick;
        //inputActions.Player.LeftClick.performed -= OnLeftClick;

        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();

        //characterController.Move(Time.deltaTime * currentSpeed * inputDirection); // 캐릭터의 움직임
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  // 목표 회전으로 변경
    }

    #endregion

    #region PlayerMovement Method

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * moveDirection * transform.forward);
    }

    void Rotate()
    {
        //Quaternion rotate = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection, transform.up);
        rigid.MoveRotation(rigid.rotation * rotate);
    }

    void SetMoveInput(Vector2 input, bool IsMove)
    {
        // 입력 방향 저장
        rotateDirection = input.x;
        moveDirection = input.y;

        // 입력을 시작한 상황
        if (IsMove)
        {
            // 이동 모드 변경
            MoveSpeedChange(CurrentMoveMode);
        }

        // 입력을 끝낸 상황
        else
        {
            currentSpeed = 0.0f; // 정지
            animator.SetFloat(SpeedHash, AnimatorStopSpeed);
        }
    }

    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        SetMoveInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    /// <summary>
    /// 이동 모드 변경 함수
    /// </summary>
    private void OnMoveModeChangeInput(InputAction.CallbackContext _)
    {
        if (CurrentMoveMode == MoveMode.Walk)
        {
            CurrentMoveMode = MoveMode.Run;
        }
        else
        {
            CurrentMoveMode = MoveMode.Walk;
        }
    }

    /// <summary>
    /// 모드에 따라 이동 속도를 변경하는 함수
    /// </summary>
    /// <param name="mode">설정된 모드</param>
    void MoveSpeedChange(MoveMode mode)
    {
        // 이동 모드에 따라 속도와 애니메이션 변경
        switch (mode)
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(SpeedHash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(SpeedHash, AnimatorRunSpeed);
                break;
        }
    }

    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext _)
    {
        // 점프가 가능한 경우
        //if (IsJumpAvailable)
        {
            animator.SetTrigger(IsJumpHash);

            // 위쪽과 앞쪽으로 jumpPower만큼 힘 더하기
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // 점프했다고 표시
            //isJumping = true;
        }
    }

    /// <summary>
    /// 공격 처리 함수
    /// </summary>
    private void OnAttackInput(InputAction.CallbackContext _)
    {
        animator.SetTrigger(IsAttackHash);

        // 기본 공격할 동안 Player의 이동이 불가하도록 설정
        StopAllCoroutines();
        StartCoroutine(StopInput());
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext _)
    {
        animator.SetTrigger(IsSlideHash);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isJumping = false;
    //    }
    //}

    /// <summary>
    /// 입력 처리 불가 처리 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator StopInput() // 24.04.01
    {
        inputActions.Player.Disable();          // Player 액션맵 비활성화
        yield return new WaitForSeconds(4.0f);
        inputActions.Player.Enable();           // Player 액션맵 활성화
    }

    #endregion

    #region Player skills methods

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
    public void LookForwardPlayer(Vector3 rotate)
    {
        //rotate.x = 0;
        //rotate.z = 0;
        rotate.y = 0;
        transform.forward = rotate;
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

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

#endif

}