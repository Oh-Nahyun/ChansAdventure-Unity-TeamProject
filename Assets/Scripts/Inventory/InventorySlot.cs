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
    int currentItemCount = 0;
    public int CurrentItemCount => currentItemCount;

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
    /// <param name="count">추가할 개수 (Default = 1)</param>
    public void AddItem(int code, int count = 1)
    {
        
        itemData = ItemDataManager.Instance.datas[code];
        currentItemCount += count;
    }

    //감소
    public void DiscardItem(int discardCount)
    {
        currentItemCount -= discardCount;
    }

    //clear
    public void ClearItem()
    {
        itemData = null;
        currentItemCount = 0;
    }
}
