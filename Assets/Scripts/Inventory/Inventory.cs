using System;
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

    // 슬롯에 아이템 추가
    public void AddSlotItem(uint code, uint count, uint index = 0)
    {
        uint overCount = 0;

        if(index > maxSlot)
        {
            Debug.Log($"존재하지 않는 슬롯 인덱스 입니다.");
        }

        if (slots[index].SlotItemData != null) // 해당 슬롯에 아이템이 존재한다 (1개이상)
        {
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // 슬롯이 다 찼는지 체크
            {                
                Debug.Log($"slot[{index}] is Full");

                // 남아있는 슬롯 찾기
                uint vaildIndex = FindSlot(code, index); // 다음 칸 체크
                if (vaildIndex >= maxSlot) // 모든 슬롯이 전부 찼으면
                {
                    Debug.Log($"비어있는 슬롯이 없습니다.");
                    return;
                }
                else // 모든 슬롯이 다 안찼으면
                {
                    slots[vaildIndex].AddItem(code, count, out overCount);
                }
            }
            else
            {
                if (slots[index].SlotItemData.itemCode != ((ItemCode)code)) // 추가하려는 아이템이 서로 다른 아이템이다.
                {               
                    uint vaildIndex = FindSlot(code, index); // 비어있는 칸 확인

                    if (vaildIndex >= maxSlot)
                    {
                        Debug.Log($"비어있는 슬롯이 없습니다.");
                        return;
                    }
                    else
                    {
                        slots[vaildIndex].AddItem(code, count, out overCount);
                    }

                }
                else // 추가하려는 아이템이 서로 같은 아이템이다.
                {
                    slots[index].AddItem(code, count, out overCount);  // 아이템 추가
                }
            }
        }
        else // 해당 슬롯에 아이템이 없다.
        {
            slots[index].AddItem(code, count, out overCount);  // 아이템 추가
        }

        Debug.Log(overCount);
    }

    public void DiscardSlotItem(uint count, uint index = 0)
    {
        if(slots[index].SlotItemData == null)
        {
            Debug.Log($"슬롯이 비어있습니다.");
            return;
        }

        slots[index].DiscardItem(count);
    }
    
    /// <summary>
    /// 가장 빠른번호의 비어있는 슬롯을 찾아주는 함수
    /// </summary>
    /// <param name="code">찾는 아이템 코드</param>
    /// <param name="start">시작 인덱스</param>
    uint FindSlot(uint code, uint start)
    {
        uint index = start;
        foreach(var slot in slots)
        {
            // 찾는 조건 
            if (slot.SlotItemData == null) // 데이터가 비어있다
                break;
            else if (slot.SlotItemData.itemCode == (ItemCode)code && 
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)
                break;

            index++;
        }

        return index;
    }

#if UNITY_EDITOR
    public void TestShowInventory()
    {
        string str = null;
        str += $"(";
        for(int i = 0; i < maxSlot; i++)
        {
            if(slots[i].SlotItemData == null)
            {
                str += $"빈칸";
            }
            else
            {
                str += $"[{slots[i].SlotItemData.itemName} ({slots[i].CurrentItemCount}/ {slots[i].SlotItemData.maxCount}]";
            }
            str += $", ";
        }
        str += $")";

        Debug.Log($"{str}");
    }
#endif
}