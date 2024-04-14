using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory2 : TestBase
{
    public GameObject Test_Player;

    Inventory inven;

    [Header("슬롯 정보")]

    [Tooltip("아이템 코드 입력")]
    public uint code;
    [Tooltip("슬롯 인덱스")]
    [Range(0,5)]
    public uint indexA = 0;
    [Range(0,5)]
    public uint indexB = 0;
    [Tooltip("개수")]
    [Range(1,10)]
    public int count = 1;

    public SortMode sortMode;
    public bool isAcending = false;

    void Start()
    {
        inven = new Inventory(Test_Player);

        inven.AddSlotItem(5, 3);
        inven.AddSlotItem(1, 3);
        inven.AddSlotItem(3, 1);
        inven.AddSlotItem(4, 1);

        inven.TestShowInventory();
        ItemDataManager.Instance.InventoryUI.InitializeInventoryUI(inven);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inven.AddSlotItem(code, count);
            inven.TestShowInventory();
        }
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inven.AddSlotItem(code, count, indexA);
            inven.TestShowInventory();
        }
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.DividItem(indexA, indexB, count);
            inven.TestShowInventory();
        }
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.SortSlot((SortMode)2, false);
        }
    }
}
