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
    public void AddSlotItem(int code, int count, uint index = 0)
    {
        if (slots[index].SlotItemData != null) // 해당 슬롯에 아이템이 존재한다 (1개이상)
        {            
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // 슬롯이 다 찼는지 체크
            {                
                Debug.Log($"slot[{index}] is Full");

                uint vaildIndex = FindEmptySlot(index); // 다음 칸 체크
                if (vaildIndex > maxSlot)
                {
                    Debug.Log($"비어있는 슬롯이 없습니다.");
                    return;
                }
                else
                {
                    slots[vaildIndex].AddItem(code, count);
                }
            }
            else
            {
                if (slots[index].SlotItemData.itemCode != ((ItemCode)code)) // 추가하려는 아이템이 서로 다른 아이템이다.
                {               
                    uint vaildIndex = FindEmptySlot(index); // 비어있는 칸 확인

                    if (vaildIndex > maxSlot)
                    {
                        Debug.Log($"비어있는 슬롯이 없습니다.");
                        return;
                    }
                    else
                    {
                        slots[vaildIndex].AddItem(code, count);
                    }

                }
                else // 추가하려는 아이템이 서로 같은 아이템이다.
                {                    
                    slots[index].AddItem(code, count);  // 아이템 추가
                }
            }
        }
        else // 해당 슬롯에 아이템이 없다.
        {
            slots[index].AddItem(code, count);  // 아이템 추가
        }
    }
    
    /// <summary>
    /// 가장 빠른번호의 비어있는 슬롯을 찾아주는 함수
    /// </summary>
    /// <param name="start">시작 인덱스</param>
    uint FindEmptySlot(uint start)
    {
        uint index = start;
        foreach(var slot in slots)
        {
            if (slot.SlotItemData == null) // 데이터가 없으면 break
                break;

            index++;
        }

        return index;
    }
}