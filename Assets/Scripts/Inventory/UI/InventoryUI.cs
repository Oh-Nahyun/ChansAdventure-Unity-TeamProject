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

    // 드래그 시작
    private void OnSlotDragBegin(uint index)
    {
        if (Inventory[index].SlotItemData != null)
        {
            Debug.Log(Inventory.TempSlot);
            Inventory.SlotToTemp(Inventory[index].SlotIndex,
                           (uint)Inventory[index].SlotItemData.itemCode,
                                 Inventory[index].CurrentItemCount);
        }
    }

    // 드래그 종료
    private void OnSlotDragEnd(uint index)
    {
        Debug.Log(Inventory.TempSlot.SlotIndex);
        if (Inventory[index].SlotItemData != null) // 옮기는 슬롯에 아이템이 있다
        {
            ItemData tempData = Inventory[index].SlotItemData;
            uint tempIndex = Inventory[index].SlotIndex;
            int tempItemCount = Inventory[index].CurrentItemCount;

            Inventory.TempToSlot(Inventory.TempSlot.SlotIndex,
                           (uint)Inventory.TempSlot.SlotItemData.itemCode,
                                 Inventory.TempSlot.CurrentItemCount);
            Inventory.SlotToTemp(tempIndex, (uint)tempData.itemCode, tempItemCount);
        }
        else // 옮기는 곳에 아이템이 없다.
        {
            Inventory.TempToSlot(Inventory.TempSlot.SlotIndex,
                           (uint)Inventory.TempSlot.SlotItemData.itemCode,
                                 Inventory.TempSlot.CurrentItemCount);
        }
    }

    // UI 열기
    // UI 닫기
}
