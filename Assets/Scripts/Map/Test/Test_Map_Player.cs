using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Map_Player : MonoBehaviour
{
    PlayerinputActions inputActions;
    public CanvasGroup map_CanvasGroup;


    void Awake()
    {
        inputActions = new PlayerinputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Open_Map.performed += OnOpenMap;
        inputActions.Player.Open_Map.canceled += OnOpenMap;
    }

    void OnDisable()
    {
        inputActions.Player.Open_Map.canceled -= OnOpenMap;
        inputActions.Player.Open_Map.performed -= OnOpenMap;
        inputActions.Player.Disable();
        
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
