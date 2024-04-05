using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// 플레이어 스크립트
/// </summary>
public class Player : MonoBehaviour
{
    PlayerController controller;
    
    /// <summary>
    /// PlayerSKills를 받기위한 프로퍼티
    /// </summary>
    public PlayerSkills Skills => gameObject.GetComponent<PlayerSkills>();

    Animator animator;
    CharacterController characterController;

    #region PlayerMove
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
    #endregion

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        // exception
        if (cameraRoot == null)
        {
            Debug.LogError("CameraRoot가 비어있습니다. CameraRoot Prefab 오브젝트를 넣어주세요 ( PlayerLookVCam 스크립트 있는 오브젝트 )");
        }

        controller.onMove += OnMove;
        controller.onMoveModeChagne += OnMoveModeChange;
        controller.onLook += OnLookAround;
        controller.onSlide += OnSlide;
        controller.onJump += OnJump;
    }

    void Update()
    {        
        LookRotation();
        Jump();
    }

    void FixedUpdate()
    {
        characterController.Move(Time.fixedDeltaTime * currentSpeed * inputDirection); // 캐릭터의 움직임
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);  // 목표 회전으로 변경
    }

    /// <summary>
    /// Get Player input Values
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="isMove">check press button ( wasd )</param>
    void OnMove(Vector2 input, bool isMove)
    {
        // 입력 방향 저장
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;

        // 입력을 시작한 상황
        if (isMove)
        {
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
    /// 이동 모드 변경 함수
    /// </summary>
    private void OnMoveModeChange()
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
    /// Check Look Around
    /// </summary>
    /// <param name="lookInput">lookInput value</param>
    /// <param name="isLookingAround">true : , false : No input Value</param>
    void OnLookAround(Vector2 lookInput, bool isLookingAround)
    {
        if (isLookingAround)
        {
            isLook = true;
            lookVector = lookInput;
        }

        if (!isLookingAround)
        {
            isLook = false;
        }
    }

    /// <summary>
    /// Change Player Movemode
    /// </summary>
    /// <param name="mode">MoveMode</param>
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
    /// Player Camera Rotation
    /// </summary>
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
    }
    private void OnJump(bool isJump)
    {
        if (isJump)
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
        }

        isJumping = true;
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlide()
    {
        animator.SetTrigger(IsSlideHash);
    }

    /// <summary>
    /// 캐릭터의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void CharacterColliderEnable()
    {
        characterController.enabled = true;
    }

    /// <summary>
    /// 캐릭터의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void CharacterColliderDisable()
    {
        characterController.enabled = false;
    }

    // 
    public void LookForwardPlayer(Vector3 rotate)
    {
        //rotate.x = 0;
        //rotate.z = 0;
        rotate.y = 0;
        transform.forward = rotate;
    }
}