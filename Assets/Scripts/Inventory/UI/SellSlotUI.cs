using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlotUI : SlotUI_Base, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    SellPanelUI sellPanelUI;

    private void Start()
    {
        sellPanelUI = GameManager.Instance.ItemDataManager.SellPanelUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 판매창 뜨기
        sellPanelUI.onShowCheckPanel?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sellPanelUI.IsProcess)
            return;

        sellPanelUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);
        ShowHighlightSlotBorder(); // hightlight 활성화
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (sellPanelUI.IsProcess)
            return;

        sellPanelUI.onCloseDetail?.Invoke();
        HideHighlightSlotBorder(); // highlight 제거
    }
}