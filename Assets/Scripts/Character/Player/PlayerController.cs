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
public class PlayerController : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    // movment delegate
    public Action<Vector2, bool> onMove;
    public Action onMoveRunMode;
    public Action onMoveWalkMode;
    public Action<Vector2, bool> onLook;
    public Action<bool> onJump;
    public Action<bool> onSlide;
    public Action onSkillModeChange;

    // behavior delegate
    public Action onInteraction;
    public Action onInventoryOpen;
    public Action onMapOpen;

    // 컴포넌트
    Weapon weapon;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();
        weapon = GetComponent<Weapon>();
    }

    void OnEnable()
    {
        playerInputAction.Player.Enable();

        // Player Movement
        playerInputAction.Player.Move.performed += OnMoveInput;
        playerInputAction.Player.Move.canceled += OnMoveInput;
        playerInputAction.Player.LookAround.performed += OnLookInput;
        playerInputAction.Player.LookAround.canceled += OnLookInput;
        playerInputAction.Player.Jump.performed += OnJumpInput;
        playerInputAction.Player.Slide.performed += OnSlideInput;
        playerInputAction.Player.MoveModeChange.performed += OnMoveRunModeInput;
        playerInputAction.Player.MoveModeChange.canceled += OnMoveWalkModeInput;

        // Player Inventory
        playerInputAction.Player.Open_Inventory.performed += OnOpenInventory;
        playerInputAction.Player.Get_Item.performed += OnGetItem;

        // Map
        playerInputAction.Player.Open_Map.performed += OnOpenMap;

        //playerInputAction.Player.ActiveSkillMode.performed += OnSkillModeChange;
    }

    void OnDisable()
    {
        //playerInputAction.Player.ActiveSkillMode.performed -= OnSkillModeChange;

        // Map
        playerInputAction.Player.Open_Map.performed -= OnOpenMap;

        // Player Inventory
        playerInputAction.Player.Open_Inventory.performed -= OnOpenInventory;
        playerInputAction.Player.Get_Item.performed -= OnGetItem;

        // Player Movement
        playerInputAction.Player.MoveModeChange.canceled -= OnMoveWalkModeInput;
        playerInputAction.Player.MoveModeChange.performed -= OnMoveRunModeInput;
        playerInputAction.Player.Slide.performed -= OnSlideInput;
        playerInputAction.Player.Jump.performed -= OnJumpInput;
        playerInputAction.Player.LookAround.canceled -= OnLookInput;
        playerInputAction.Player.LookAround.performed -= OnLookInput;
        playerInputAction.Player.Move.canceled -= OnMoveInput;
        playerInputAction.Player.Move.performed -= OnMoveInput;

        playerInputAction.Player.Disable();
    }

    #region Player Movement Input

    /*private void OnSkillModeChange(CallbackContext context)
    {
        bool isActiveSelf = playerInputAction.Skill.enabled;
        if (!isActiveSelf)
        {
            playerInputAction.Skill.Enable();
            playerInputAction.Weapon.Disable();
        }
        else
        {
            playerInputAction.Skill.Disable();
            playerInputAction.Weapon.Enable();
        }

        Debug.Log($"스킬 모드 활성화 여부 : {playerInputAction.Skill.enabled}");
    }*/

    /// <summary>
    /// 이동 처리 함수
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context.ReadValue<Vector2>(), !context.canceled);
    }

    /// <summary>
    /// 달리기 모드 함수
    /// </summary>
    private void OnMoveRunModeInput(CallbackContext _)
    {
        onMoveRunMode?.Invoke();
    }

    /// <summary>
    /// 걷기 모드 함수
    /// </summary>
    private void OnMoveWalkModeInput(CallbackContext _)
    {
        onMoveWalkMode?.Invoke();
    }

    /// <summary>
    /// 카메라 회전 입력 함수
    /// </summary>
    private void OnLookInput(InputAction.CallbackContext context)
    {
        if (weapon.IsZoomIn)
        {
            // 카메라가 줌을 당길 경우 => 카메라 회전 중지
            onLook?.Invoke(context.ReadValue<Vector2>(), !context.performed);
        }
        else
        {
            // 그 외의 경우 => 주변 시야 확인 가능
            onLook?.Invoke(context.ReadValue<Vector2>(), context.performed);
        }
    }

    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (weapon.IsZoomIn)
        {
            // 카메라가 줌을 당길 경우 => 점프 불가능
            onJump?.Invoke(!context.performed);
        }
        else
        {
            onJump?.Invoke(context.performed);
        }
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    private void OnSlideInput(InputAction.CallbackContext context)
    {
        if (weapon.IsZoomIn)
        {
            // 카메라가 줌을 당길 경우 => 슬라이드 불가능
            onSlide?.Invoke(!context.performed);
        }
        else
        {
            onSlide?.Invoke(context.performed);
        }
    }
    #endregion

    #region Player Inventory

    /// <summary>
    /// 인벤토리 열 때 실행되는 인풋 함수
    /// </summary>
    private void OnOpenInventory(InputAction.CallbackContext _)
    {
        onInventoryOpen?.Invoke();        
    }

    /// <summary>
    /// 아이템을 획득하는 인풋 ( F Key )
    /// </summary>
    private void OnGetItem(InputAction.CallbackContext context)
    {
        onInteraction?.Invoke();
    }
    #endregion

    #region Etc
    /// <summary>
    /// 맵 키는 함수 ( M key )
    /// </summary>
    /// <param name="context"></param>
    private void OnOpenMap(InputAction.CallbackContext context)
    {
        onMapOpen?.Invoke();
    }

    #endregion

    /// <summary>
    /// 입력 처리 불가 처리 코루틴
    /// </summary>
    /// <param name="clipPath">애니메이션 클립의 리소스 경로</param>
    public IEnumerator StopInput(string clipPath)
    {
        float waitingTime = GetAnimationLegth(clipPath);    // 애니메이션 클립 로드

        if (clipPath == weapon.clipPath_None)
        {
            waitingTime *= 0.5f;
        }
        else if (clipPath == weapon.clipPath_SwordSheath2)
        {
            // 검 공격 시간 고려
            waitingTime += GetAnimationLegth(weapon.clipPath_Sword1);

            //Debug.Log(GetAnimationLegth(weapon.clipPath_Sword1)); // 1.5초
            //Debug.Log(GetAnimationLegth(weapon.clipPath_Sword2)); // 2.43초

            //if (randomAttackSelector.AttackModeHash == 0) // 일반 공격
            //{
            //    waitingTime += GetAnimationLegth(weapon.clipPath_Sword1);
            //}
            //else if (randomAttackSelector.AttackModeHash == 1) // Critical 공격
            //{
            //    waitingTime += GetAnimationLegth(weapon.clipPath_Sword2);
            //}
        }

        playerInputAction.Player.Disable();                 // Player 액션맵 비활성화
        yield return new WaitForSeconds(waitingTime);       // waitingTime만큼 딜레이
        playerInputAction.Player.Enable();                  // Player 액션맵 활성화
    }

    /// <summary>
    /// 애니메이션 클립 로드 및 재생 시간 출력 함수
    /// </summary>
    /// <param name="clipPath">애니메이션 클립의 리소스 경로</param>
    /// <returns>애니메이션 재생 시간</returns>
    public float GetAnimationLegth(string clipPath)
    {
        AnimationClip clip = Resources.Load<AnimationClip>(clipPath);
        if (clip != null)
        {
            return clip.length;
        }
        else
        {
            Debug.Log("애니메이션 재생 시간을 출력할 수 없습니다.");
            return -1.0f;
        }
    }
}