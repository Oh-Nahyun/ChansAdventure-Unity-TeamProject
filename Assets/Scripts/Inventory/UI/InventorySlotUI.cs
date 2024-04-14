using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                            IPointerClickHandler, 
                                            IPointerEnterHandler, IPointerExitHandler
{
    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = ItemDataManager.Instance.InventoryUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // temp로 아이템 옮기기 (slot -> temp)
        inventoryUI.onSlotDragBegin?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"드래그 중 : {eventData}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;

        inventoryUI.onSlotDragEnd?.Invoke(obj);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;

        // 예외처리
        if(InventorySlotData.SlotItemData == null)
        {
            Debug.Log($"슬롯에 아이템이 없습니다.");
            return;
        }

        // OnPointerClick 이벤트 처리
        if (obj != null)
        {
            PointerEventData.InputButton buttonValue = eventData.button; // 무슨 클릭인지 확인하는 enum값
            //Debug.Log($"value : {buttonValue}");

            if(buttonValue == PointerEventData.InputButton.Left) // 왼쪽 클릭
            {
                inventoryUI.onLeftClickItem(InventorySlotData.SlotIndex);
            }
            else // 오른쪽 클릭
            {
                inventoryUI.onRightClickItem(InventorySlotData.SlotIndex, transform.position);
            }
        }
        else
        {
            Debug.Log($"오브젝트가 없습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);

        ShowHighlightSlotBorder(); // hightlight 활성화
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.onCloseDetail?.Invoke();

        HideHighlightSlotBorder(); // highlight 제거
    }
}
