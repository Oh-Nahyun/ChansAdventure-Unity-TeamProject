using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                            IPointerClickHandler, 
                                            IPointerEnterHandler, IPointerExitHandler
{
    InventoryUI invenUI;

    void Start()
    {
        invenUI = ItemDataManager.Instance.InventoryUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // temp로 아이템 옮기기 (slot -> temp)
        invenUI.onSlotDragBegin?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"드래그 중 : {eventData}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        InventorySlot slot = obj.GetComponent<SlotUI_Base>().InventorySlotData;

        if (slot != null)
        {
            invenUI.onSlotDragEnd?.Invoke(slot.SlotIndex);
        }
        else
        {
            // 슬롯이 아니다
            invenUI.onSlotDragEndFail?.Invoke();
        }
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
            bool isEquipment = obj.GetComponent<SlotUI_Base>().InventorySlotData.SlotItemData is IEquipable; // 잘못된 접근
            if(isEquipment)
            {
                Debug.Log($"[{InventorySlotData.SlotItemData.name}]은 장비 입니다. < 클릭 O >");
            }

            // Key Q
            bool isPressedQ = Keyboard.current.qKey.ReadValue() > 0;

            if(isPressedQ) // dividUI 열기
            {

                if (InventorySlotData.CurrentItemCount <= 1)
                {
                    Debug.Log($"[{InventorySlotData.SlotItemData.itemName}]은 아이템이 [{InventorySlotData.CurrentItemCount}]개 있습니다.");
                    return;
                }
                else
                {
                    invenUI.onDivdItem(InventorySlotData.SlotIndex);
                }
            }
        }
        else
        {
            Debug.Log($"오브젝트가 없습니다.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        invenUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        invenUI.onCloseDetail?.Invoke();
    }
}
