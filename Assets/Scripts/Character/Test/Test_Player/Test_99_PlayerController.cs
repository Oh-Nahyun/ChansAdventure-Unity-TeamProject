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

    //
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChagne;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        playerInputAction.Enable();
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;
        playerInputAction.Player.MoveModeChange.performed += OnMoveModeChangeInput;
    }

    void OnDisable()
    {
        playerInputAction.Player.MoveModeChange.performed -= OnMoveModeChangeInput;
        playerInputAction.Player.Move.canceled -= OnMoveInput;
        playerInputAction.Player.Move.performed -= OnMoveInput;
        playerInputAction.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnMoveModeChangeInput(CallbackContext _)
    {
        onMoveModeChagne?.Invoke();
    }
}
