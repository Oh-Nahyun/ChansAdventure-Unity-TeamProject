using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y는 무조건 바닥 높이

    /// <summary>
    /// 걷는 속도
    /// </summary>
    float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    float runSpeed = 5.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float currentSpeed = 0.0f;

    /// <summary>
    /// 이동 모드
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // 걷기 모드
        Run         // 달리기 모드
    }

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Run;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;    // 상태 변경
            if (currentSpeed > 0.0f)     // 이동 중인지 아닌지 확인
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
    /// 캐릭터 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    // 컴포넌트들
    PlayerinputActions inputActions;
    Animator animator;
    CharacterController characterController;

    // 애니메이터용 해시값
    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int SpeedHash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;

    private void Awake()
    {
        inputActions = new PlayerinputActions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Attack.performed += OnAttackInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDirection);   // 좀 더 수동

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  // 목표 회전으로 변경
    }

    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        inputDirection.x = input.x;     // 입력 방향 저장
        inputDirection.y = 0;
        inputDirection.z = input.y;

        animator.SetTrigger(IsMoveHash);

        if (!context.canceled)
        {
            // 눌려진 상황(입력을 시작한 상황)

            // 입력 방향 회전 시키기
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y회전만 따로 추출
            inputDirection = camY * inputDirection;     // 입력 방향을 카메라의 y회전과 같은 정도로 회전 시키기
            targetRotation = Quaternion.LookRotation(inputDirection);   // 목표 회전 저장

            // 이동 모드 변경
            MoveSpeedChange(CurrentMoveMode);
        }
        else
        {
            // 입력을 끝낸 상황
            currentSpeed = 0.0f; // 정지
            animator.SetFloat(SpeedHash, AnimatorStopSpeed);
        }
    }

    /// <summary>
    /// 이동 모드 변경 함수
    /// </summary>
    private void OnMoveModeChangeInput()
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
        switch (mode) // 이동 모드에 따라 속도와 애니메이션 변경
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

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        
    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {
        
    }
}
