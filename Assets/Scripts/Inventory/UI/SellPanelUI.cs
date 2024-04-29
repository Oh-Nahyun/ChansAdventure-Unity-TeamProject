using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SellPanelUI : MonoBehaviour
{
    /// <summary>
    /// 아이템 정보 UI
    /// </summary>
    InventoryDetailUI detailUI;

    /// <summary>
    /// 아이템 파는 개수를 설정하는 UI
    /// </summary>
    SellCountUI sellCountUI;

    /// <summary>
    /// Check 패널
    /// </summary>
    SellCheckUI sellCheckUI;

    /// <summary>
    /// 목표 오브젝트의 인벤토리 클래스
    /// </summary>
    Inventory targetInventory;

    /// <summary>
    /// 아이템 목록 저장 변수
    /// </summary>
    SellSlotUI[] slots;

    /// <summary>
    /// 판매할려는 아이템의 슬롯
    /// </summary>
    InventorySlot targetSlot;

    /// <summary>
    /// 패널 닫는 버튼
    /// </summary>
    Button closeButton;

    /// <summary>
    /// 인벤토리 슬롯 프리팹
    /// </summary>
    public GameObject invenSlotPrefab;
    CanvasGroup canvasGroup;

    /// <summary>
    /// target의 인벤토리 사이즈
    /// </summary>
    uint inventorySize = 0;

    /// <summary>
    /// 받게될 최종 골드수
    /// </summary>
    uint totalGetGold = 0;

    /// <summary>
    /// 팔게될 최종 아이템 수
    /// </summary>
    int totalSellItemCount = 0;

    /// <summary>
    /// 첫 실행인지 확인하는 변수 (실행 않했으면 true, 한번이라도 실행했으면 false)
    /// </summary>
    bool isFirst = true;

    /// <summary>
    /// 현재 판매가 진행중인지 확인하는 변수 (판매가 진행중이면 true 아니면 false)
    /// </summary>
    bool isProcess = false;

    /// <summary>
    /// isProcess를 접근하기위한 프로퍼티 (판매가 진행중이면 true 아니면 false)
    /// </summary>
    public bool IsProcess => isProcess;

    public Action<uint> onShowDetail;
    public Action onCloseDetail;
    public Action<uint> onShowCheckPanel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        detailUI = GetComponentInChildren<InventoryDetailUI>();
        sellCountUI = GetComponentInChildren<SellCountUI>();
        sellCheckUI = GetComponentInChildren<SellCheckUI>();
        closeButton = transform.GetChild(4).GetComponent<Button>();
        closeButton.onClick.AddListener(() =>
        {
            CloseSellUI();
        });

        onShowDetail += OnShowDetail;
        onCloseDetail += OnCloseDetail;
        onShowCheckPanel += OnShowSellCount;
        sellCountUI.onSell += OnSellCheck;
        sellCountUI.onCloseSellCount += OnSellClose;
        sellCheckUI.onConformSell += OnConformSellItem;      
    }

    /// <summary>
    /// 아이템 정보를 불러와서 출력하는 함수
    /// </summary>
    /// <param name="index">인벤토리 인덱스</param>
    public void OnShowDetail(uint index)
    {
        if (slots[index].InventorySlotData.SlotItemData != null)
        {
            string name = slots[index].InventorySlotData.SlotItemData.itemName;
            string desc = slots[index].InventorySlotData.SlotItemData.desc;
            uint price = slots[index].InventorySlotData.SlotItemData.price;

            detailUI.SetDetailText(name, desc, price);
            detailUI.ShowItemDetail();
        }
    }

    /// <summary>
    /// 아이템 정보창을 닫는 함수
    /// </summary>
    public void OnCloseDetail()
    {
        detailUI.ClearText();
        detailUI.CloseItemDetail();
    }

    /// <summary>
    /// 판매창을 여는 함수
    /// </summary>
    public void OpenSellUI()
    {
        if (targetInventory == null)
        {
            Debug.Log($"targetInventory가 없습니다.");
            return;
        }

        SetSlot();
        isFirst = false;
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// 판매창을 닫는 함수
    /// </summary>
    public void CloseSellUI()
    {
        canvasGroup.alpha = 0f;
        targetInventory = null;
        isProcess = false;
    }

    /// <summary>
    /// SellCountUI 닫을 때 실행하는 함수
    /// </summary>
    private void OnSellClose()
    {
        isProcess = false;
    }

    /// <summary>
    /// SellCount 패널을 여는 함수
    /// </summary>
    /// <param name="index">판매할 아이템 슬롯</param>
    public void OnShowSellCount(uint index)
    {
        if(targetInventory[index] == null)
        {
            Debug.Log("아이템이 존재하지 않습니다.");
            return; 
        }

        if (targetInventory[index].IsEquip)
        {
            Debug.Log("해당 아이템은 장착 중입니다. 판매할 수 없습니다.");
            return;
        }

        isProcess = true;

        // 아이템이 여러개면 나누는 창 띄우기
        if (targetInventory[index].CurrentItemCount > 1)
        {
            sellCountUI.InitializeValue(targetInventory[index], 1, targetInventory[index].CurrentItemCount);
            sellCountUI.SellCountUIOpen();
        }
        else // 아이템이 1개면
        {
            OnSellCheck(targetInventory[index], 1);
        }
    }

    /// <summary>
    /// 확인 창을 보이게하는 함수
    /// </summary>
    /// <param name="slot">판매할 아이템 슬롯</param>
    /// <param name="count">판매할 개수</param>
    private void OnSellCheck(InventorySlot slot, int count)
    {
        // 확인 창 띄우기
        sellCheckUI.ShowCheckPanel();
        sellCheckUI.onCheckSell(slot, count);

        targetSlot = slot;
        totalGetGold = targetInventory[slot.SlotIndex].SlotItemData.price * (uint)count;
        totalSellItemCount = count;
    }

    /// <summary>
    /// 아이템 사는걸 확인 함수
    /// </summary>
    private void OnConformSellItem()
    {
        targetInventory.AddGold(totalGetGold);
        targetInventory[targetSlot.SlotIndex].DiscardItem(totalSellItemCount);
    }


    /// <summary>
    /// 게임 오브젝트의 인벤토리를 찾는 함수
    /// </summary>
    /// <param name="target">인벤토리 객체가 있는 게임 오브젝트 ( Player )</param>
    public void GetTarget(Inventory inventory)
    {
        targetInventory = inventory; // target의 인벤토리 연결
    }


    /// <summary>
    /// 슬롯을 생성하는 함수
    /// </summary>
    void SetSlot()
    {
        Transform targetInventoryPanelUI = transform.GetChild(0);

        if(isFirst)
        {
            inventorySize = targetInventory.SlotSize;
            slots = new SellSlotUI[inventorySize];

            for(int i = 0; i < inventorySize; i++)
            {
                Instantiate(invenSlotPrefab, targetInventoryPanelUI);
            }
        }

        for(uint i = 0; i < inventorySize; i++)
        {
             SellSlotUI slotUI = targetInventoryPanelUI.GetChild((int)i).GetComponent<SellSlotUI>();
             slotUI.InitializeSlotUI(targetInventory[i]);
             slots[i] = slotUI;
        }        
    }
}