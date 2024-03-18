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
    PlayerinputActions inputController;
    CharacterController characterController;
    Animator animator;

    private void Awake()
    {
        inputController = new PlayerinputActions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputController.Player.Enable();
        inputController.Player.Move.performed += OnMoveInput;
        inputController.Player.Move.canceled += OnMoveInput;
        inputController.Player.MoveModeChange.performed += OnMoveModeChangeInput;
        inputController.Player.Jump.performed += OnJumpInput;
        inputController.Player.Attack.performed += OnAttackInput;
        inputController.Player.Slide.performed += OnSlideInput;

        //inputActions.Player.LookAround.performed += OnLookInput;
        //inputActions.Player.LookAround.canceled += OnLookInput;
    }

    private void OnDisable()
    {
        //inputActions.Player.LookAround.canceled -= OnLookInput;
        //inputActions.Player.LookAround.performed -= OnLookInput;

        inputController.Player.Slide.performed -= OnSlideInput;
        inputController.Player.Attack.performed -= OnAttackInput;
        inputController.Player.Jump.performed -= OnJumpInput;
        inputController.Player.MoveModeChange.performed -= OnMoveModeChangeInput;
        inputController.Player.Move.canceled -= OnMoveInput;
        inputController.Player.Move.performed -= OnMoveInput;
        inputController.Player.Disable();
    }

    private void FixedUpdate()
    {
        characterController.Move(Time.fixedDeltaTime * currentSpeed * inputDirection); // 캐릭터의 움직임
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);  // 목표 회전으로 변경
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
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y회전만 따로 추출
            inputDirection = camY * inputDirection;                     // 입력 방향을 카메라의 y회전과 같은 정도로 회전시키기
            targetRotation = Quaternion.LookRotation(inputDirection);   // 목표 회전 저장

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
    private void OnJumpInput(InputAction.CallbackContext _)
    {
        // 점프가 가능한 경우
        //if (IsJumpAvailable)
        {
            animator.SetTrigger(IsJumpHash);

            // 위쪽과 앞쪽으로 jumpPower만큼 힘 더하기
            //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

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
    IEnumerator StopInput()
    {
        inputController.Player.Disable();          // Player 액션맵 비활성화
        yield return new WaitForSeconds(4.0f);
        inputController.Player.Enable();           // Player 액션맵 활성화
    }
}
