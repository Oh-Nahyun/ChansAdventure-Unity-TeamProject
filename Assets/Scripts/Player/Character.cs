using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;

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
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 중력
    /// </summary>
    //[Range(-1, 1)]
    //public float gravity = 0.96f;

    /// <summary>
    /// 슬라이드 정도
    /// </summary>
    public float slidePower = 5.0f;

    /// <summary>
    /// 점프 시간 제한
    /// </summary>
    //public float jumpTimeLimit = 4.0f;

    /// <summary>
    /// 점프 시간
    /// </summary>
    //[SerializeField]
    //public float jumpTime;

    /// <summary>
    /// 점프 정도
    /// </summary>
    public float jumpPower = 5.0f;

    /// <summary>
    /// 점프 속도값
    /// </summary>
    //public float jumpVelocity;

    /// <summary>
    /// 점프 중인지 아닌지 확인용 변수
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티 (점프중이 아닐 때)
    /// </summary>
    bool IsJumpAvailable => !isJumping;

    /// <summary>
    /// 주변 시야 버튼이 눌렸는지 아닌지 확인용 변수
    /// </summary>
    public bool isLook = false;

    /// <summary>
    /// 주변 시야 방향 벡터
    /// </summary>
    Vector3 lookVector = Vector3.zero;

    /// <summary>
    /// 주변 시야 카메라
    /// </summary>
    public GameObject cameraRoot;

    /// <summary>
    /// 주변 시야 카메라 회전 정도
    /// </summary>
    public float followCamRotatePower = 5.0f;

    // 애니메이터용 해시값
    //readonly int IsMoveBackHash = Animator.StringToHash("IsMoveBack");
    readonly int IsJumpHash = Animator.StringToHash("IsJump");
    readonly int IsSlideHash = Animator.StringToHash("IsSlide");
    readonly int SpeedHash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;

    // 컴포넌트들
    PlayerinputActions inputActions;
    CharacterController characterController;
    Animator animator;

    private void Awake()
    {
        inputActions = new PlayerinputActions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isJumping = true;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;

        inputActions.Player.MoveModeChange.performed += OnMoveModeChangeInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        /////inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.Slide.performed += OnSlideInput;

        inputActions.Player.LookAround.performed += OnLookInput;
        inputActions.Player.LookAround.canceled += OnLookInput;
    }

    private void OnDisable()
    {
        inputActions.Player.LookAround.canceled -= OnLookInput;
        inputActions.Player.LookAround.performed -= OnLookInput;

        inputActions.Player.Slide.performed -= OnSlideInput;
        /////inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChangeInput;

        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        characterController.Move(Time.fixedDeltaTime * currentSpeed * inputDirection); // 캐릭터의 움직임
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);  // 목표 회전으로 변경
    }

    private void Update()
    {
        LookRotation();
        Jump();
    }

    void SetMoveInput(Vector2 input, bool IsPress)
    {
        // 입력 방향 저장
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;

        // 입력을 시작한 상황
        if (IsPress)
        {
            //// 캐릭터가 지면에 닿았는지 확인
            // if (characterController.isGrounded)

            //// 캐릭터가 뒤로 갈 경우
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    // 기존의 currentSpeed를 저장하고 walkSpeed로 변경
            //    float a = currentSpeed;
            //    currentSpeed = walkSpeed;

            //    animator.SetFloat(SpeedHash, AnimatorWalkSpeed);
            //    animator.SetTrigger(IsMoveBackHash);

            //    // 뒤로 가기를 끝내기 전, currentSpeed를 원래대로 돌려주기
            //    currentSpeed = a;
            //}
            //else if (Input.GetKeyUp(KeyCode.S))
            //{
            //    currentSpeed = 0.0f; // 정지
            //    animator.SetFloat(SpeedHash, AnimatorStopSpeed);
            //}

            // 입력 방향 회전시키기
            Quaternion followCamY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);   // 카메라의 y회전만 따로 추출
            inputDirection = followCamY * inputDirection;                                                   // 입력 방향을 카메라의 y회전과 같은 정도로 회전시키기
            targetRotation = Quaternion.LookRotation(inputDirection);                                       // 회전 저장

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
        Vector3 input = context.ReadValue<Vector2>();
        SetMoveInput(input, !context.canceled);
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
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 점프하는 중이 아닌 경우 => 점프 가능
            isJumping = false;
        }
        else
        {
            // 점프 중인 경우 => 점프 불가능
            isJumping = true;
        }
    }

    void Jump()
    {
        // 점프가 가능한 경우
        if (IsJumpAvailable)
        {
            animator.SetTrigger(IsJumpHash);

            // 위쪽과 앞쪽으로 jumpPower만큼 힘 더하기
            //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //jumpTime -= Time.deltaTime * currentSpeed;
            //jumpTime = Mathf.Clamp(jumpTime, 0.1f, jumpTimeLimit);
            //jumpVelocity = Mathf.Lerp(transform.position.y, 10, Time.deltaTime * jumpPower);
            //inputDirection.y += gravity + jumpVelocity * jumpTime;
        }

        isJumping = true;
    }

    ///// <summary>
    ///// 공격 처리 함수
    ///// </summary>
    //private void OnAttackInput(InputAction.CallbackContext _)
    //{
    //    animator.SetTrigger(IsAttackHash);

    //    // 기본 공격할 동안 Player의 이동이 불가하도록 설정
    //    StopAllCoroutines();
    //    StartCoroutine(StopInput());
    //}

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext context)
    {
        animator.SetTrigger(IsSlideHash);
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLook = true;
            lookVector = context.ReadValue<Vector2>();
        }

        if (!context.performed)
        {
            isLook = false;
        }
    }

    void LookRotation()
    {
        if (!isLook)
            return;

        cameraRoot.transform.localRotation *= Quaternion.AngleAxis(lookVector.x * followCamRotatePower, Vector3.up);
        cameraRoot.transform.localRotation *= Quaternion.AngleAxis(-lookVector.y * followCamRotatePower, Vector3.right);

        var angles = cameraRoot.transform.localEulerAngles;
        angles.z = 0;

        var angle = cameraRoot.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        cameraRoot.transform.localEulerAngles = angles;
        cameraRoot.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);



        //------------------------------------------------------------------------------------------------------------------------


        //Vector2 dir = cameraRoot.transform.position - transform.position; // 카메라에서 플레이어로 가는 방향 벡터 계산
        //dir.y = 0.0f;
        //// transform.LookAt()
        //// barrelBody.forward = dir; // 즉시 바라보기
        //cameraRoot.transform.rotation = Quaternion.Slerp(cameraRoot.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * followCamRotatePower); // 정해진 속도에 맞게 목표 지점 바라보기

        //float angle = Vector3.Angle(cameraRoot.transform.forward, dir); // 사잇각
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
    public IEnumerator StopInput()
    {
        inputActions.Player.Disable();          // Player 액션맵 비활성화
        yield return new WaitForSeconds(4.0f);
        inputActions.Player.Enable();           // Player 액션맵 활성화
    }
}
