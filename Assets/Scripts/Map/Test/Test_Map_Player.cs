using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Map_Player : MonoBehaviour
{
    PlayerinputActions inputActions;
    public CanvasGroup map_CanvasGroup;
    public LineRenderer linePrefab;

    public float speed = 5f;
    Vector3 moveVector = Vector3.zero;


    void Awake()
    {
        inputActions = new PlayerinputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Open_Map.performed += OnOpenMap;
        inputActions.Player.Open_Map.canceled += OnOpenMap;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Open_Map.canceled -= OnOpenMap;
        inputActions.Player.Open_Map.performed -= OnOpenMap;
        inputActions.Player.Disable();
        
    }

    void Update()
    {
        transform.position += moveVector * speed * Time.deltaTime;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        moveVector = new Vector3(inputVector.x, 0, inputVector.y);
    }

    private void OnOpenMap(InputAction.CallbackContext obj)
    {
        // 임시 온오프
        if(map_CanvasGroup.alpha == 1f)
        {
            map_CanvasGroup.alpha = 0f;
        }
        else
        {
            map_CanvasGroup.alpha = 1f;
        }
    }
}
