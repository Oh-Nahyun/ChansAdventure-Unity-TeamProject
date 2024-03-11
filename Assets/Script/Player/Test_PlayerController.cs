using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어를 관리하는 스크립트
/// </summary>
public class Test_PlayerController : MonoBehaviour
{
    PlayerInputActionFPS inputAction;

    /// <summary>
    /// 마우스 이동으로 카메라 회전
    /// </summary>
    RotateMouse rotateToMouse;

    /// <summary>
    /// 키보드 입력으로 플레이어 이동, 점프
    /// </summary>
    PlayerMovementController movement;

    /// <summary>
    /// 플레이어의 이동속도 정보
    /// </summary>
    Status status;

    /// <summary>
    /// 플레이어 에니메이션 제어 하기 위해 찾을 컴포넌트 변수
    /// </summary>
    PlayerAnimatorController animator;

    /// <summary>
    /// 무기를 사용해서 공격을 제어하기 위해 컴포넌트를 찾기 위한 변수
    /// </summary>
    Weapon weapon;

    /// <summary>
    /// 움직이는 값 벡터로 변경을 ㅜ이한ㅇ 변수
    /// </summary>
    Vector2 movePos;

    /// <summary>
    /// 현재 달리기를 시작했는지 확인용
    /// </summary>
    float run;

    /// <summary>
    /// 현재 공격을 시작했는지 확인용
    /// </summary>
    bool isAttack;

    /// <summary>
    /// 현재 에이밍을 하는 중인지 확인용
    /// </summary>
    bool isAim;

    /// <summary>
    /// 현재 장전을 시작했는지 확인용
    /// </summary>
    bool isReload;
    
    /// <summary>
    /// 마우스 X축
    /// </summary>
    float mouseX;
    
    /// <summary>
    /// 마우스 Y축
    /// </summary>
    float mouseY;

    /// <summary>
    /// 살아있는지 확인용
    /// </summary>
    bool isAlive = true;


    private void Awake()
    {
        // 마우스 커서를 보이지 않게 설정
        Cursor.visible = false;
        // 현재 위치에 고정
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateMouse>();            // RotateMouse 컴포넌트 찾기
        movement = GetComponent<PlayerMovementController>();    // PlayerMovementController 컴포넌트 찾기
        status = GetComponent<Status>();                        // Status 컴포넌트 찾기
        animator = GetComponent<PlayerAnimatorController>();    // PlayerAnimatorController 컴포넌트 찾기
        weapon = GetComponentInChildren<Weapon>();              // 자식 오브젝트 안에 있는 Weapon 컴포넌트 찾기

        inputAction = new PlayerInputActionFPS();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnWalkStart;
        inputAction.Player.Move.canceled += OnWalkStart;
        inputAction.Player.Run.performed += OnRunStart;
        inputAction.Player.Run.canceled += OnRunStart;
        inputAction.Player.Jump.performed += Onjump;
        inputAction.Player.Jump.canceled += Onjump;
        inputAction.Player.Fire.performed += OnFireStart;
        inputAction.Player.Fire.canceled += OnFireEnd;
        inputAction.Player.Aim.performed += OnAimStart;
        inputAction.Player.Aim.canceled += OnAimEnd;
        inputAction.Player.Reload.performed += OnReloadStart;
        inputAction.Player.Reload.canceled += OnReloadEnd;
    }

    
    private void OnDisable()
    {
        inputAction.Player.Reload.canceled -= OnReloadEnd;
        inputAction.Player.Reload.performed -= OnReloadStart;
        inputAction.Player.Aim.canceled -= OnAimEnd;
        inputAction.Player.Aim.performed -= OnAimStart;
        inputAction.Player.Fire.canceled -= OnFireEnd;
        inputAction.Player.Fire.performed -= OnFireStart;
        inputAction.Player.Jump.canceled -= Onjump;
        inputAction.Player.Jump.performed -= Onjump;
        inputAction.Player.Run.canceled -= OnRunStart;
        inputAction.Player.Run.performed -= OnRunStart;
        inputAction.Player.Move.canceled -= OnWalkStart;
        inputAction.Player.Move.performed -= OnWalkStart;
        inputAction.Player.Disable();
    }
    private void OnReloadEnd(InputAction.CallbackContext context)
    {
        isReload = false;
    }

    private void OnReloadStart(InputAction.CallbackContext context)
    {
        isReload = true;
    }


    private void OnAimEnd(InputAction.CallbackContext context)
    {
        isAim = false;
    }

    private void OnAimStart(InputAction.CallbackContext context)
    {
        isAim = true;
    }

    private void OnFireEnd(InputAction.CallbackContext context)
    {
        isAttack = false;
    }

    private void OnFireStart(InputAction.CallbackContext context)
    {
        isAttack = true;
    }

    private void Onjump(InputAction.CallbackContext context)
    {
        movement.jump(); // 점프 실행
    }

    private void OnRunStart(InputAction.CallbackContext context)
    {
        run = context.ReadValue<float>();
    }

    private void OnWalkStart(InputAction.CallbackContext context)
    {
       movePos  = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        UpdateRotate();         // 마우스 이동
        UpdateMove();           // 플레이어 이동 실행
        UpdateWeaponAction(); ; // 공격 실행
    }


    /// <summary>
    /// 마우스의 X,Y의 이동값을 받기 위한 함수
    /// </summary>
    void UpdateRotate()
    {
        if(isAlive)
        {
            mouseX = Input.GetAxis("Mouse X"); // 마우스 X의 이동 값
            mouseY = Input.GetAxis("Mouse Y"); // 마우스 Y의 이동 값


            rotateToMouse.UpdateRotate(mouseX, mouseY); // 이동한 마우스의 X,Y의 값을 매개변수로 넣기
        }
        else
        {
            rotateToMouse.UpdateRotate(0, 0); // 이동한 마우스의 X,Y의 값을 매개변수로 넣기
            // 마우스 커서를 보이게 설정
            Cursor.visible = true;
            // 마우스 고정 풀기
            Cursor.lockState = CursorLockMode.None;
        }
            
    }

    /// <summary>
    /// 플레이어 이동 값 받아 이동 실제 이동 시키는 함수
    /// </summary>
    void UpdateMove()
    {
        float x = movePos.x;
        float z = movePos.y;

        if (x != 0 || z != 0) // 이동 중일 때(걷기 아니면 뛰기)
        {
            bool isRun = false; //달리기 버튼을 누르면 ture 아니면 false

            // 옆이나 뒤로 이동할 떄는 달릴 수 없음
            if (z > 0) // 앞으로 이동 중일 때
            {
                isRun = run == 1 ? true : false;
            }
            // isRun == true 이면 RunSpeed(뛰는 속도) // isRun == false 이면 WalkSpeed (걷는 속도)
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            // 달리는 중이면 애니메이터 MoveSpeed 해쉬값 1(뛰는 애니메이션)
            // 걷는 중이면 애니메이터 MoveSpeed 해쉬값 0.5(걷는 애니메이션)
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
        }
        else // 멈춰 있을 때
        {
            movement.MoveSpeed = 0; // 이동속도 0
            animator.MoveSpeed = 0; // 애니메이터 MoveSpeed 해쉬값 0(Idle 애니메이션)
        }

        movement.MoveTo(new Vector3(x, 0, z));  // 받은 이동 값에 따라 플레이어 이동
    }

    /// <summary>
    /// 플레이어의 행동을 제어하는 함수
    /// </summary>
    void UpdateWeaponAction()
    {
        if (isAttack == true) // 마우스 버튼을 누르면
        {
            weapon.StartWeaponAction(); // 공격 시작 함수 실행
        }
        else if (isAttack == false) // 마우스 버튼을 떼면
        {
            weapon.StopWeaponAction();  // 공격 중지 함수 실행
        }

        if (isAim == true) // 마우스 오른쪽 버튼 누르면 
        {
            weapon.StartWeaponAction(1);    // 에임 모드 시작
        }
        else if (isAim == false) // 마우스 오른쪽 버튼 뗴면
        {
            weapon.StopWeaponAction(1);     // 에임 모드 중지
        }

        if (isReload == true) // R키 누르면
        {
            weapon.StartReload();       // 재장전 함수 실행
        }
    }

    /// <summary>
    /// 플레이어 데미지 받을 때 실행할 함수
    /// </summary>
    /// <param name="damage">받은 데미지 </param>
    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        if (isDie)
        {
            GameOverPanel gameOver = FindAnyObjectByType<GameOverPanel>();
            gameOver.GameOverUI();
            isAlive = false; // 죽은거 표시
            inputAction.Disable(); // 인풋 시스템 뺴기
            Debug.Log("플레이어 죽음");
        }
    }
}
