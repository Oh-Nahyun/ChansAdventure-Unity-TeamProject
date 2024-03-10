using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TestPlayer : MonoBehaviour
{

    /// <summary>
    /// 현재 속도
    /// </summary>
    float currentSpeed = 5.0f;

    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y는 무조건 바닥 높이

    /// <summary>
    /// 캐릭터 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    // 입력용 인풋 액션
    PlayerInputActions inputActions;

    GameManager gameManager;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Use.performed += OnUse;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUse;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * currentSpeed * inputDirection);   // 좀 더 수동
        //characterController.SimpleMove(currentSpeed * inputDirection);            // 좀 더 자동
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  // 목표 회전으로 변경
        if (!(inputDirection.x == 0 && inputDirection.z == 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDirection), Time.deltaTime * turnSpeed);
        }
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();

        inputDirection.x = input.x;     // 입력 방향 저장
        inputDirection.y = 0;
        inputDirection.z = input.y;

    }

    private void OnUse(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
       
    }


}
