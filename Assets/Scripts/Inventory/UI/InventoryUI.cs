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
    /// 인벤토리 UI를 초기화하는 함수
    /// </summary>
    /// <param name="playerInventory">플레이어 인벤토리</param>
    public void InitializeInventoryUI(Inventory playerInventory)
    {
        inventory = playerInventory;    // 초기화한 인벤토리 내용 받기
        slotsUIs = new InventorySlotUI[Inventory.slotSize]; // 슬롯 크기 할당
        slotsUIs = GetComponentsInChildren<InventorySlotUI>();  // 컴포넌트 받기

        for (uint i = 0; i < Inventory.slotSize; i++)
        {
            slotsUIs[i].InitializeSlotUI(Inventory[i]); // 인벤토리슬롯을 slotUI와 연결
        }
    }

    // UI 열기
    // UI 닫기
}
