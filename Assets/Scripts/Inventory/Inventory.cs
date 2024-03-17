using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 클래스
/// </summary>
public class Inventory
{
    // 슬롯들관리

    const uint maxSlot = 6;

    public InventorySlot[] slots;

    public Inventory()
    {
        slots = new InventorySlot[maxSlot];

        for(int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    /// <summary>
    /// 슬롯에 아이템 추가 ( 맨 앞 슬롯에 추가 )
    /// </summary>
    /// <param name="data">아이템 코드</param>
    /// <param name="n">아이템 개수</param>
    public void AddItem(int code, int n)
    {
        for(uint i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItem == null)
            {   // check Slot is Empty
                slots[i].AddItem(ItemDataManager.itemDataManager[code], n);
                return;
            }
        }
    }

    /// <summary>
    /// 특정 슬롯에 아이템 추가
    /// </summary>
    /// <param name="data">아이템 코드</param>
    /// <param name="n">아이탬 개수</param>
    /// <param name="index">슬롯 위치</param>
    public void AddItem(ItemData data, uint n, uint index)
    {
        if(index > maxSlot)
        {
            Debug.Log($"존재하지 않는 슬롯입니다.");
            return;
        }
    }

#if UNITY_EDITOR

    string str;
    public void ShowInventory()
    {
        for(int i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItem != null)
            {
                str += $"{slots[i].SlotItem.itemName}" +
                       $"{slots[i].CurrentItemCount} / " +
                       $"{slots[i].SlotItem.maxCount}";
            }
            else
            {
                str += $"빈칸";
            }
            str += $", ";
        }

        Debug.Log(str);
    }
#endif
}
