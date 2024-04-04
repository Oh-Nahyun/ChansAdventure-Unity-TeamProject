using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// 플레이어 인풋만 받는 스크립트
/// </summary>
public class Test_99_PlayerController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    // movment delegate
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChagne;
    public Action<Vector2, bool> onLook;
    public Action onSlide;
    public Action<bool> onJump;

    // behavior delegate


    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        playerInputAction.Enable();
        // Player Movement
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;
        playerInputAction.Player.LookAround.performed += OnLookInput;
        playerInputAction.Player.LookAround.canceled += OnLookInput;
        playerInputAction.Player.Jump.performed += OnJumpInput;
        playerInputAction.Player.Slide.performed += OnSlideInput;
        playerInputAction.Player.MoveModeChange.performed += OnMoveModeChangeInput;
    }

    void OnDisable()
    {
        // Player Movement
        playerInputAction.Player.MoveModeChange.performed -= OnMoveModeChangeInput;
        playerInputAction.Player.Slide.performed -= OnSlideInput;
        playerInputAction.Player.Jump.performed -= OnJumpInput;
        playerInputAction.Player.LookAround.canceled -= OnLookInput;
        playerInputAction.Player.LookAround.performed -= OnLookInput;
        playerInputAction.Player.Move.canceled -= OnMoveInput;
        playerInputAction.Player.Move.performed -= OnMoveInput;

        playerInputAction.Player.Disable();
    }
    #region Player Movement Input
    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    /// <summary>
    /// 이동 모드 변경 함수
    /// </summary>
    private void OnMoveModeChangeInput(CallbackContext _)
    {
        onMoveModeChagne?.Invoke();
    }

    /// <summary>
    /// 카메라 회전 입력 함수
    /// </summary>
    private void OnLookInput(InputAction.CallbackContext context)
    {
        onLook?.Invoke(context.ReadValue<Vector2>(), context.performed);
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext context)
    {
        onSlide?.Invoke();
    }

    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        onJump?.Invoke(context.performed);
    }
    #endregion

    /// <summary>
    /// 입력 처리 불가 처리 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator StopInput()
    {
        playerInputAction.Player.Disable();          // Player 액션맵 비활성화
        yield return new WaitForSeconds(4.0f);
        playerInputAction.Player.Enable();           // Player 액션맵 활성화
    }
}