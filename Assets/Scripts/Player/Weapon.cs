using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 무기 모드
    /// </summary>
    enum WeaponMode
    {
        None = 0,   // 무기 없음
        Sword,      // 칼
        Bow         // 활
    }

    /// <summary>
    /// 현재 무기 모드
    /// </summary>
    WeaponMode currentWeaponMode = WeaponMode.None;

    /// <summary>
    /// 캐릭터의 오른손
    /// </summary>
    //Transform rightHand;

    /// <summary>
    /// [무기1] 칼
    /// </summary>
    Transform swordWeapon;

    /// <summary>
    /// [무기2] 활
    /// </summary>
    Transform bowWeapon;

    // 컴포넌트들
    PlayerinputActions inputActions;
    Animator animator;

    private void Awake()
    {
        inputActions = new PlayerinputActions();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //rightHand = GameObject.Find("Character1_RightHand").transform;
        swordWeapon = GameObject.FindWithTag("Sword").transform;
        bowWeapon = GameObject.FindWithTag("Bow").transform;
        ShowWeapon(false, false);
    }

    private void OnEnable()
    {
        inputActions.Weapon.Enable();
        inputActions.Weapon.Attack.performed += OnAttackInput;
        inputActions.Weapon.Change.performed += OnChangeInput;

        //inputActions.Weapon.Load.performed += OnLoadInput;
    }

    private void OnDisable()
    {
        //inputActions.Weapon.Load.performed -= OnLoadInput;

        inputActions.Weapon.Change.performed -= OnChangeInput;
        inputActions.Weapon.Attack.performed -= OnAttackInput;
        inputActions.Weapon.Disable();
    }

    /// <summary>
    /// 무기에 따른 공격 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnAttackInput(InputAction.CallbackContext context)
    {
        // animator
    }

    /// <summary>
    /// 무기를 바꾸는 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnChangeInput(InputAction.CallbackContext context)
    {
        if (currentWeaponMode == WeaponMode.None)
        {
            // 무기를 들고 있지 않는 경우 => 칼을 들도록 한다.
            currentWeaponMode = WeaponMode.Sword;
            Debug.Log("WeaponMode_Change : None >> Sword");
        }
        else if (currentWeaponMode == WeaponMode.Sword)
        {
            // 칼을 무기로 사용하고 있던 경우 => 활을 들도록 한다.
            currentWeaponMode = WeaponMode.Bow;
            Debug.Log("WeaponMode_Change : Sword >> Bow");
        }
        else if (currentWeaponMode == WeaponMode.Bow)
        {
            // 활을 무기로 사용하고 있던 경우 => 무기를 넣도록 한다.
            currentWeaponMode = WeaponMode.None;
            Debug.Log("WeaponMode_Change : Bow >> None");
        }

        ChangeWeapon(currentWeaponMode);
    }

    /// <summary>
    /// 무기 모드에 따라 보여줄 무기이 변경되는 함수
    /// </summary>
    /// <param name="mode"></param>
    void ChangeWeapon(WeaponMode mode)
    {
        switch (mode)
        {
            case WeaponMode.None:
                ShowWeapon(false, false);
                break;
            case WeaponMode.Sword:
                ShowWeapon(true, false);
                break;
            case WeaponMode.Bow:
                ShowWeapon(false, true);
                break;
        }
    }

    /// <summary>
    /// 무기를 보여줄지 말지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여주고, false면 안보여준다.</param>
    public void ShowWeapon(bool isSwordShow = false, bool isBowShow = false)
    {
        swordWeapon.gameObject.SetActive(isSwordShow);
        bowWeapon.gameObject.SetActive(isBowShow);
    }
}
