using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour, IEquipTarget, IHealth, IStamina, IBattler
{
    #region additional Classes
    PlayerController controller;

    /// <summary>
    /// 플레이어 컨트롤러 접근용 프로퍼티
    /// </summary>
    public PlayerController PlayerController => controller;
    
    /// <summary>
    /// PlayerSKills를 받기 위한 프로퍼티
    /// </summary>
    public PlayerSkills Skills => gameObject.GetComponent<PlayerSkills>();

    Animator animator;
    CharacterController characterController;

    /// <summary>
    /// 스킬 관련 클래스
    /// </summary>
    PlayerSkillRelatedAction skillRelatedAction;

    /// <summary>
    /// 스킬 관련 클래스 접근을 위한 프로퍼티
    /// </summary>
    public PlayerSkillRelatedAction SkillRelatedAction => skillRelatedAction;

    /// <summary>
    /// 오브젝트에 가할 힘 값 - 05.13
    /// </summary>
    const float reactPower = 5f;

    /// <summary>
    /// 오브젝트에 가할 힘에 접근할 프로퍼티 ( 5f ) - 05.13
    /// </summary>
    public float ReactPower => reactPower;
    #endregion

    // 변수 ==========================================================================================================================

    #region PlayerMove Values

    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;

    [Header("캐릭터 기본 정보")]
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 7.0f;

    /// <summary>
    /// 모든 기력 소진으로 느려진 속도
    /// </summary>
    public float slowSpeed = 1.5f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    public float currentSpeed = 0.0f;

    /// <summary>
    /// 이동 모드
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // 걷기
        Run         // 달리기
    }

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Walk;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;        // 상태 변경
            if (currentSpeed > 0.0f)        // 이동 중인지 아닌지 확인
            {
                // 이동 중이면 모드에 맞게 속도와 애니메이션 변경
                MoveSpeedChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// 이동 중인지 아닌지 확인용 변수
    /// </summary>
    bool isMoving = false;

    /// <summary>
    /// 카메라 회전
    /// </summary>
    Quaternion followCamY;

    /// <summary>
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 중력값
    /// </summary>
    float gravity = -15f;

    /// <summary>
    /// 점프 시간 제한
    /// </summary>
    const float jumpTimeLimit = 1.0f;

    /// <summary>
    /// 점프 시간
    /// </summary>
    float jumpTime = 0.0f;

    /// <summary>
    /// 점프 정도
    /// </summary>
    public float jumpPower = 5.0f;

    /// <summary>
    /// 점프 속도
    /// </summary>
    float jumpVelocity;

    /// <summary>
    /// 플레이어 점프 벡터
    /// </summary>
    Vector3 playerJump;

    /// <summary>
    /// 점프 중인지 아닌지 확인용 변수
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티 (점프중이 아닐 때)
    /// </summary>
    bool IsJumpAvailable => !isJumping;

    /// <summary>
    /// 슬라이드 시간 제한
    /// </summary>
    const float slideTimeLimit = 1.0f;

    /// <summary>
    /// 슬라이드 시간
    /// </summary>
    float slideTime = 0.0f;

    /// <summary>
    /// 슬라이드 정도
    /// </summary>
    public float slidePower = 10.0f;

    /// <summary>
    /// 플레이어 슬라이드 벡터
    /// </summary>
    Vector3 playerSlide;

    /// <summary>
    /// 슬라이드 중인지 아닌지 확인용 변수
    /// </summary>
    bool isSliding = false;

    /// <summary>
    /// 슬라이드가 가능한지 확인하는 프로퍼티 (슬라이드중이 아닐 때)
    /// </summary>
    bool IsSlideAvailable => !isSliding;

    /// <summary>
    /// 주변 시야 버튼이 눌렸는지 아닌지 확인용 변수
    /// </summary>
    public bool isLook = false;

    /// <summary>
    /// 주변 시야 방향 벡터
    /// </summary>
    Vector3 lookVector = Vector3.zero;

    /// <summary>
    /// 주변 시야 카메라
    /// </summary>
    public GameObject cameraRoot;

    /// <summary>
    /// 주변 시야 카메라 회전 정도
    /// </summary>
    public float followCamRotatePower = 5.0f;

    /// <summary>
    /// 카메라의 수직회전을 하는지 여부 (마그넷캐치에서 사용중)
    /// </summary>
    bool isCameraRotateVertical = true;

    // 애니메이터용 해시값
    //readonly int IsMoveBackHash = Animator.StringToHash("IsMoveBack");
    readonly int IsJumpHash = Animator.StringToHash("IsJump");
    readonly int IsSlideHash = Animator.StringToHash("IsSlide");
    readonly int SpeedHash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorSlowSpeed = 0.15f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    readonly int DieHash = Animator.StringToHash("IsDie");
    readonly int GetHitHash = Animator.StringToHash("IsGetHit");
    readonly int SpendAllStaminaHash = Animator.StringToHash("IsSpendAllStamina");

    // 컴포넌트
    Weapon weapon;
    #endregion

    #region Inventory Values

    [Header("인벤토리 정보")]
    /// <summary>
    /// 아이템 장착할 위치 ( equipPart 순서대로 초기화 해야함)
    /// </summary>
    [Tooltip("Equip Part와 동일하게 배치할 것")]
    public Transform[] partPosition;

    /// <summary>
    /// 장착한 부위의 아이템들
    /// </summary>
    private InventorySlot[] equipPart;

    /// <summary>
    /// 장착할 부위접근을 하기위한 프로퍼티
    /// </summary>
    public InventorySlot[] EquipPart
    {
        get => equipPart;
        set
        {
            if(equipPart != value)
            {
                equipPart = value;
            }
        }
    }

    int partCount = Enum.GetNames(typeof(EquipPart)).Length;

    /// 인벤토리에서 아이템 장착시 실행되는 델리게이트
    /// </summary>
    public Action<int> OnEquipWeaponItem;

    /// <summary>
    /// 인벤토리에서 아이템 장착해제시 실행되는 델리게이트
    /// </summary>
    public Action<int> OnUnEquipWeaponItem;

    /// <summary>
    /// 해당 오브젝트의 인벤토리
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// 오브젝트 인벤토리 접근을 위한 프로퍼티
    /// </summary>
    public Inventory Inventory
    {
        get
        {
            if (inventory == null)
            {
                Debug.Log($"받을 인벤토리가 존재하지 않습니다");

                inventory = new Inventory(this.gameObject, 16);
            }
            return inventory;
        }
        set
        {
            if (inventory == null)
            {
                Debug.Log($"수정할 인벤토리가 존재하지 않습니다");

                inventory = new Inventory(this.gameObject, 16);
            }

            inventory = value;
        }
    }

    /// <summary>
    /// 맵 패널 활성화 여부 ( true : 열려있음 , false 닫혀있음 )
    /// </summary>
    bool isOpenMapPanel = false;

    /// <summary>
    /// 맵 패널 활성화 여부를 접근하기 위한 프로퍼티 
    /// </summary>
    public bool IsOpenMapPanel => isOpenMapPanel;

    /// <summary>
    /// 인벤토리 패널 활성화 여부 ( true : 열려있음, false 닫혀있음)
    /// </summary>
    bool isOpenInventoryPanel = false;

    /// <summary>
    /// UI가 열려있는지 확인하는 변수
    /// </summary>
    bool isOpenedAnyUIPanel = false;

    /// <summary>
    /// UI가 열려있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsOpenedAnyUIPanel => isOpenedAnyUIPanel;

    #endregion

    #region IHealth Values

    /// <summary>
    /// 오브젝트가 가지고 있는 현재 체력
    /// </summary>
    float hp = 1;

    /// <summary>
    /// 체력을 접근하기 위한 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            if (IsAlive) // 살아있을 때만 HP 변화
            {
                hp = Mathf.Clamp(value, 0, MaxHP);  // 최소 ~ 최대 사이로 숫자 유지
                onHealthChange?.Invoke(hp);         // 델리게이트로 HP 변화 알림

                if (hp <= 0.0f)                     // HP가 0 이하인 경우
                {
                    Die();                          // 사망 처리
                }
            }
        }
    }

    /// <summary>
    /// 최대 HP
    /// </summary>
    float maxHP = 1;

    /// <summary>
    /// 최대 HP 접근 프로퍼티
    /// </summary>
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }

    /// <summary>
    /// 체력이 변경될 때 실행되는 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 캐릭터가 살아있는지 확인하는 프로퍼티 (0 초과 : true)
    /// </summary>
    public bool IsAlive => HP > 0;

    /// <summary>
    /// 캐릭터가 사망하면 실행되는 델리게이트
    /// </summary>
    public Action onDie { get; set; }

    #endregion

    #region IStamina Values

    [Header("캐릭터 스태미나 정보")]

    /// <summary>
    /// 기력 소모 및 증가 속도
    /// </summary>
    public float spendStaminaTime = 10.0f;

    /// <summary>
    /// 현재 기력
    /// </summary>
    float stamina = 100.0f;

    /// <summary>
    /// 기력 확인 및 설정용 프로퍼티
    /// </summary>
    public float Stamina
    {
        get => stamina;
        set
        {
            if (IsEnergetic) // 기력이 있을 때만 Stamina 변화
            {
                stamina = Mathf.Clamp(value, 0, MaxStamina);            // 최소 ~ 최대 사이로 숫자 유지
                onStaminaChange?.Invoke(stamina);                       // 델리게이트로 Stamina 변화 알림

                if (stamina <= 0.0f)                                    // Stamina가 0 이하인 경우
                {
                    SpendAllStamina();                                  // Stamina가 없는 경우 처리
                    StartCoroutine(DecreaseSpeedForChargingStamina());  // Stamina 충전 코루틴
                    //CurrentMoveMode = MoveMode.Walk;                    // Walk 모드로 변경
                }
            }
        }
    }

    /// <summary>
    /// 최대 기력
    /// </summary>
    float maxStamina = 100.0f;

    /// <summary>
    /// 최대 기력 확인용 프로퍼티
    /// </summary>
    public float MaxStamina
    {
        get => maxStamina;
        set
        {
            maxStamina = value;
        }
    }

    /// <summary>
    /// 기력이 변경될 때마다 실행될 델리게이트용 프로퍼티
    /// </summary>
    public Action<float> onStaminaChange { get; set; }

    /// <summary>
    /// 기력이 남아있는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsEnergetic => Stamina > 0;

    /// <summary>
    /// 기력 소진을 알리기 위한 델리게이트용 프로퍼티
    /// </summary>
    public Action onSpendAllStamina { get; set; }

    /// <summary>
    /// 스태미너 UI
    /// </summary>
    StaminaCheckUI staminaCheckUI;
    #endregion

    #region IBattler Values

    [Header("캐릭터 전투 정보")]
    // 플레이어의 공격력과 방어력
    public float baseAttackPower = 10.0f;
    public float baseDefencePower = 3.0f;

    /// <summary>
    /// 공격력
    /// </summary>
    public float attackPower = 10.0f;

    /// <summary>
    /// 캐릭터 공격력 프로퍼티
    /// </summary>
    public float AttackPower => attackPower;   

    /// <summary>
    /// 방어력
    /// </summary>
    public float defencePower = 3.0f;

    /// <summary>
    /// 캐릭터 방어도 프로퍼티
    /// </summary>
    public float DefencePower => defencePower;

    /// <summary>
    /// 공격 받았을 때 실행하는 델리게이트
    /// </summary>
    public Action<int> onHit { get; set; }

    /// <summary>
    /// 플레이어가 받은 최종 데미지
    /// </summary>
    public float finalDamage;

    #endregion

    #region PlayerInteraction Values

    [Header("인터렉션 정보")]
    public bool isTalk = false;

    /// <summary>
    /// 상호작용을 하기위한 interaction 클래스
    /// </summary>
    Interaction interaction;

    #endregion

    #region Etc Values
    /// <summary>
    /// UI 패널이 열렸는지 확인하는 변수
    /// </summary>
    bool isAnyUIPanelOpened = false;
    public bool IsAnyUIPanelOpened => isAnyUIPanelOpened;

    MenuPanel menuPanel;

    #endregion

    // 함수 ==========================================================================================================================

    #region Player LifeCycle Method
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();

        isJumping = true;
        skillRelatedAction = GetComponent<PlayerSkillRelatedAction>();
        cameraRoot = FindAnyObjectByType<PlayerLookVCam>().gameObject;
        staminaCheckUI = FindAnyObjectByType<StaminaCheckUI>();
        interaction = GetComponent<Interaction>();
    }

    void OnEnable()
    {
        if(MaxHP < 2) // 체력 설정이 안되어있음
        {
            MaxHP = 300f;
            HP = MaxHP;
        }
        menuPanel = FindAnyObjectByType<MenuPanel>();

        // 게임 시작 전이면 비활성화
        GameState state = GameManager.Instance.CurrnetGameState;
        if (state == GameState.NotStart)
        {
            inventory = new Inventory(this.gameObject, 16);
            EquipPart = new InventorySlot[partCount]; // EquipPart 배열 초기화
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        // 기본값 설정
        attackPower = baseAttackPower;
        defencePower = baseDefencePower;

        // exception
        if (cameraRoot == null)
        {
            Debug.LogError("CameraRoot가 비어있습니다. CameraRoot Prefab 오브젝트를 넣어주세요 ( PlayerLookVCam 스크립트 있는 오브젝트 )");
        }

        isMoving = false;
        isJumping = true;
        isSliding = true;
        
        // controller
        controller.onMove += OnMove;
        controller.onMoveRunMode += OnMoveRunMode;
        controller.onMoveWalkMode += OnMoveWalkMode;
        controller.onLook += OnLookAround;
        controller.onSlide += OnSlide;
        controller.onJump += OnJump;
        controller.onInteraction += OnGetItem;
        controller.onInventoryOpen += OnInventoryShow;
        controller.onMapOpen += OnMapShow;
        controller.onOpenQuest += OnQusetShow;
        controller.onMenuOpen += OnOpenMenuPanel;

        // inventory
        //inventory = new Inventory(this.gameObject, 16);

        GameManager.Instance.ItemDataManager.InventoryUI.InitializeInventoryUI(Inventory); // 인벤 UI 초기화
        GameManager.Instance.TextBoxManager.isTalkAction += (talk) => IsTalk(talk);
        EquipPart = new InventorySlot[partCount]; // EquipPart 배열 초기화
    }

    private void Update()
    {
        LookRotation();
        Jump();
        Slide();

        // Stamina -------------------------------------------------------------

        // 플레이어 상태에 따른 스태미너 변경
        if (isMoving && CurrentMoveMode == MoveMode.Run)
        {
            staminaCheckUI.gameObject.SetActive(true);          // 스태미너 UI 활성화

            // 플레이어가 Run 모드로 움직이고 있는 경우
            // (스태미너가 모두 소진되지 않은 경우에만 스태미너를 감소시킴)
            if (Stamina > 0)
                Stamina -= spendStaminaTime * Time.deltaTime;   // Stamina 감소
        }
        else
        {
            staminaCheckUI.gameObject.SetActive(true);          // 스태미너 UI 활성화

            // 플레이어가 움직이지 않거나, Walk 모드인 경우
            // (스태미너가 최대치가 아닌 경우에만 스태미너를 증가시킴)
            if (Stamina < MaxStamina)
                Stamina += spendStaminaTime * Time.deltaTime;   // Stamina 증가

            // 스태미너가 다 찬 경우
            if (Stamina >= MaxStamina)
                staminaCheckUI.gameObject.SetActive(false);     // 스태미너 UI 비활성화
        }

        // Debug.Log($"Player's Stamina : {Stamina}");

        inputDirection.y += gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * currentSpeed * inputDirection);      // 캐릭터의 움직임
    }

    private void FixedUpdate()
    {        
        if(weapon.IsZoomIn)
        {
            // set target position
            Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraRotation, Time.fixedDeltaTime * turnSpeed); // 목표 회전으로 변경            
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed); // 목표 회전으로 변경            
        }
    }
    #endregion

    #region Player Movement Method
    /// <summary>
    /// Get Player input Values
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="isMove">check press button ( wasd )</param>
    void OnMove(Vector2 input, bool isMove)
    {
        if (IsOpenedAnyUIPanel)
            return;

        // 입력 방향 저장
        inputDirection.x = input.x;
        inputDirection.y = 0;
        inputDirection.z = input.y;

        // 입력을 시작한 상황
        if (isMove)
        {
            isMoving = true;

            // 입력 방향 회전시키기
            followCamY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);   // 카메라의 y회전만 따로 추출
            inputDirection = followCamY * inputDirection;                                        // 입력 방향을 카메라의 y회전과 같은 정도로 회전시키기
            targetRotation = Quaternion.LookRotation(inputDirection);                            // 회전 저장

            // 이동 모드 변경
            MoveSpeedChange(CurrentMoveMode);

            // 카메라 상태에 따른 무기 설정
            if (weapon.IsZoomIn)
                weapon.ShowWeapon(false, true);

        }

        // 입력을 끝낸 상황
        else
        {
            isMoving = false;
            currentSpeed = 0.0f; // 정지
            animator.SetFloat(SpeedHash, AnimatorStopSpeed);

            // 무기 모드에 따른 무기 설정
            if (weapon.CheckWeaponMode() == 1) // 무기 모드가 칼일 때
                weapon.ShowWeapon(true, false);
            else if (weapon.CheckWeaponMode() == 2) // 무기 모드가 활일 때
                weapon.ShowWeapon(false, true);
        }
    }

    /// <summary>
    /// 달리기 모드 처리용 함수
    /// </summary>
    private void OnMoveRunMode()
    {
        CurrentMoveMode = MoveMode.Run;
    }

    /// <summary>
    /// 걷기 모드 처리용 함수
    /// </summary>
    private void OnMoveWalkMode()
    {
        CurrentMoveMode = MoveMode.Walk;
    }

    /// <summary>
    /// 이동 모드 변경 함수
    /// </summary>
    /*
    private void OnMoveModeChange()
    {
        if (CurrentMoveMode == MoveMode.Walk)
        {
            CurrentMoveMode = MoveMode.Run;
        }
        else
        {
            CurrentMoveMode = MoveMode.Walk;
        }
    }
    */

    /// <summary>
    /// Check Look Around
    /// </summary>
    /// <param name="lookInput">lookInput value</param>
    /// <param name="isLookingAround">true : , false : No input Value</param>
    void OnLookAround(Vector2 lookInput, bool isLookingAround)
    {
        if (IsOpenedAnyUIPanel) // UI 열렸을 때
        {
            isLook = false;     // 카메라 움직임 비활성화
            return;
        }

        if (isLookingAround)
        {
            isLook = true;
            lookVector = lookInput;
        }

        if (!isLookingAround)
        {
            isLook = false;
        }
    }

    /// <summary>
    /// Change Player Movemode
    /// </summary>
    /// <param name="mode">MoveMode</param>
    void MoveSpeedChange(MoveMode mode)
    {
        // 이동 중에는 무기 비활성화
        weapon.ShowWeapon(false, false);

        // 이동 모드에 따라 속도와 애니메이션 변경
        switch (mode)
        {
            case MoveMode.Walk:
                //jumpPower = 1.0f;
                currentSpeed = walkSpeed;
                animator.SetFloat(SpeedHash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                //jumpPower = 1.25f;
                currentSpeed = runSpeed;
                animator.SetFloat(SpeedHash, AnimatorRunSpeed);
                break;
        }
    }

    /// <summary>
    /// Player Camera Rotation
    /// </summary>
    void LookRotation()
    {
        if (!isLook || IsAnyUIPanelOpened)
            return;

        cameraRoot.transform.localRotation *= Quaternion.AngleAxis(lookVector.x * followCamRotatePower, Vector3.up);

        if (isCameraRotateVertical)
        {
            cameraRoot.transform.localRotation *= Quaternion.AngleAxis(-lookVector.y * followCamRotatePower, Vector3.right);
        }

        var angles = cameraRoot.transform.localEulerAngles;
        angles.z = 0;

        var angle = cameraRoot.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        cameraRoot.transform.localEulerAngles = angles;
        cameraRoot.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
    }

    public void SetMagnetCamera(bool isRotateVertical, float angle)
    {
        isCameraRotateVertical = isRotateVertical;
        Vector3 rotate = cameraRoot.transform.localRotation.eulerAngles;
        rotate.x = angle;
        cameraRoot.transform.localRotation = Quaternion.Euler(rotate);
    }

    /// <summary>
    /// 점프 입력 함수
    /// </summary>
    private void OnJump(bool isJump)
    {
        if (SkillRelatedAction.IsPickUp // 물건을 들고 있을 때 입력 막기
            || IsOpenedAnyUIPanel)
            return;

        if (isJump)
        {
            // 점프하는 중이 아닌 경우 => 점프 가능
            isJumping = false;
        }
        else
        {
            // 점프 중인 경우 => 점프 불가능
            isJumping = true;
        }
    }
    
    /// <summary>
    /// 점프 처리 함수
    /// </summary>
    void Jump()
    {
        if (IsJumpAvailable)
        {
            // 점프가 가능한 경우
            StopAllCoroutines();
            StartCoroutine(JumpProcess());   // 실제 점프 과정 처리
            animator.SetTrigger(IsJumpHash); // 점프 애니메이션 재생
        }
        else
        {
            // 점프가 불가능한 경우
            jumpVelocity = 0.0f;             // 점프 속도 초기화
            playerJump.y = jumpVelocity;
        }
        
        isJumping = true;
        //characterController.Move(playerJump * Time.fixedDeltaTime); // 점프 실행
    }

    /// <summary>
    /// 실제 점프 과정 처리 코루틴
    /// </summary>
    IEnumerator JumpProcess()
    {
        yield return new WaitForSeconds(0.2f); // 애니메이션 딜레이
        inputDirection.y = jumpPower;
    }

    /// <summary>
    /// 회피 입력 함수
    /// </summary>
    private void OnSlide(bool isSlide)
    {
        if (SkillRelatedAction.IsPickUp) // 물건을 들고 있을 때 입력 막기
            return;

        if (isSlide)
        {
            if (CurrentMoveMode == MoveMode.Run)    // 달리기 모드인 경우
            {
                if (isMoving)                       // 이동 중인 경우
                {
                    isSliding = false;              // 슬라이드 가능
                }
                else                                // 이동 중이 아닌 경우
                {
                    isSliding = true;               // 슬라이드 불가능
                }
            }
            else
            {
                isSliding = true;                   // 달리기 모드가 아닌 경우 => 슬라이드 불가능
            }
        }
        else
        {
            isSliding = true;                       // 슬라이드 중인 경우 => 슬라이드 불가능
        }
    }

    /// <summary>
    /// 회피 처리 함수
    /// </summary>
    void Slide()
    {
        if (IsSlideAvailable)
        {
            // 슬라이드가 가능한 경우
            StopAllCoroutines();
            StartCoroutine(SlideProcess());   // 실제 슬라이드 과정 처리
            animator.SetTrigger(IsSlideHash); // 슬라이드 애니메이션 처리
        }
        else
        {
            // 슬라이드가 불가능한 경우
            playerSlide = Vector3.zero;
        }

        isSliding = true;
        characterController.Move(playerSlide * Time.fixedDeltaTime); // 슬라이드 실행
    }

    /// <summary>
    /// 실제 슬라이드 과정 처리 코루틴
    /// </summary>
    IEnumerator SlideProcess()
    {
        slideTime = 0.0f;                                                           // 변수 초기화
        yield return new WaitForSeconds(0.1f);                                      // 0.1초 딜레이

        while (slideTime < slideTimeLimit)
        {
            slideTime += Time.deltaTime;                                            // 슬라이드 시간 갱신
            Vector3 localForward = transform.TransformDirection(Vector3.forward);   // 로컬 기준 Forward
            playerSlide = slideTime * slidePower * localForward;                    // 플레이어 슬라이드
            characterController.Move(playerSlide * Time.deltaTime);                 // 슬라이드 실행

            yield return null;
        }
    }

    #endregion 

    #region Player Inventory Method

    /// <summary>
    /// 아이템을 주울 때 실행되는 함수
    /// </summary>
    void OnGetItem()
    {
        if (SkillRelatedAction.IsPickUp) // 물건을 들고 있을 때 입력 막기
            return;

        if (interaction.short_enemy != null) // 감지한 아이템 오브젝트가 존재한다.
        {
            GameObject itemObject = interaction.short_enemy.gameObject;       // 가장 가까운 오브젝트 
            if (itemObject.TryGetComponent(out ItemDataObject itemDataObject)) // 해당 오브젝트에 ItemDataObject 클래스가 존재하면 true
            {
                itemDataObject.AdditemToInventory(inventory);
            }
            else
            {
                Debug.Log($"오브젝트 내에 ItemDataObject 클래스가 존재하지 않습니다.");
            }
        }
        else
        {
            Debug.Log($"감지한 아이템 오브젝트가 존재하지 않습니다.");
        }
    }

    /// <summary>
    /// 캐릭터 아이템 장착할 때 실행하는 함수
    /// </summary>
    /// <param name="equipment">장비 프리팹</param>
    /// <param name="part">장착할 부위</param>
    public void CharacterEquipItem(GameObject equipment, EquipPart part, InventorySlot slot)
    {
        if (EquipPart[(int)part] != null) // 장착한 아이템이 있으면
        {
            CharacterUnequipItem(part); // 장착했던 아이템 파괴
            Instantiate(equipment, partPosition[(int)part]); // 아이템 오브젝트 생성
            
            EquipPart[(int)part] = slot;    // 장착부위에 아이템 정보 저장
        }
        else // 장착한 아이템이 없으면
        {
            Instantiate(equipment, partPosition[(int)part]); // 아이템 오브젝트 생성
            EquipPart[(int)part] = slot;
        }

        OnEquipWeaponItem?.Invoke((int)part);
    }

    /// <summary>
    /// 캐릭터 아이템 장착해제 할 때 실행하는 함수
    /// </summary>
    /// <param name="part"></param>
    public void CharacterUnequipItem(EquipPart part)
    {
        if (EquipPart[(int)part] == null)
        {
            Debug.Log("장착한 아이템정보가 존재하지 않습니다.");
        }

        GameObject obj = partPosition[(int)part].GetChild(0).gameObject; // 아이템 파괴전 파괴할 오브젝트 활성화

        if (obj == null)
        {
            Debug.Log("아이템이 없습니다");
            return;
        }

        obj.SetActive(true);
        DestroyImmediate(obj);

        OnUnEquipWeaponItem?.Invoke((int)part);
        //Destroy(partPosition[(int)part].GetChild(0).gameObject);        // 아이템 오브젝트 파괴
    }

    /// <summary>
    /// 인벤토리 키 ( I Key )를 눌렀을 때 실행되는 함수
    /// </summary>
    public void OnInventoryShow()
    {
        if (isAnyUIPanelOpened)
            return;

        menuPanel.ShowMenu((MenuState)1);
        isAnyUIPanelOpened = true;
    }

    /// <summary>
    /// 인벤토리 데이터를 받는 함수
    /// </summary>
    /// <param name="invenData">받을 인벤토리 데이터</param>
    public void GetInventoryData(Inventory invenData)
    {
        Inventory = invenData;
    }

    /// <summary>
    /// UI가 닫힐 때 실행하는 함수
    /// </summary>
    public void UIPanelClose()
    {
        isAnyUIPanelOpened = false;
    }

    #endregion

    #region Player IHealth Method
    /// <summary>
    /// 적으로부터 공격을 받으면 실행되는 함수
    /// </summary>
    public void GetHit()
    {
        animator.SetTrigger(GetHitHash);
    }

    /// <summary>
    /// 사망시 실행되는 함수
    /// </summary>
    public void Die()
    {
        animator.SetTrigger(DieHash);
        controller.DisableInput();
        onDie?.Invoke();
        Debug.Log("플레이어 사망");
    }

    /// <summary>
    /// 체력회복 할 때 실행되는 함수
    /// </summary>
    /// <param name="totalRegen">총 회복량</param>
    /// <param name="duration">회복 주기 시간</param>
    public void HealthRegenerate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegen_Coroutine(totalRegen, duration));
    }

    /// <summary>
    /// 체력 회복 코루틴
    /// </summary>
    /// <param name="totalRegen">최종 회복할 체력량</param>
    /// <param name="Duration">체력회복하는 시간</param>
    /// <returns></returns>
    IEnumerator HealthRegen_Coroutine(float totalRegen, float Duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < Duration)
        {
            timeElapsed += Time.deltaTime;
            HP += (totalRegen / Duration) * Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// 틱당 체력 회복할 때 실행하는 함수
    /// </summary>
    /// <param name="tickRegen">틱당 회복량</param>
    /// <param name="tickInterval">회복 주기</param>
    /// <param name="totalTickCount">최종 틱 수</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(HealthRegenByTick_Coroutine(tickRegen, tickInterval, totalTickCount));
    }

    /// <summary>
    /// 틱당 체력 회복 코루틴
    /// </summary>
    /// <param name="tickRegen">틱당 회복량</param>
    /// <param name="tickInterval">회복 주기</param>
    /// <param name="totalTickCount">최종 틱 수</param>
    /// <returns></returns>
    IEnumerator HealthRegenByTick_Coroutine(float tickRegen, float tickInterval, uint totalTickCount)
    {
        for (int i = 0; i < totalTickCount; i++)
        {
            float timeElapsed = 0f;
            while (timeElapsed < tickInterval)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            HP += tickRegen;
        }
    }
    #endregion

    // Stamina =========================================================================================

    // 애니메이션 클립의 리소스 경로
    //private string clipPath_SpendAllStamina = "PlayerAnimations/Reflesh";

    /// <summary>
    /// 기력을 모두 소진했을 경우 처리용 함수
    /// </summary>
    public void SpendAllStamina()
    {
        animator.SetTrigger(SpendAllStaminaHash);   // 애니메이션 재생
        onSpendAllStamina?.Invoke();                // 스태미나 모두 사용했다고 알림
        Debug.Log("플레이어 스테미너 모두 사용");
    }

    /// <summary>
    /// 기력을 모두 소진하여 일정 시간동안 플레이어의 속도 감소 처리용 코루틴
    /// </summary>
    /// <param name="waitingTime">기력이 채워지는 시간</param>
    /// <returns></returns>
    IEnumerator DecreaseSpeedForChargingStamina()
    {
        //float timeElapsed = 0.0f;
        //float waitingTime = controller.GetAnimationLegth(clipPath_SpendAllStamina); // 애니메이션 클립 로드

        //while (timeElapsed < waitingTime)                       // 애니메이션 길이만큼 시간 투자
        //{
        //    timeElapsed += Time.deltaTime;                      // timeElapsed 갱신
        //    animator.SetFloat(SpeedHash, AnimatorSlowSpeed);    // Slow Speed 설정
        //    yield return null;
        //}
        CurrentMoveMode = MoveMode.Walk;

        while (stamina < MaxStamina)
        {
            stamina += spendStaminaTime * Time.deltaTime;       // Stamina 증가

            currentSpeed = slowSpeed;
            animator.SetFloat(SpeedHash, AnimatorSlowSpeed);    // Slow Speed 설정

            yield return null;
        }

        currentSpeed = walkSpeed;
        animator.SetFloat(SpeedHash, AnimatorWalkSpeed);        // Walk Speed 설정
    }

    // IBatter 인터페이스 상속 --------------------------------------------------------------------------------------------------

    /// <summary>
    /// 적의 약점 공격 시 플레이어의 공격력 증가율
    /// </summary>
    public float weakPointAttack = 1.2f;

    /// <summary>
    /// IBatter 공격 함수
    /// </summary>
    /// <param name="target">공격 대상</param>
    /// <param name="isWeakPoint">약점 공격했으면 true, 아니면 false</param>
    public void Attack(IBattler target, bool isWeakPoint = false)
    {
        if (isWeakPoint)
        {
            target.Defence(AttackPower * weakPointAttack);
        }
        else
        {
            target.Defence(AttackPower);
        }
    }

    /// <summary>
    /// IBatter 방어 함수
    /// </summary>
    /// <param name="damage">데미지</param>
    public void Defence(float damage)
    {
        if (IsAlive)
        {
            GetHit(); // 애니메이션 재생

            // 최종 데미지
            finalDamage = Mathf.Max(0, damage - DefencePower);
            HP -= finalDamage;

            onHit?.Invoke(Mathf.RoundToInt(finalDamage));
        }
    }

    #region Etc Method
    public void OnMapShow()
    {
        if (isAnyUIPanelOpened)
            return;

        menuPanel.ShowMenu((MenuState)2);
        isAnyUIPanelOpened = true;

    }

    void OnOpenMenuPanel()
    {
        if (isAnyUIPanelOpened)
            return;

        menuPanel.ShowMenu((MenuState)0);
        isAnyUIPanelOpened = true;
    }

    void OnQusetShow()
    {
        if (isAnyUIPanelOpened)
            return;

        GameManager.Instance.QuestManager.OpenQuest();
        isAnyUIPanelOpened = !isAnyUIPanelOpened;
    }

    public void SetTransform(Vector3 position)
    {
        gameObject.transform.localPosition = position;
    }

    /// <summary>
    /// 대화중임을 확인하는 함수
    /// </summary>
    /// <param name="talk">true면 대화중</param>
    /// <returns></returns>
    void IsTalk(bool talk)
    {
        isTalk = talk;
        isAnyUIPanelOpened = talk;
    }

    /// <summary>
    /// 캐릭터의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void CharacterColliderEnable()
    {
        characterController.enabled = true;
    }

    /// <summary>
    /// 캐릭터의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void CharacterColliderDisable()
    {
        characterController.enabled = false;
    }
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Player 임의로 아이템 부여하는 함수 ( 빌드 할 때는 없어짐 ) 
    /// </summary>
    public void Test_AddItem()
    {
        inventory.AddSlotItem((uint)ItemCode.Hammer);
        inventory.AddSlotItem((uint)ItemCode.Sword);
        inventory.AddSlotItem((uint)ItemCode.HP_portion, 3);
        inventory.AddSlotItem((uint)ItemCode.Coin);
    }
#endif
}