using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 인벤토리
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// 인벤토리 접근용 프로퍼티
    /// </summary>
    Inventory Inventory => inventory;

    /// <summary>
    /// UI slots
    /// </summary>
    InventorySlotUI[] slots;

    /// <summary>
    /// UI 슬롯 접근용 인덱서
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public InventorySlotUI this[uint index] => slots[index];     

    void Awake()
    {
        inventory = new Inventory();
        slots = new InventorySlotUI[Inventory.slotSize];

        for(uint i = 0; i < Inventory.slotSize; i++)
        {
            slots = GetComponentsInChildren<InventorySlotUI>();
            slots[i].SlotData = Inventory[i]; 
        }

        int a = 0;
    }
}
