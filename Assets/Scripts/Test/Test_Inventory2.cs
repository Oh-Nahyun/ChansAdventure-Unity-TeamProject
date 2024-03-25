using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory2 : TestInputBase
{
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
        inven = new Inventory();

        inven.AddSlotItem(0, 3);
        inven.AddSlotItem(1, 2);
        inven.AddSlotItem(2, 1);
        inven.AddSlotItem(1, 3);

        inven.TestShowInventory();
        ItemDataManager.Instance.InventoryUI.InitializeInventoryUI(inven);
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inven.AddSlotItem(code, count);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey2Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inven.AddSlotItem(code, count, indexA);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey3Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.DividItem(indexA, indexB, count);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey4Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.SortSlot((SortMode)2, false);
        }
    }
}
