using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 클래스
/// </summary>
public class Inventory
{
    /// <summary>
    /// 최대 슬롯개수
    /// </summary>
    const uint maxSlot = 6;

    /// <summary>
    /// 임시 슬롯 인덱스
    /// </summary>
    const uint tempIndex = 999999;

    /// <summary>
    /// 인벤토리 슬롯들
    /// </summary>
    public InventorySlot[] slots;

    List<InventorySlot> tempList;

    /// <summary>
    /// 임시 슬롯
    /// </summary>
    TempSlot tempslot;

    public Inventory()
    {
        slots = new InventorySlot[maxSlot];
        tempList = new List<InventorySlot>();

        for(int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    /// <summary>
    /// 슬롯에 아이템 추가
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">추가할 아이템 개수</param>
    /// <param name="index">추가할 슬롯 위치 인덱스</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        int overCount = 0;
        uint vaildIndex = 0;

        if (index > maxSlot)
        {
            Debug.Log($"존재하지 않는 슬롯 인덱스 입니다.");
        }

        if (slots[index].SlotItemData != null) // 해당 슬롯에 아이템이 존재한다 (1개이상)
        {
            if(slots[index].CurrentItemCount == slots[index].SlotItemData.maxCount) // 슬롯이 다 찼는지 체크
            {                
                Debug.Log($"slot[{index}] is Full");

                // 남아있는 슬롯 찾기
                vaildIndex = FindSlot(code); // 다음 칸 체크
                if (vaildIndex >= maxSlot) // 모든 슬롯이 전부 찼으면
                {
                    Debug.Log($"비어있는 슬롯이 없습니다.");
                    return;
                }
                else // 모든 슬롯이 다 안찼으면
                {
                    slots[vaildIndex].AssignItem(code, count, out overCount);
                }
            }
            else
            {
                if (slots[index].SlotItemData.itemCode != ((ItemCode)code)) // 추가하려는 아이템이 서로 다른 아이템이다.
                {
                    vaildIndex = FindSlot(code); // 비어있는 칸 확인

                    if (vaildIndex >= maxSlot)
                    {
                        Debug.Log($"비어있는 슬롯이 없습니다.");
                        return;
                    }
                    else
                    {
                        slots[vaildIndex].AssignItem(code, count, out overCount);
                    }

                }
                else // 추가하려는 아이템이 서로 같은 아이템이다.
                {
                    slots[index].AssignItem(code, count, out overCount);  // 아이템 추가
                }
            }
        }
        else // 해당 슬롯에 아이템이 없다.
        {
            slots[index].AssignItem(code, count, out overCount);  // 아이템 추가
        }


        // 남은 거 추가
        if (overCount == 0) return; // 넘치는게 없으면 종료
        else
        {
            vaildIndex = FindSlot(code);
            if(vaildIndex < maxSlot)
            {
                slots[vaildIndex].AssignItem(code, overCount, out _);
            }
            else
            {
                Debug.Log($"인벤토리가 가득찼습니다.");
            }
        }
    }

    /// <summary>
    /// 슬롯에 아이템 제거
    /// </summary>
    /// <param name="count">제거할 슬롯 개수</param>
    /// <param name="index">제거할 슬롯 위치 인덱스</param>
    public void DiscardSlotItem(int count, uint index = 0)
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
    uint FindSlot(uint code)
    {
        uint index = 0;
        foreach(var slot in slots)
        {
            if (slot.SlotItemData == null) // 데이터가 비어있다
                break;
            else if (slot.SlotItemData.itemCode == (ItemCode)code &&
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)
                break;

            index++;
        }

        return index;
    }

    /// <summary>
    /// 슬롯 스왑함수 (a인덱스 슬롯과 b인덱스 슬롯을 교체한다)
    /// </summary>
    /// <param name="indexA">스왑할 슬롯 인덱스 1</param>
    /// <param name="indexB">스왑할 슬롯 인덱스 2</param>
    public void SwapSlot(uint indexA, uint indexB)
    {   
        if(indexA == indexB)
        {
            Debug.Log($"인덱스값이 동일합니다.");
            return;
        }

        uint tempIndex = slots[indexA].SlotIndex;   // 슬롯 인덱스        
        ItemData tempItemdata = slots[indexA].SlotItemData; // 아이템 데이터        
        int tempItemCount = slots[indexA].CurrentItemCount; // 슬롯이 가진 아이템 개수

        slots[indexA].ClearItem();
        slots[indexA].AssignItem((uint)slots[indexB].SlotItemData.itemCode,
                              slots[indexB].CurrentItemCount,
                              out _);

        slots[indexB].ClearItem();
        slots[indexB].AssignItem((uint)tempItemdata.itemCode,
                              tempItemCount,
                              out _);
    }

    /// <summary>
    /// 정렬하는 함수
    /// </summary>
    /// <param name="sortMode"></param>
    public void SortSlot(SortMode sortMode, bool isAcending)
    {
        tempList = new List<InventorySlot>(slots);

        switch(sortMode)
        {
            case SortMode.Name:
                tempList.Sort((current, other) =>
                {
                    if(current.SlotItemData == null)
                        return 1;
                    if(other.SlotItemData == null)
                        return -1;
                    if(isAcending)
                    {
                        return current.SlotItemData.itemName.CompareTo(other.SlotItemData.itemName);
                    }
                    else
                    {
                        return other.SlotItemData.itemName.CompareTo(current.SlotItemData.itemName);
                    }
                });
                break;
            case SortMode.Price:
                tempList.Sort((current, other) =>
                {
                    if (current.SlotItemData == null)
                        return 1;
                    if (other.SlotItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.SlotItemData.price.CompareTo(other.SlotItemData.price);
                    }
                    else
                    {
                        return other.SlotItemData.price.CompareTo(current.SlotItemData.price);
                    }
                });
                break;
            case SortMode.Count:
                tempList.Sort((current, other) =>
                {
                    if (current.SlotItemData == null)
                        return 1;
                    if (other.SlotItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return current.CurrentItemCount.CompareTo(other.CurrentItemCount);
                    }
                    else
                    {
                        return other.CurrentItemCount.CompareTo(current.CurrentItemCount);
                    }
                });
                break;
        }

        int slotIndex = 0;
        foreach(var listIndex in tempList)
        {
            slots[slotIndex] = listIndex;
            slotIndex++;
        }

        tempList.Clear();
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