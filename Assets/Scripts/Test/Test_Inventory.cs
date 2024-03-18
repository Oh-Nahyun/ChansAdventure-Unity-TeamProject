using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    Inventory inven;

    [Header("슬롯 정보")]

    [Tooltip("아이템 코드 입력")]
    public int code;
    [Tooltip("슬롯 인덱스")]
    [Range(0,5)]
    public uint index;

    void Start()
    {
        inven = new Inventory();
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inven.AddSlotItem(code,1,index);
        }
    }
}
