using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GoldUI : TestBase
{
    public int value;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ItemDataManager.Instance.InventoryUI.Test_GoldChange(value);
    }
}
