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
    /// [무기1] 칼
    /// </summary>
    Transform swordWeapon;

    /// <summary>
    /// [무기2-1] 활
    /// </summary>
    Transform bowWeapon;

    /// <summary>
    /// [무기2-2] 화살
    /// </summary>
    //Transform arrowWeapon;

    /// <summary>
    /// 캐릭터가 활을 장비했는지 알기 위한 변수
    /// </summary>
    public bool IsBowEquip = false;

    /// <summary>
    /// 캐릭터의 활에 화살이 장전되었는지 알기 위한 변수
    /// </summary>
    public bool IsArrowEquip = false;

    /// <summary>
    /// 카메라 줌 설정 변수
    /// </summary>
    public bool IsZoomIn = false;

    /// <summary>
    /// 공격 버튼을 누르고 있는지 확인하는 변수
    /// </summary>
    public bool isPressed = false;

    /// <summary>
    /// 누르고 있는 시간값
    /// </summary>
    float pressTime = 0f;

    // 애니메이션 클립의 리소스 경로
    public string clipPath_None = "PlayerAnimations/N_Attack";
    public string clipPath_Sword1 = "PlayerAnimations/S_Attack1";
    public string clipPath_Sword2 = "PlayerAnimations/S_Attack2";
    public string clipPath_SwordSheath2 = "PlayerAnimations/S_Sheath2";
    readonly string clipPath_Bow = "PlayerAnimations/B_Attack";

    // 애니메이터용 해시값
    readonly int IsAttackHash = Animator.StringToHash("IsAttack");
    readonly int IsSwordHash = Animator.StringToHash("IsSword");
    readonly int IsBowHash = Animator.StringToHash("IsBow");
    //readonly int CriticalHitHash = Animator.StringToHash("CriticalHit");
    readonly int UseWeaponHash = Animator.StringToHash("UseWeapon");
    readonly int HaveArrowHash = Animator.StringToHash("HaveArrow");
    readonly int ZoomInHash = Animator.StringToHash("ZoomIn");

    // 컴포넌트들
    PlayerController playerController;
    PlayerinputActions inputActions;
    Animator animator;
    Player player;
    Sword sword;
    Bow bow;
    Arrow arrow;
    ArrowFirePoint arrowFirePoint;
    //PlayerFollowVCam vcam;

    GameManager gameManager;

    /// <summary>
    /// 화살이 있는 아이템 슬롯
    /// </summary>
    InventorySlot arrowSlot;

    /// <summary>
    /// 화살개수
    /// </summary>
    uint arrowCount = 0; // -> public float arrowCount = 0; 

    /// <summary>
    /// 화살개수 확인하기 위한 프로퍼티
    /// </summary>
    public uint ArrowCount => arrowCount;

    /// <summary>
    /// 화살 프리팹
    /// </summary>
    GameObject arrowPrefab;

    /// <summary>
    /// 활 줌했을 때 실행하는 델리게이트 
    /// Delegate executed when on zoomIn
    /// </summary>
    public Action<bool> OnBowZoomIn;

    /// <summary>
    /// 활 줌 아웃 했을 때 실행하는 델리게이트 
    /// Delegate executed when on zoomOut
    /// </summary>
    public Action OnBowZoomOut;

    /// <summary>
    /// 공격 버튼을 떼었을 때 실행하는 델리게이트
    /// </summary>
    public Action OnReleaseAttackButton;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputActions = new PlayerinputActions();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        sword = GetComponentInChildren<Sword>();
        bow = GetComponentInChildren<Bow>();
        arrow = GetComponentInChildren<Arrow>();
        arrowFirePoint = GetComponentInChildren<ArrowFirePoint>();
        //vcam = FindAnyObjectByType<PlayerFollowVCam>();
    }

    private void Start()
    {
        //rightHand = GameObject.Find("Character1_RightHand").transform;
        //leftHand = GameObject.Find("Character1_LeftHand").transform;
        //swordWeapon = GameObject.FindWithTag("Sword").transform;
        //bowWeapon = GameObject.FindWithTag("Bow").transform;
        //arrowWeapon = GameObject.FindWithTag("Arrow").transform;
        //arrowWeapon.gameObject.SetActive(false);
        //arrow.CloseArrow();

        //swordWeapon = GameObject.FindWithTag("Sword").transform; // 아이템 장착을 위한 주석처리
        //bowWeapon = GameObject.FindWithTag("Bow").transform; // 아이템 장착을 위한 주석처리
        //ShowWeapon(false, false); // 아이템 장착을 위한 주석처리

        player.OnEquipWeaponItem += OnEquipWeapon;
        player.OnUnEquipWeaponItem += OnUnEquipWeapon;
    }

    private void OnEnable()
    {
        inputActions.Weapon.Enable();
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Weapon.Attack.performed += OnAttackInput;
        inputActions.Weapon.Attack.canceled += OnAttackInputRelease;
        inputActions.Weapon.Zoom.performed += OnZoomIn;
        inputActions.Weapon.Zoom.canceled += OnZoomOut;

        inputActions.Weapon.NormalMode.performed += OnNormalModeInput;
        inputActions.Weapon.SwordMode.performed += OnSwordModeInput;
        inputActions.Weapon.BowMode.performed += OnBowModeInput;

        // inputActions.Weapon.Change.performed += OnChangeInput;
        // inputActions.Weapon.Load.performed += OnLoadInput;
    }

    private void OnDisable()
    {
        // inputActions.Weapon.Load.performed -= OnLoadInput;
        // inputActions.Weapon.Change.performed -= OnChangeInput;

        inputActions.Weapon.BowMode.performed -= OnBowModeInput;
        inputActions.Weapon.SwordMode.performed -= OnSwordModeInput;
        inputActions.Weapon.NormalMode.performed -= OnNormalModeInput;

        inputActions.Weapon.Zoom.canceled -= OnZoomOut;
        inputActions.Weapon.Zoom.performed -= OnZoomIn;
        inputActions.Weapon.Attack.performed -= OnAttackInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Weapon.Disable();
    }

    private void Update()
    {
        if(isPressed)   // 공격키 누르는 중, pressed attack key 
        {
            pressTime += Time.deltaTime;
            pressTime = Mathf.Clamp(pressTime, 0f, 3f);
        }
        else            // 공격키 땠을 때, release attack Key
        {
            UpdateArrow();
            animator.SetBool(ZoomInHash, false);        // 활 시위 해제  
            animator.SetBool(HaveArrowHash, false);
            IsArrowEquip = false;
        }
    }

    /// <summary>
    /// 무기 모드에 따른 공격 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnAttackInput(InputAction.CallbackContext context)
    {
        if (player.SkillRelatedAction.IsPickUp || player.isTalk || player.IsAnyUIPanelOpened) // 물건을 들고 있거나 대화중일 때 입력 막기
            return;

        isPressed = true;

        animator.SetTrigger(IsAttackHash);

        if (currentWeaponMode == WeaponMode.None)
        {
            animator.SetBool(UseWeaponHash, false);

            // 공격할 동안 Player의 이동이 불가하도록 설정
            StopAllCoroutines();
            StartCoroutine(playerController.StopInput(clipPath_None));
        }
        else // 무기 모드가 Sword 또는 Bow인 경우
        {
            animator.SetBool(UseWeaponHash, true);

            if (currentWeaponMode == WeaponMode.Sword)
            {
                animator.SetTrigger(IsSwordHash);
                ShowWeapon(true, false);

                // 공격할 동안 Player의 이동이 불가하도록 설정
                StopAllCoroutines();
                StartCoroutine(playerController.StopInput(clipPath_SwordSheath2));
            }
            else if (currentWeaponMode == WeaponMode.Bow)
            {
                animator.SetTrigger(IsBowHash);
                ShowWeapon(false, true);

                // 갱신한 화살이 존재할 경우 ( 1개 이상 ) >> 화살 자동 장전 후 공격
                if (ArrowCount > 0) //
                {
                    OnLoad();               // reload
                    LoadArrowAfter();       // Shot  
                }

                // 인벤토리에 화살 개수가 0이고, 화살 장전이 안되어있는 경우 >> 활 자체 공격
                if (ArrowCount == 0 && !IsArrowEquip) //
                {
                    animator.SetBool(HaveArrowHash, false);
                    Debug.Log("***** 인벤토리 내 보유하고 있는 화살이 없습니다.");

                    // 공격할 동안 Player의 이동이 불가하도록 설정
                    StopAllCoroutines();
                    StartCoroutine(playerController.StopInput(clipPath_Bow));
                }
            }
        }

        SetBasedamage();
    }

    /// <summary>
    /// 공격 버튼을 땠을 때 실행하는 함수
    /// </summary>
    private void OnAttackInputRelease(InputAction.CallbackContext context)
    {
        isPressed = false;
        OnReleaseAttackButton?.Invoke();
        OnBowZoomOut?.Invoke();
    }

    /// <summary>
    /// 검 모드 처리용 함수
    /// </summary>
    private void OnSwordModeInput(InputAction.CallbackContext _)
    {
        if (IsZoomIn)
            return;

        currentWeaponMode = WeaponMode.Sword;
        QuickEquipWeapon(currentWeaponMode);
    }

    /// <summary>
    /// 활 모드 처리용 함수
    /// </summary>
    private void OnBowModeInput(InputAction.CallbackContext _)
    {
        if (IsZoomIn)
            return;

        currentWeaponMode = WeaponMode.Bow;
        QuickEquipWeapon(currentWeaponMode);
    }

    /// <summary>
    /// 노멀 모드 처리용 함수
    /// </summary>
    private void OnNormalModeInput(InputAction.CallbackContext _)
    {
        if (IsZoomIn)
            return;

        currentWeaponMode = WeaponMode.None;
        ChangeWeaponMode(currentWeaponMode);
        Debug.Log("WeaponMode : None");
    }

    /// <summary>
    /// 줌 인 했을 때 실행하는 함수 
    /// Function executed when on zoomIn
    /// </summary>
    private void OnZoomIn(InputAction.CallbackContext context)
    {
        if (currentWeaponMode != WeaponMode.Bow)
            return;

        OnBowZoomIn?.Invoke(true);
    }

    /// <summary>
    /// 줌 아웃 했을 때 실행하는 함수 
    /// Function executed when on zoomOut
    /// </summary>
    private void OnZoomOut(InputAction.CallbackContext context)
    {
        OnBowZoomOut?.Invoke();
    }

    /// <summary>
    /// 무기 모드에 따라 보여줄 무기이 변경되는 함수
    /// </summary>
    private void ChangeWeaponMode(WeaponMode mode)
    {
        switch (mode)
        {
            case WeaponMode.None:
                ShowWeapon(false, false);
                IsBowEquip = false;
                break;
            case WeaponMode.Sword:
                ShowWeapon(true, false);
                sword.SwordColliderDisable();
                IsBowEquip = false;
                break;
            case WeaponMode.Bow:
                ShowWeapon(false, true);
                UpdateArrow();
                IsBowEquip = true;
                break;
        }
    }

    /// <summary>
    /// 무기를 보여줄지 말지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여주고, false면 안보여준다.</param>
    public void ShowWeapon(bool isSwordShow = false, bool isBowShow = false)
    {
        if (swordWeapon != null)
        {
            swordWeapon.gameObject.SetActive(isSwordShow);
        }
        if(bowWeapon != null)
        {
            bowWeapon.gameObject.SetActive(isBowShow);
        }
    }

    /// <summary>
    /// 현재 무기 모드 확인용 함수
    /// </summary>
    /// <returns></returns>
    public int CheckWeaponMode()
    {
        int weaponNum = -1;
        if ((int)currentWeaponMode == 0)
        {
            weaponNum = 0;
            return weaponNum;
        }

        if ((int)currentWeaponMode == 1)
        {
            weaponNum = 1;
            return weaponNum;
        }

        if ((int)currentWeaponMode == 2)
        {
            weaponNum = 2;
            return weaponNum;
        }

        return weaponNum;
    }

    /// <summary>
    /// 화살 장전 함수
    /// </summary>
    private void OnLoad()
    {
        if (player.SkillRelatedAction.IsPickUp || player.isTalk || player.IsAnyUIPanelOpened) // 물건을 들고 있거나 대화중일 때 입력 막기
            return;

        if (IsBowEquip) // 활을 장비하고 있는 경우
        {
            if (!IsArrowEquip && ArrowCount > 0)  // 장전된 화살이 없는 경우
            {
                animator.SetBool(HaveArrowHash, true); // 화살 장전
                // Debug.Log($"화살 장전 완료");
            }

            IsArrowEquip = true; // 화살이 장전되어있다고 변수 설정
        }
    }

    /// <summary>
    /// 화살이 장전된 후 작업(화살 관련 변수 설정)을 위한 함수
    /// </summary>
    public void LoadArrowAfter()
    {
        if (player.SkillRelatedAction.IsPickUp || player.isTalk || player.IsAnyUIPanelOpened) // 물건을 들고 있거나 대화중일 때 입력 막기
            return;

        if (IsArrowEquip) // 화살이 장전된 상태인 경우
        {
            // Debug.Log($"IsZoomIn : {IsZoomIn}");
            //animator.SetBool(ZoomInHash, IsZoomIn); // 카메라 줌 설정 , 활 시위 당기는 중
            animator.SetBool(ZoomInHash, isPressed);
            if(!IsZoomIn) OnBowZoomIn?.Invoke(false);
        }
    }

    /// <summary>
    /// 장전된 화살을 발사하는 함수 (Animation 설정용)
    /// </summary>
    public void FireLoadedArrow()
    {
        //arrowFirePoint.FireArrow();
        // 화살 개수 소비
        OnFireArrow();
    }

    /// <summary>
    /// 무기 [검]의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void SwordWeaponColliderEnable()
    {
        sword.SwordColliderEnable();
    }

    /// <summary>
    /// 무기 [검]의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void SwordWeaponColliderDisable()
    {
        sword.SwordColliderDisable();
    }

    /// <summary>
    /// 무기 [활]의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void BowWeaponColliderEnable()
    {
        bow.BowColliderEnable();
    }

    /// <summary>
    /// 무기 [활]의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void BowWeaponColliderDisable()
    {
        bow.BowColliderDisable();
    }

    /// <summary>
    /// 무기 [화살]의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void ArrowWeaponColliderEnable()
    {
        arrow.ArrowColliderEnable();
    }

    /// <summary>
    /// 무기 [화살]의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void ArrowWeaponColliderDisable()
    {
        arrow.ArrowColliderDisable();
    }

    ///// <summary>
    ///// 무기 [검]을 꺼내는 함수 (Animation 설정용)
    ///// </summary>
    //public void SwordWeaponOpen()
    //{
    //    sword.OpenSwordWeapon();
    //}

    ///// <summary>
    ///// 무기 [검]을 넣는 함수 (Animation 설정용)
    ///// </summary>
    //public void SwordWeaponClose()
    //{
    //    sword.CloseSwordWeapon();
    //}

    ///// <summary>
    ///// 무기 [활]을 꺼내는 함수 (Animation 설정용)
    ///// </summary>
    //public void BowWeaponOpen()
    //{
    //    bow.OpenBowWeapon();
    //}

    ///// <summary>
    ///// 무기 [활]을 넣는 함수 (Animation 설정용)
    ///// </summary>
    //public void BowWeaponClose()
    //{
    //    bow.CloseBowWeapon();
    //}

    ///// <summary>
    ///// 무기 [화살]을 꺼내는 함수 (Animation 설정용)
    ///// </summary>
    //public void ArrowWeaponOpen()
    //{
    //    arrow.OpenArrow();
    //}

    ///// <summary>
    ///// 무기 [화살]을 넣는 함수 (Animation 설정용)
    ///// </summary>
    //public void ArrowWeaponClose()
    //{
    //    arrow.CloseArrow();
    //}

    /// <summary>
    /// 크리티컬 데미지 설정 함수
    /// </summary>
    /// <param name="damageRatio">베이스 데미지에서 더할 데미지 비율</param>
    public void SetCritDamage(float damageRatio)
    {
        player.attackPower += player.attackPower * damageRatio;
    }

    /// <summary>
    /// attackPower을 baseDamage로 설정하는 함수
    /// </summary>
    public void SetBasedamage()
    {
        player.attackPower = player.baseAttackPower;
    }

    #region Inventory Item Method
    /// <summary>
    /// 아이템 장착시 실행되는 함수 ( 무기 정보를 가진 변수 초기화 함수 )
    /// </summary>
    /// <param name="partIndex">장착부위 인덱스</param>
    void OnEquipWeapon(int partIndex)
    {
        if(partIndex == (int)EquipPart.Hand_R)
        {
            swordWeapon = player.partPosition[partIndex].GetChild(0);
            sword = swordWeapon.GetComponent<Sword>();

            currentWeaponMode = WeaponMode.Sword;
            ShowWeapon(true, false);   
        }

        if (partIndex == (int)EquipPart.Hand_L)
        {
            bowWeapon = player.partPosition[partIndex].GetChild(0);
            bow = bowWeapon.GetComponent<Bow>();

            currentWeaponMode = WeaponMode.Bow;
            ShowWeapon(false, true);   
        }
    }

    /// <summary>
    /// 장비 장착이 해제됬을 때 실행하는 함수
    /// </summary>
    /// <param name="partIndex">장비 부위 인덱스</param>
    void OnUnEquipWeapon(int partIndex)
    {
        // 장착해제한 아이템이 검이면
        if (partIndex == (int)EquipPart.Hand_R)
        {
            sword = null;
            swordWeapon = null;
        }

        // 장착해제한 아이템이 활이면
        if (partIndex == (int)EquipPart.Hand_L)
        {
            bow = null;
            bowWeapon = null;
        }
    }

    /// <summary>
    /// 화살을 발사 할 때 화살 개수를 소모하고 화살을 생성하는 함수
    /// </summary>
    void OnFireArrow()
    {
        // 해당슬롯에 개수가 부족하면 보충
        if(ArrowCount > 0 && arrowSlot.CurrentItemCount <= 0)
        {
            if (!UpdateArrow())
            {
                Debug.Log("인벤토리에 화살이 존재하지 않습니다.");

                return;
            }
        }

        arrowCount--;
        arrowSlot.DiscardItem(1); // 인벤토리에서 화살 개수 소비
        arrowFirePoint.GetFireArrow(PoolObjectType.Arrow, arrowPrefab, pressTime); // 화살 Factory에서 생성
        pressTime = 0f; // 발사하고 시간 초기화
    }

    /// <summary>
    /// 화살을 들 때 인벤토리에서 사용할 수 있는 화살 개수를 업데이트 하는 함수
    /// </summary>
    bool UpdateArrow()
    {
        bool result = true;
        
        Inventory inventory = player.Inventory;
        uint totalGetItemCount = 0;

        for (uint i = 0; i < inventory.SlotSize; i++)
        {
            InventorySlot slot = inventory[i];
            ItemData_AttackConsumption itemData = slot.SlotItemData as ItemData_AttackConsumption;

            if (itemData != null)
            {
                totalGetItemCount += (uint)slot.CurrentItemCount;  // 사용할 수 있는 화살 개수 추가
                arrowPrefab = itemData.ItemPrefab;  // 화살 프리팹 갱신

                arrowSlot = slot; // 사용하는 슬롯 갱신
            }
            else
            {
                //Debug.Log($"{inventory[i]}번 슬롯에 화살이 존재하지 않습니다.");
                result = false; 
            }
        }

        arrowCount = totalGetItemCount; // 찾은 화살 개수 갱신

        return result;
    }

    /// <summary>
    /// 가장 강한 아이템을 장착하는 함수
    /// </summary>
    /// <param name="mode">장착할 무기 모드</param>
    void QuickEquipWeapon(WeaponMode mode)
    {
        WeaponType weaponType; // mode에 따른 무기 타입 설정
        switch (mode)
        {
            case WeaponMode.Sword:
                weaponType = WeaponType.Melee;
                break;
            case WeaponMode.Bow:
                weaponType = WeaponType.Range;
                UpdateArrow();  // 화살 개수 갱신
                break;
            default:
                return;
        }
        InventorySlot slot = player.Inventory.QuickWeaponEquip(weaponType); // 아이템 슬롯

        if (slot == null) 
            return;   // 반환한 슬롯이 없으면 리턴
        else
        {
            ItemData_Weapon itemData = slot.SlotItemData as ItemData_Weapon;    // 아이템 데이터

            GameObject itemPrefab = itemData.EqiupPrefab;                       // 아이템 프리팹
            EquipPart part = itemData.equipPart;                                // 아이템 장착 부위

            player.CharacterEquipItem(itemPrefab, part, slot);

            ChangeWeaponMode(currentWeaponMode); // 성공하면 모드 변경
        }

    }
    #endregion
}