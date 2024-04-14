using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 인벤토리용 테스트 캐릭터 스크립트
/// </summary>
public class Test_EquipCharacter : MonoBehaviour, IEquipTarget, IHealth
{   
    public float hp;
    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP);
            onHealthChange?.Invoke(hp);
        }
    }

    public float maxHP = 5;
    public float MaxHP => maxHP;

    /// <summary>
    /// 체력이 변경될 때 실행되는 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 캐릭터가 살아있는지 확인하는 프로퍼티 ( 0 초과 : true,)
    /// </summary>
    public bool IsAlive => HP > 0;

    /// <summary>
    /// 캐릭터가 사망하면 실행되는 델리게이트
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// 아이템 장착할 위치 ( equipPart 순서대로 초기화 해야함)
    /// </summary>
    [Tooltip("Equip Part와 동일하게 배치할 것")]
    public Transform[] partPosition;

    /// <summary>
    /// 장착한 부위의 아이템들
    /// </summary>
    private InventorySlot[] equipPart;

    public InventorySlot[] EquipPart
    {
        get => equipPart;
        set
        {
            equipPart = value;
        }
    }

    Inventory inventory;
    PlayerinputActions input;

    Interaction interaction;

    int partCount = Enum.GetNames(typeof(EquipPart)).Length;

    void Awake()
    {
        input = new PlayerinputActions();   // 인풋 객체 생성
        interaction = GetComponent<Interaction>();
    }

    void Start()
    {
        inventory = new Inventory(this.gameObject, 16); // 인벤 초기화
        GameManager.Instance.ItemDataManager.InventoryUI.InitializeInventoryUI(inventory); // 인벤 UI 초기화

        EquipPart = new InventorySlot[partCount]; // EquipPart 배열 초기화

        HP = MaxHP; // 체력 초기화

#if UNITY_EDITOR
        Test_AddItem();
#endif
    }

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Open_Inventory.performed += OnOpenInventory;
        input.Player.Get_Item.performed += OnGetItem;
        input.Player.Get_Item.canceled += OnGetItem;
    }

    /// <summary>
    /// 아이템을 획득하는 인풋 ( F Key )
    /// </summary>
    private void OnGetItem(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("F키 눌림");

            if(interaction.short_enemy != null) // 감지한 아이템 오브젝트가 존재한다.
            {
                GameObject itemObject = interaction.short_enemy.gameObject;       // 가장 가까운 오브젝트 
                if(itemObject.TryGetComponent(out ItemDataObject itemDataObject)) // 해당 오브젝트에 ItemDataObject 클래스가 존재하면 true
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
    }

    void OnDisable()
    {
        input.Player.Get_Item.canceled -= OnGetItem;
        input.Player.Get_Item.performed -= OnGetItem;
        input.Player.Open_Inventory.performed -= OnOpenInventory;
        input.Player.Disable();
    }

    private void OnOpenInventory(InputAction.CallbackContext _)
    {
        GameManager.Instance.ItemDataManager.InventoryUI.ShowInventory();

        GameManager.Instance.ItemDataManager.CharaterRenderCameraPoint.transform.eulerAngles = new Vector3(0, 180f, 0); //
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
            // false
            CharacterUnequipItem(part); // 장착했던 아이템 파괴

            Instantiate(equipment, partPosition[(int)part]); // 아이템 오브젝트 생성
            EquipPart[(int)part] = slot;    // 장착부위에 아이템 정보 저장
        }
        else // 장착한 아이템이 없으면
        {
            EquipPart[(int)part] = slot;
            Instantiate(equipment, partPosition[(int)part]); // 아이템 오브젝트 생성
        }
    }

    /// <summary>
    /// 캐릭터 아이템 장착해제 할 때 실행하는 함수
    /// </summary>
    /// <param name="part"></param>
    public void CharacterUnequipItem(EquipPart part)
    {
        Destroy(partPosition[(int)part].GetChild(0).gameObject);    // 아이템 오브젝트 파괴
    }

    /// <summary>
    /// 사망시 실행되는 함수
    /// </summary>
    public void Die()
    {
        throw new NotImplementedException();
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
        while(timeElapsed < Duration)
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

#if UNITY_EDITOR
    void Test_AddItem()
    {
        inventory.AddSlotItem((uint)ItemCode.Hammer);
        inventory.AddSlotItem((uint)ItemCode.Sword);
        inventory.AddSlotItem((uint)ItemCode.HP_portion,3);
        inventory.AddSlotItem((uint)ItemCode.Coin);
    }
#endif
}