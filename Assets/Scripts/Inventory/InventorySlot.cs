using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 슬롯 클래스
/// </summary>
public class InventorySlot
{
    // 슬롯 아이템 관리
    // 갯수, 아이템코드
    // 아이템 개수 증가, 감소

    /// <summary>
    /// 슬롯 인덱스
    /// </summary>
    uint slotIndex;
    
    /// <summary>
    /// 슬롯 인덱스 접근 프로퍼티
    /// </summary>
    public uint SlotIndex => slotIndex;

    /// <summary>
    /// 아이템 데이터
    /// </summary>
    ItemData itemData = null;
    public ItemData SlotItemData
    {
        get => itemData;
        private set
        {
            if (itemData != value)
            {
                itemData = value;

                onChangeSlotData?.Invoke();
            }
        }
    }

    /// <summary>
    /// Current Item count
    /// </summary>
    int currentItemCount = 0;

    /// <summary>
    /// 아이템 개수 접근을 위한 프로퍼티
    /// </summary>
    public int CurrentItemCount
    {
        get => currentItemCount;
        set
        {
            if (currentItemCount != value)
            {
                currentItemCount = value;
                onChangeSlotData?.Invoke();
            }
        }
    }

    //장착 여부

    /// <summary>
    /// 아이템 데이터 변경을 알리는 델리게이트
    /// </summary>
    public Action onChangeSlotData;

    /// <summary>
    /// InventorySlot 생성자
    /// </summary>
    /// <param name="index"></param>
    public InventorySlot(uint index)
    {
        slotIndex = index;
        SlotItemData = null;
        CurrentItemCount = 0;
    }

    /// <summary>
    /// 아이템 추가 함수
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">추가할 개수</param>
    public virtual void AssignItem(uint code, int count, out int over)
    {
        int overCount = 0;
        // 넘친다면?
        SlotItemData = ItemDataManager.Instance.datas[code];
        CurrentItemCount += count;  // add item

        if (CurrentItemCount > SlotItemData.maxCount)
        {
            overCount = CurrentItemCount - (int)SlotItemData.maxCount;  // 개수가 초과하는 아이템

            CurrentItemCount = (int)SlotItemData.maxCount;
        }
        over = overCount;
    }

    //감소
    public void DiscardItem(int discardCount)
    {
        CurrentItemCount -= discardCount;
        if (CurrentItemCount < 1)
        {
            CurrentItemCount = 0;
            ClearItem();
        }
    }

    //clear
    public virtual void ClearItem()
    {
        SlotItemData = null;
        CurrentItemCount = 0;
    }
}
