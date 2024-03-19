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
    ItemData itemData;
    public ItemData SlotItemData => itemData;

    /// <summary>
    /// Current Item count
    /// </summary>
    uint currentItemCount = 0;
    public uint CurrentItemCount => currentItemCount;

    //장착 여부

    /// <summary>
    /// InventorySlot 생성자
    /// </summary>
    /// <param name="index"></param>
    public InventorySlot(uint index)
    {
        slotIndex = index;
        itemData = null;
        currentItemCount = 0;
    }

    /// <summary>
    /// 아이템 추가 함수
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">추가할 개수</param>
    public void AddItem(uint code, uint count, out uint over)
    {
        uint overCount = 0;
        // 넘친다면?
        itemData = ItemDataManager.Instance.datas[code];
        currentItemCount += count;  // add item

        if (currentItemCount > SlotItemData.maxCount)
        {
            overCount = currentItemCount - SlotItemData.maxCount;  // 개수가 초과하는 아이템

            currentItemCount = SlotItemData.maxCount;
        }
        over = overCount;
    }

    //감소
    public void DiscardItem(uint discardCount)
    {
        currentItemCount -= (uint)discardCount;
        if (currentItemCount < 1)
        {
            currentItemCount = 0;
            ClearItem();
        }
    }

    //clear
    public void ClearItem()
    {
        itemData = null;
        currentItemCount = 0;
    }
}
