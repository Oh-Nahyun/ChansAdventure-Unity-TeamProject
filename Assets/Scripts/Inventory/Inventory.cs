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
    /// 인벤토리 크기 접근용 프로퍼티
    /// </summary>
    public uint slotSize => maxSlot;

    /// <summary>
    /// 인벤토리 슬롯들
    /// </summary>
    InventorySlot[] slots;

    /// <summary>
    /// 인벤토리 슬롯 접근을 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    /// <returns></returns>
    public InventorySlot this[uint index] => slots[index];

    /// <summary>
    /// 아이템 정렬을 위한 리스트
    /// </summary>
    List<InventorySlot> tempSortList;

    /// <summary>
    /// 임시 슬롯 클래스
    /// </summary>
    TempSlot tempSlot;

    /// <summary>
    /// 임시 슬롯 접근을 위한 프로퍼티
    /// </summary>
    public TempSlot TempSlot => tempSlot;

    /// <summary>
    /// 임시 슬롯 인덱스
    /// </summary>
    const uint tempIndex = 999999;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    public Inventory()
    {
        slots = new InventorySlot[maxSlot];
        tempSortList = new List<InventorySlot>();
        tempSlot = new TempSlot(tempIndex);

        for (int i = 0; i < maxSlot; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    #region Legacy AddItem Method
/*    /// <summary>
    /// 슬롯에 아이템 추가
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">추가할 아이템 개수</param>
    /// <param name="index">추가할 슬롯 위치 인덱스</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        int overCount = 0;
        uint vaildIndex = 0;

        if (!IsVaildSlot(index))
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
    }*/
    #endregion

    /// <summary>
    /// 아이템 추가 함수 , 가장 먼저있는 슬롯을 채움
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">아이템 개수</param>
    /// <param name="index">슬롯 인덱스</param>
    public void AddSlotItem(uint code, int count, uint index = 0)
    {
        if (index >= maxSlot) // 슬롯 우뮤 확인
        {
            Debug.Log($"{index}번 슬롯은 존재하지 않습니다.");
            return;
        }

        if (index == 0) // index값이 default값이면 자동 추가
        {
            uint slotIndex = FindSlot(code);

            if(slotIndex >= maxSlot)
            {
                Debug.Log("인벤토리가 가득 차있습니다");
                return;
            }

             slots[slotIndex].AssignItem(code, count, out int overCount);

            if (overCount > 0) // 넘친 아이템이 존재한다면
            {
                // 재탐색 후 넣기
                slotIndex = FindSlot(code);
                slots[slotIndex].AssignItem(code, overCount, out _);
            }
        }
        else // 특정 인덱스에 추가
        {
            slots[index].AssignItem(code, count, out int overCount);

            if (overCount > 0) // 넘친 아이템이 존재한다면
            {
                // 재탐색 후 넣기
                uint slotIndex = FindSlot(code);

                if (slotIndex >= maxSlot)
                {
                    Debug.Log("인벤토리가 가득 차있습니다");
                    return;
                }
                else
                {
                    slots[slotIndex].AssignItem(code, overCount, out _);
                }
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
            else if (slot.SlotItemData.itemCode == (ItemCode)code &&        // 매개변수 아이템 코드와 동일하고 
                     slot.CurrentItemCount < slot.SlotItemData.maxCount)    // 해당 슬롯의 아이템 개수가 최대치보다 낮다면 반환
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
        tempSortList = new List<InventorySlot>(slots);

        switch(sortMode)
        {
            case SortMode.Name:
                tempSortList.Sort((current, other) =>
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
                tempSortList.Sort((current, other) =>
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
                tempSortList.Sort((current, other) =>
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
        foreach(var listIndex in tempSortList)
        {
            slots[slotIndex] = listIndex;
            slotIndex++;
        }

        tempSortList.Clear();
    }

    public void AccessTempSlot(uint index, uint itemCode, int itemCount)
    {
        if(TempSlot.SlotItemData == null) // 임시 슬롯이 비어있다.
        {
            tempSlot.SetTempSlotIndex(index);
            tempSlot.AssignItem(itemCode, itemCount, out _); // temp슬롯 내용 추가
        }
        else if(TempSlot.SlotItemData != null)  // 임시 슬롯을 사용중이다.
        {
            slots[index].AssignItem(itemCode, itemCount, out _);

            tempSlot.ClearItem();
        }
    }

    /// <summary>
    /// 아이템 나누는 함수
    /// </summary>
    /// <param name="indexA">나눌 슬롯</param>
    /// <param name="indexB">나눈 아이템 넣을 슬롯</param>
    /// <param name="count">아이템 개수</param>
    public void DividItem(uint indexA, uint indexB, int count = 1)
    {
        if(indexA == indexB) // 동일 인덱스 확인
        {
            Debug.Log($"인덱스가 동일합니다. 나눌 수 없습니다.");
            return;
        }

        if(slots[indexA].CurrentItemCount < 2) // 아이템 개수 확인 ( 1이하면 실행 X )
        {
            Debug.Log($"[{slots[indexA]}]의 아이템 개수가 [{slots[indexA].CurrentItemCount}] 입니다. 나눌 수 없습니다.");
            return;
        }

        if(count > slots[indexA].CurrentItemCount)
        {
            count = slots[indexA].CurrentItemCount;
        }

        uint itemCode = (uint)slots[indexA].SlotItemData.itemCode;
        slots[indexA].DiscardItem(count);
        slots[indexB].AssignItem(itemCode, count, out _);
    }

    /// <summary>
    /// 해당 인덱스에 슬롯에 아이템이 들어갈 수 있는 지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>슬롯이 존재하면 true 아니면 false</returns>
    public bool IsVaildSlot(uint index)
    {
        return slots[index].SlotItemData == null;
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