using System;
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
    InventorySlotUI[] slotsUIs;

    /// <summary>
    /// 임시 슬롯 UI
    /// </summary>
    TempSlotUI tempSlotUI;

    public Action<uint> onSlotDragBegin;
    public Action<uint> onSlotDragEnd;

    /// <summary>
    /// 인벤토리 UI를 초기화하는 함수
    /// </summary>
    /// <param name="playerInventory">플레이어 인벤토리</param>
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        inventory = playerInventory;    // 초기화한 인벤토리 내용 받기
        slotsUIs = new InventorySlotUI[Inventory.slotSize]; // 슬롯 크기 할당
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // 일반 슬롯
        tempSlotUI = GetComponentInChildren<TempSlotUI>(); // 임시 슬롯

        for (uint i = 0; i < Inventory.slotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // 인벤토리슬롯을 slotUI와 연결
        }
        tempSlotUI.InitializeSlotUI(Inventory.TempSlot); // null 참조

        onSlotDragBegin += OnSlotDragBegin;
        onSlotDragEnd += OnSlotDragEnd;

    }

    /// <summary>
    /// 슬롯 드래그 시작
    /// </summary>
    /// <param name="index">임시 슬롯에 들어갈 인벤토리 슬롯 인덱스</param>
    private void OnSlotDragBegin(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            uint targetSlotIndex = index;
            uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
            int targetItemSlotCount = Inventory[index].CurrentItemCount;

            Inventory.AccessTempSlot(targetSlotIndex, targetSlotItemCode, targetItemSlotCount);
            inventory[index].ClearItem();
        }
    }

    /// <summary>
    /// 슬롯 드래그 종료
    /// </summary>
    /// <param name="index">아이템을 넣을 인벤토리 슬롯 인덱스</param>
    private void OnSlotDragEnd(uint index)
    {
        uint tempFromIndex = Inventory.TempSlot.FromIndex;

        // 임시 슬롯에 들어있는 내용
        uint tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
        int tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;

        if(Inventory[index].SlotItemData != null)   // 아이템이 들어있다.
        {
            uint targetSlotItemCode = (uint)Inventory[index].SlotItemData.itemCode;
            int targetSlotItemCount = Inventory[index].CurrentItemCount;

            Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount); // target 슬롯에 아이템 저장
            Inventory.AccessTempSlot(index, targetSlotItemCode, targetSlotItemCount); // target 슬롯에 있었던 아이템 내용 임시 슬롯에 저장
            
            tempSlotItemCode = (uint)Inventory.TempSlot.SlotItemData.itemCode;
            tempSlotItemCount = Inventory.TempSlot.CurrentItemCount;
            
            Inventory.AccessTempSlot(tempFromIndex, tempSlotItemCode, tempSlotItemCount);
        }
        else
        {
            Inventory.AccessTempSlot(index, tempSlotItemCode, tempSlotItemCount);
        }
    }
    // UI 열기
    // UI 닫기
}
