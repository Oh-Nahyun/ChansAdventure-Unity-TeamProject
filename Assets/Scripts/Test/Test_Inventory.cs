using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestInputBase
{
    Inventory inventory;

    [Header("슬롯 정보")]

    [Tooltip("아이템 코드 입력")]
    public int code;
    [Tooltip("슬롯 인덱스")]
    public int index;

    void Start()
    {
        inventory = new Inventory();
    }

    protected override void OnKey1Input(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inventory.AddItem(0, 1);
            inventory.ShowInventory();
        }
    }
}
