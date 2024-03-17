using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputBase : MonoBehaviour
{
    TestInputAction actions;

    void Awake()
    {
        actions = new TestInputAction();
    }

    void OnEnable()
    {
        actions.TestInput.Enable();
        actions.TestInput.Key1.performed += OnKey1Input;
        actions.TestInput.Key1.canceled += OnKey1Input;
        actions.TestInput.Key2.performed += OnKey2Input;
        actions.TestInput.Key2.canceled += OnKey2Input;
        actions.TestInput.Key3.performed += OnKey3Input;
        actions.TestInput.Key3.canceled += OnKey3Input;
        actions.TestInput.Key4.performed += OnKey4Input;
        actions.TestInput.Key4.canceled += OnKey4Input;
        actions.TestInput.Key5.performed += OnKey5Input;
        actions.TestInput.Key5.canceled += OnKey5Input;
    }
    protected virtual void OnKey1Input(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnKey2Input(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnKey4Input(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnKey3Input(InputAction.CallbackContext context)
    {
        
    }
    protected virtual void OnKey5Input(InputAction.CallbackContext context)
    {
        
    }


    void OnDisable()
    {
        actions.TestInput.Key1.performed -= OnKey1Input;
        actions.TestInput.Key1.canceled -= OnKey1Input;
        actions.TestInput.Key2.performed -= OnKey2Input;
        actions.TestInput.Key2.canceled -= OnKey2Input;
        actions.TestInput.Key3.performed -= OnKey3Input;
        actions.TestInput.Key3.canceled -= OnKey3Input;
        actions.TestInput.Key4.performed -= OnKey4Input;
        actions.TestInput.Key4.canceled -= OnKey4Input;
        actions.TestInput.Key5.performed -= OnKey5Input;
        actions.TestInput.Key5.canceled -= OnKey5Input;
        actions.TestInput.Disable();
    }
}
