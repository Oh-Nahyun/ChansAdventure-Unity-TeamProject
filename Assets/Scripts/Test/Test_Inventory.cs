using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    public GameObject Test_Player;
     
    Inventory inven;

    public InventoryUI inventoryUI;

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

        inven.AddSlotItem(0, 3);
        inven.AddSlotItem(1, 2);
        inven.AddSlotItem(2, 1);
        inven.TestShowInventory();

        inventoryUI.InitializeInventoryUI(inven);
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //inven.AddSlotItem(code,count, indexA);
            //inven.TestShowInventory();

            inven.AddSlotItem(code, count);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey2Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.DiscardSlotItem(count, indexA);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey3Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.SwapSlot(indexA, indexB);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey4Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.SortSlot(sortMode, isAcending);
            inven.TestShowInventory();
        }
    }
}
