using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    Inventory inven;

    [Header("슬롯 정보")]

    [Tooltip("아이템 코드 입력")]
    public uint code;
    [Tooltip("슬롯 인덱스")]
    [Range(0,5)]
    public uint index = 0;
    [Tooltip("개수")]
    [Range(1,10)]
    public uint count = 1;

    void Start()
    {
        inven = new Inventory();
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.AddSlotItem(code,count,index);
            inven.TestShowInventory();
        }
    }

    protected override void OnKey2Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.DiscardSlotItem(count, index);
            inven.TestShowInventory();
        }
    }
}
