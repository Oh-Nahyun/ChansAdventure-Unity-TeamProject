using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
#if UNITY_EDITOR

    public GameObject Test_Player;
    public uint index;
    Inventory inven;

    Transform target;

    void Start()
    {
        target = transform.GetChild(0);

        inven = new Inventory(Test_Player);

        inven.AddSlotItem(5, 3);
        inven.AddSlotItem(1, 3);
        inven.AddSlotItem(3, 1);
        inven.AddSlotItem(4, 1);

        inven.TestShowInventory();
        GameManager.Instance.ItemDataManager.InventoryUI.InitializeInventoryUI(inven);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetItemObject(inven[index].SlotItemData, target.position);
    }

#endif
}
