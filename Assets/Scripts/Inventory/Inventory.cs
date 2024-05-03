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
    /// 최대 슬롯개수 ( 생성자에서 초기화 )
    /// </summary>
    uint maxSlot_size = 0;

    /// <summary>
    /// 인벤토리 크기 접근용 프로퍼티
    /// </summary>
    public uint SlotSize => maxSlot_size;

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
    /// 해당 인벤토리를 가진 오브젝트
    /// </summary>
    GameObject owner;

    /// <summary>
    /// 인벤토리를 가진 오브젝트에게 접근하는 프로퍼티
    /// </summary>
    public GameObject Owner => owner;

    /// <summary>
    /// 인벤토리의 골드
    /// </summary>
    uint gold = 0;

    /// <summary>
    /// 인벤토리의 골드를 접근하기 위한 프로퍼티
    /// </summary>
    public uint Gold
    {
        get => gold;
        private set
        {
            if(gold != value)
            {
                gold = value;
                onInventoryGoldChange?.Invoke(Gold);
            }
        }
    }

    public Action<uint> onInventoryGoldChange;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    public Inventory(GameObject invenOwner, uint slotSize = 6)
    {
        owner = invenOwner;
        maxSlot_size = slotSize;
        slots = new InventorySlot[maxSlot_size];
        tempSlot = new TempSlot(tempIndex);

        for (int i = 0; i < maxSlot_size; i++)
        {
            slots[i] = new InventorySlot((uint)i);
        }
    }

    /// <summary>
    /// 아이템 추가 함수 , 가장 먼저있는 슬롯을 채움
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">아이템 개수</param>
    /// <param name="index">슬롯 인덱스</param>
    public void AddSlotItem(uint code, int count = 1, uint index = 0)
    {
        if (index >= maxSlot_size) // 슬롯 우뮤 확인
        {
            Debug.Log($"{index}번 슬롯은 존재하지 않습니다.");
            return;
        }

        if (index == 0) // index값이 default값이면 자동 추가
        {
            uint slotIndex = FindSlot(code);

            if(slotIndex >= maxSlot_size)
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

                if (slotIndex >= maxSlot_size)
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
    /// <param name="sortMode">정렬방식</param>
    /// <param name="isAcending">오름차순 여부 (treu : 오름차순, false : 내림차순)</param>
    public void SortSlot(SortMode sortMode, bool isAcending)
    {
        List<InventorySlot>tempSortList = new List<InventorySlot>(slots); // 리스트 얕은 복사

        switch (sortMode)
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

        List<(ItemData, int)> sortedData = new List<(ItemData, int)>((int)SlotSize); // 정렬한 내용 복사
        foreach (var slot in tempSortList)
        {
            sortedData.Add((slot.SlotItemData, slot.CurrentItemCount));       // 정렬 내용 복사
        }

        int index = 0;
        foreach (var slot in sortedData)
        {
            // 슬롯에 내용 추가 -> 아이템을 첫 슬롯부터 재배치
            if (slot.Item1 == null) break;  // 정렬된 내용을 다 옮겼으면 break;
            slots[index].ClearItem();       // 슬롯 내용 정리 후
            slots[index].AssignItem((uint)slot.Item1.itemCode, (int)slot.Item2, out _);    // 복사한 내용을 슬롯에 설정 ( item1 : ItemDatam , item2 : CurrentItemCount )
            
            index++;
        }

        tempSortList.Clear();   // 임시 리스트 제거

        // 재배치후 불필요한 슬롯 데이터 제거
        for(int i = index; i < SlotSize; i++)
        {
            slots[i].ClearItem(); 
        }
    }

    /// <summary>
    /// 임시 슬롯 접근 함수
    /// </summary>
    /// <param name="index">임시 슬롯에 옮길 아이템 슬롯 인덱스</param>
    /// <param name="itemCode">임시 슬롯에 옮길 아이템 코드</param>
    /// <param name="itemCount">임시 슬롯에 옮길 아이템 량</param>
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
    /// 아이템 드롭할 때 실행하는 함수
    /// </summary>
    /// <param name="index">드롭할 아이템 슬롯</param>
    public void DropItem(uint index)
    {
        //GameObject dropItem = UnityEngine.Object.Instantiate(slots[index].SlotItemData.ItemPrefab, Owner.transform);
        ItemData dropItemData = slots[index].SlotItemData;
        uint itemCount = 1;

        GameObject dropItem = Factory.Instance.GetItemObject(dropItemData, itemCount, Owner.transform.position + Vector3.up * 0.5f); // Factory에서 아이템 소환

        dropItem.name = $"{slots[index].SlotItemData.itemName}";    // 아이템 이름 변경
        //dropItem.transform.SetParent(null);

        slots[index].DiscardItem(1);
    }

    /// <summary>
    /// 해당 인덱스에 슬롯에 아이템이 들어갈 수 있는 지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>슬롯이 존재하면 true 아니면 false</returns>
    public bool IsVaildSlot(uint index)
    {
        return slots[index].SlotItemData != null;
    }

    /// <summary>
    /// 골드를 획득할 때 실행되는 함수
    /// </summary>
    /// <param name="price">획득할 골드량</param>
    public void AddGold(uint price)
    {
        Gold += price;
    }

    /// <summary>
    /// 골드를 소모할 때 실행되는 함수
    /// </summary>
    /// <param name="price">획득할 골드량</param>
    public void SubCoin(uint price)
    {
        Gold -= price;
    }

#if UNITY_EDITOR

    /// <summary>
    /// 인벤토리 슬롯들의 정보를 보여주는 함수
    /// </summary>
    public void TestShowInventory()
    {
        string str = null;
        str += $"(";
        for(int i = 0; i < maxSlot_size; i++)
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