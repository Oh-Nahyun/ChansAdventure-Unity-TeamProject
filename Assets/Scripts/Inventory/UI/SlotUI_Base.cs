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
    /// slotUI의 슬롯데이터
    /// </summary>
    InventorySlot inventorySlot;

    /// <summary>
    /// slotUI 데이터 접근 프로퍼티
    /// </summary>
    public InventorySlot Slot => inventorySlot;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponentInChildren<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitializeSlotUI(InventorySlot slot)
    {
        inventorySlot = slot;
        inventorySlot.onChangeSlotData = Refresh;
        Refresh();
    }

    protected virtual void Refresh()
    {
        if(Slot.SlotItemData == null)
        {
            slotIcon.color = Color.clear;
            slotIcon.sprite = null;
            slotItemCount.text = string.Empty;
        }
        else
        {
            slotIcon.color = Color.white;
            slotIcon.sprite = Slot.SlotItemData.itemIcon;
            slotItemCount.text = Slot.CurrentItemCount.ToString();
        }

        Debug.Log($"{Slot.SlotIndex}, {Slot.CurrentItemCount}");
    }
}
