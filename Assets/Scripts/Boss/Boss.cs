using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boss : MonoBehaviour
{
    BossInputActions inputActions;
    Rigidbody rb;
    BossAttackArea bossAttackArea;
    Animator animator;

    float moveFB = 0.0f;
    float moveLR = 0.0f;
    float moveSpeed = 5.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputActions = new BossInputActions();
        rb = GetComponent<Rigidbody>();
        bossAttackArea = GetComponentInChildren<BossAttackArea>(true);
    }

    private void OnEnable()
    {
        inputActions.Boss.Enable();
        inputActions.Boss.Move.performed += OnMove;
        inputActions.Boss.Move.canceled += OnMove;
        inputActions.Boss.Clow.performed += OnClow;
        inputActions.Boss.Bite.performed += OnBite;
    }

    private void OnDisable()
    {
        inputActions.Boss.Move.canceled -= OnMove;
        inputActions.Boss.Move.performed -= OnMove;
        inputActions.Boss.Clow.performed -= OnClow;
        inputActions.Boss.Bite.performed -= OnBite;
        inputActions.Boss.Disable();
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        SetInput(obj.ReadValue<Vector2>(), !obj.canceled);
    }

    private void OnClow(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Clow");
    }

    private void OnBite(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Bite");
    }

    public void OnAttackArea()
    {
        if(bossAttackArea != null)
        {
            bossAttackArea.Activate();
        }
    }

    public void OffAttackArea()
    {
        if(bossAttackArea != null)
        {
            bossAttackArea.Deactivate();
        }
    }

    private void SetInput(Vector2 input, bool isMove)
    {
        moveLR = input.x;
        moveFB = input.y;
    }

    

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * moveFB * transform.forward);
        rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * moveLR * transform.right);
    }
    protected enum BossState
    {
        Wait = 0,   // ´ë±â
        Bite,       // ¹°±â
        Breath,     // ºê·¹½º
        Clow,       // ÇÓÄû±â
        Dead        // »ç¸Á
    }

    BossState state = BossState.Wait;

    protected BossState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch(state)
                {
                    case BossState.Wait:
                        break;
                    case BossState.Bite:
                        break;
                    case BossState.Breath:
                        break;
                    case BossState.Clow:
                        break;
                    case BossState.Dead:
                        break;
                }
            }
        }

    }

    protected float hp = 10000.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(State != BossState.Dead && hp <= 0)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke(hp / MaxHP);
        }
    }

    public float maxHP = 10000.0f;
    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }

    void Die()
    {

    }
}
