using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 슬롯 UI 베이스 클래스
/// </summary>
public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// 슬롯 아이콘
    /// </summary>
    Image slotIcon;

    /// <summary>
    /// 아이템 개수
    /// </summary>
    TextMeshProUGUI slotItemCount;

    /// <summary>
    /// 아이템 장착
    /// </summary>
    TextMeshProUGUI slotEquip;

    /// <summary>
    /// slotUI의 슬롯데이터
    /// </summary>
    InventorySlot inventorySlot;

    /// <summary>
    /// slotUI 데이터 접근 프로퍼티
    /// </summary>
    public InventorySlot InventorySlotData => inventorySlot;

    /// <summary>
    /// 인벤토리 슬롯의 데이터를 받아서 초기화 하는 함수
    /// </summary>
    /// <param name="slot">인벤토리 슬롯</param>
    public void InitializeSlotUI(InventorySlot slot)
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        slotEquip = child.GetComponent<TextMeshProUGUI>();

        inventorySlot = slot;
        InventorySlotData.onChangeSlotData = Refresh;   // inventorySlot의 델리게이트에 UI를 갱신할 함수 등록
        Refresh();
    }
    
    /// <summary>
    /// 인벤토리 슬롯 새로고침하는 함수
    /// </summary>
    private void Refresh()
    {
        if(InventorySlotData.SlotItemData == null)
        {
            // 슬롯에 아이템이 없으면
            slotIcon.color = Color.clear;
            slotIcon.sprite = null;
            slotItemCount.text = string.Empty;

            slotEquip.color = Color.clear;
        }
        else
        {   // 슬롯에 아이템 데이터가 있으면 갱신
            slotIcon.color = Color.white;
            slotIcon.sprite = InventorySlotData.SlotItemData.itemIcon;
            slotItemCount.text = InventorySlotData.CurrentItemCount.ToString();            

            slotEquip.color = InventorySlotData.IsEquip ? Color.white : Color.clear; // 장착 여부 
        }
    }
}
